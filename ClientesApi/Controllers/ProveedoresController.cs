
using ClientesApi.Data;
using ClientesApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientesApi.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Constructor
        public ProveedoresController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/proveedores
        // GET: api/proveedores?buscar=tecnologia
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proveedor>>> Listar(
            [FromQuery] string? buscar)
        {
            var consulta = _context.Proveedores.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                var texto = buscar.Trim();

                consulta = consulta.Where(p =>
                    p.RazonSocial.Contains(texto) ||
                    p.Ruc.Contains(texto));
            }

            var lista = await consulta
                .OrderBy(p => p.RazonSocial)
                .Take(200)
                .ToListAsync();

            return Ok(lista);
        }

        // GET: api/proveedores/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Proveedor>> ObtenerPorId(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);

            if (proveedor == null)
            {
                return NotFound(new
                {
                    mensaje = $"No existe el proveedor con ID: {id}"
                });
            }

            return Ok(proveedor);
        }

        // POST: api/proveedores
        [HttpPost]
        public async Task<ActionResult<Proveedor>> Registrar(
            [FromBody] Proveedor proveedor)
        {
            // Evita que el usuario envíe un ID manualmente
            proveedor.Id = 0;

            // Validar RUC duplicado
            if (await _context.Proveedores.AnyAsync(
                p => p.Ruc == proveedor.Ruc))
            {
                return Conflict(new
                {
                    mensaje = "Ya existe un proveedor con ese RUC"
                });
            }

            // Registrar proveedor
            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync();

            // HTTP 201 Created
            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = proveedor.Id },
                proveedor);
        }

        // PUT: api/proveedores/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(
            int id, [FromBody] Proveedor proveedor)
        {
            // Validar que coincidan los ID
            if (id != proveedor.Id)
            {
                return BadRequest(new
                {
                    mensaje = "El ID de la URL no coincide con el ID del cuerpo"
                });
            }

            // Verificar si existe el proveedor
            var existente = await _context.Proveedores.FindAsync(id);

            if (existente == null)
            {
                return NotFound(new
                {
                    mensaje = $"No existe proveedor con ID: {id}"
                });
            }

            // Validar RUC duplicado en otro proveedor
            if (await _context.Proveedores.AnyAsync(
                p => p.Ruc == proveedor.Ruc && p.Id != id))
            {
                return Conflict(new
                {
                    mensaje = "Ya existe un proveedor con ese RUC"
                });
            }

            // Actualizar los campos
            existente.RazonSocial = proveedor.RazonSocial;
            existente.Ruc = proveedor.Ruc;
            existente.Telefono = proveedor.Telefono;
            existente.Correo = proveedor.Correo;
            existente.Direccion = proveedor.Direccion;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Proveedor actualizado correctamente",
                data = existente
            });
        }

        // DELETE: api/proveedores/1
        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);

            if (proveedor == null)
            {
                return NotFound(new
                {
                    mensaje = $"No existe el proveedor con ID: {id}"
                });
            }

            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
