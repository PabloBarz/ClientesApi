using ClientesApi.Data;
using ClientesApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using static ClientesApi.Models.AuthDtos;

namespace ClientesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // POST: api/auth/registrar
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegistroRequest request)
        {
            // Nota: Si tu DbSet en el AppDbContext se llama en singular, cámbialo a _context.Usuario
            if (await _context.Usuario.AnyAsync(u => u.Username == request.Username))
            {
                return Conflict(new { mensaje = "El usuario ya existe" });
            }

            var usuario = new Usuario
            {
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Rol = "Usuario"
            };

            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Usuario registrado correctamente" });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
            {
                return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos" });
            }

            var (token, expira) = GenerarToken(usuario);
            return Ok(new { token, expira });
        }

        private (string token, DateTime expira) GenerarToken(Usuario usuario)
        {
            var jwt = _config.GetSection("jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));

            var expira = DateTime.UtcNow.AddMinutes(double.Parse(jwt["ExpireMinutes"]!));

            var description = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Username),
                    new Claim(ClaimTypes.Role, usuario.Rol),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Issuer = jwt["Issuer"],
                Audience = jwt["Audience"],
                Expires = expira,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            var token = new JsonWebTokenHandler().CreateToken(description);
            return (token, expira);
        }
    }
}