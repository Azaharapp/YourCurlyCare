using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourCurlyCareApi.Models;

public enum Categoria { Secador, Toalla, PeineCepillo, Saten }

public enum Color { Negro, Blanco, Morado, Rosa, Verde }

[Table("productoVenta")]

public class ProductoVenta
{
    [Key]                                   //indica que es la clave primaria
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Required]
    [Column("descripcion")]
    public string Descripcion { get; set; } = null!;

    [Column("precio")]
    public decimal Precio { get; set; }

    [Column("imagen")]
    public string Imagen { get; set; } = null!;

    [Column("categoria")]
    public Categoria Categoria { get; set; }

    [Column("color")]
    public Color Color { get; set; }
    [Column("stock")]
    public int Stock { get; set; }

    [Column("peso")]
    public double Peso { get; set; }
}