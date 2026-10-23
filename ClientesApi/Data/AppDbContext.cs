using ClientesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientesApi.Data
{
    public class AppDbContext : DbContext
    {
        //Constructor(Inicializar)

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        //ORM tendrá acceso  al modelo cliente
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
    }
}
