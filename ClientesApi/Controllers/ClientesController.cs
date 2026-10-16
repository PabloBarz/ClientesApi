using ClientesApi.Data;
using ClientesApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController: ControllerBase
    {
        //Todas las     funcionalidades GET-POST-PUT-DELETE necesita acceso a la BD(ORM)

        private readonly AppDbContext _context;

        //Constrcuctor (Init)

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }   
        //GET : api/clientes -> retornar la lista de clientes 
        //async Task : Asincrono

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> Listar()
        {
            var clientes = await _context.Clientes.AsNoTracking().ToListAsync();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> ObtenerPorId(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if(cliente == null)
            {
                return NotFound(new { mensaje = $"No existe el cliente con ID: {id}" });
            }
            //200
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> Registrar([FromBody] Cliente cliente)
        {
            //Evita que el cliente envie un Id manualmente
            cliente.Id = 0;

            if (await _context.Clientes.AnyAsync(c => c.Dni == cliente.Dni))
            {

                //Server error 409
                return Conflict(new {mensaje = "Ya existe un cliente con ese DNI"});
            }

            //Procedemos con el registro
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            //Server Code 201 (Save ok) - Ejecutamos en segundo plano el buscador por Id, pasamos el Id de entrada, modelo 
            return CreatedAtAction(nameof(ObtenerPorId), new {id = cliente.Id}, cliente);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Cliente cliente)
        {
            //Validacion simple para el ID
            if(id != cliente.Id)
            {
                //Server code 400 
                return BadRequest(new {mensaje = "El ID de la URL  no coincide con el ID del cuerpo"});
            }

            //Validacion 2: ¿Exite el ID?
            var existente = await _context.Clientes.FindAsync(id);

            if (existente == null)
            {
                //Server Code 404
                return NotFound(new { mensaje = $"No existe cliente con id: {id}" });
            }

            //Validación 3: Conflicto DNI repetido y el ID no le pertenezca
            if (await _context.Clientes.AnyAsync(c => c.Dni == cliente.Dni && c.Id != id))
            {
                //Server Code 409 (error)
                return Conflict(new { mensaje = "Ya existe un cliente con ese DNI" });
            }

            //Procedemos a realizar la actualización...
            existente.Apellidos = cliente.Apellidos;
            existente.Nombres = cliente.Nombres;
            existente.Telefono = cliente.Telefono;
            existente.Correo = cliente.Correo;
            existente.Dni = cliente.Dni;
            existente.Direccion = cliente.Direccion;

            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();


            // Retorna un estado 200 OK o 204 No Content según lo que requiera tu API
            return Ok(new { mensaje = "Cliente actualizado correctamente", data = existente });

        }

        //DELETE: api/clientes/1 -> eliminar
        [HttpDelete("id:int")]
        public async Task<ActionResult> Eliminar(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if(cliente == null)
            {
                return NotFound(new { mensaje = $"No existe el cliente ID {id}" });
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}

