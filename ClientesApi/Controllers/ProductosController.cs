using ClientesApi.Data;
using ClientesApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> Listar()
        {
            var productos = await _context.Productos.AsNoTracking().ToListAsync();
            return Ok(productos);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<Producto>> obtenerPorId(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if(producto == null) return NotFound(new {mensaje = $"Producto no existe con el ID: ${id}"});
            
            return Ok(producto);
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> Registrar([FromBody] Producto producto)
        {
            producto.Id = 0;
            if (producto.Numserie != null && await _context.Productos.AnyAsync(p => p.Numserie == producto.Numserie))
            {
                return Conflict(new { mensaje = "Ya existe un producto con ese NumSerie" });
            }

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(obtenerPorId), new { id = producto.Id, producto });
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Actualizar(int id, [FromBody] Producto producto)
        {
            if(id != producto.Id)
            {
                return BadRequest(new {mensaje = "El id de la URL no coincide con el del cuerpo"});
            }

            var existente = await _context.Productos.FindAsync(id);

            if(existente == null) return BadRequest(new {mensaje = $"No existe producto con id ${id}"});

            if (producto.Numserie != null && await _context.Productos.AnyAsync(p => p.Numserie == producto.Numserie && p.Id != id))
            {
                return Conflict(new { mensaje = "Ya existe un producto con ese NumSerie" });
            }

            //Procedemos a realizar la actualizacion 
            // Procedemos a realizar la actualizacion 
            existente.Nombre = producto.Nombre;
            existente.Descripcion = producto.Descripcion;
            existente.Precio = producto.Precio;
            existente.Garantia = producto.Garantia;
            existente.Numserie = producto.Numserie;

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Producto actualizado correctamente", data = existente });
        }

        // DELETE: api/productos/1 -> eliminar producto
        [HttpDelete("{id:int}")] // Agregado el método de eliminación que faltaba
        public async Task<ActionResult> Eliminar(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound(new { mensaje = $"No existe el producto con ID {id}" });
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
 
        }


    }
}
