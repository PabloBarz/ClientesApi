using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientesApi.Models
{
    [Table("proveedores")]
    public class Proveedor
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required, StringLength(120)]
        [Column("razon_social")]
        public string RazonSocial { get; set; }

        [Required, StringLength(11)]
        [Column("ruc")]
        [RegularExpression(@"^[0-9]{11}$",
            ErrorMessage = "El RUC debe contener exactamente 11 dígitos numéricos.")]
        public string Ruc { get; set; }

        [Required, StringLength(9)]
        [Column("telefono")]
        public string Telefono { get; set; }

        [Required, StringLength(150)]
        [Column("correo")]
        public string Correo { get; set; }

        [Required, StringLength(150)]
        [Column("direccion")]
        public string Direccion { get; set; }
    }
}