using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourCurlyCareApi.Models;

[Table("productoEscaner")]

public class ProductoEscaner
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("codigo_barras")]
    public string? CodigoBarras { get; set; }

    [Required]
    [Column("descripcion")]
    public string Descripcion { get; set; } = null!;

    [Required]
    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Column("alcohol")]
    public bool Alcohol { get; set; }

    [Column("silicona")]
    public bool Silicona { get; set; }
}
