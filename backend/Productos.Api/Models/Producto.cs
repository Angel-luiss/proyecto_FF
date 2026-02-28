using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Productos.Api.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(20)]
        public string Codigo { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        [Required]
        public int Stock { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}