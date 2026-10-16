using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientesApi.Models
{
    [Table("Productos")]
    public class Producto
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required, StringLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; }

        [Required, StringLength(100)]
        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Required]
        [Column("precio", TypeName = "decimal(7,2)")]
        public decimal Precio { get; set; }

        [Required]
        [Column("garantia")]
        public byte Garantia { get; set; } // byte equivale a TINYINT en SQL Server

        [StringLength(40)]
        [Column("numserie")]
        public string? Numserie { get; set; } // El signo '?' permite que sea NULL en C#
    }
}