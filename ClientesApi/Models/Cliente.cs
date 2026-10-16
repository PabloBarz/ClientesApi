using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientesApi.Models
{
    //Las anotaciones, definen los roles de los objetos;
    [Table("clientes")]
    public class Cliente
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required, StringLength(70)]
        [Column("apellidos")]
        public string Apellidos { get; set; }

        [Required, StringLength(70)]
        [Column("nombres")]
        public string Nombres { get; set; }

        [Required, StringLength(9)]
        [Column("telefono")]
        public string Telefono { get; set; }

        [Required, StringLength(150)]
        [Column("correo")]
        public string Correo { get; set; }

        [Required, StringLength(8)]
        [Column("dni")]
        [RegularExpression(@"^[0-9]{8}$", ErrorMessage = "El DNI debe contener exactamente 8 dígitos numéricos.")]
        public string Dni { get; set; }

        [Required, StringLength(150)]
        [Column("direccion")]
        public string Direccion { get; set; }

    }
}
