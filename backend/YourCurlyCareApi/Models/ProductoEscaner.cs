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

    [Required]
    [Column("codigo_barras")]
    public string CodigoBarras { get; set; } = null!;

    [Required]
    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Required]
    [Column("marca")]
    public string Marca { get; set; } = null!;

    [Required]
    [Column("ingredientes")]
    public string Ingredientes { get; set; } = null!;

    [Required]
    [Column("alcohol")]
    public bool Alcohol { get; set; }

    [Required]
    [Column("silicona")]
    public bool Silicona { get; set; }

    [Required]
    [Column("sulfato")]
    public bool Sulfato { get; set; }

    [Required]
    [Column("es_apto")]
    public bool EsApto { get; set; }

    [Required]
    [Column("id_usuario")]
    public int IdUsuario { get; set; }
    [ForeignKey("IdUsuario")]
    public virtual Usuario Usuario { get; set; } = null!;

}
