using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourCurlyCareApi.Models;


[Table("productoV_carrito")]

public class ProductoVCarrito
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_carrito")]
    public int IdCarrito { get; set; }
    [ForeignKey("IdCarrito")]
    public virtual Carrito Carrito { get; set; } = null!;
    
    [Column("id_productoV")]
    public int IdProductoV { get; set; }
    [ForeignKey("IdProductoV")]
    public virtual ProductoVenta ProductoVenta { get; set; } = null!;

    [Column("cantidad")]
    public int Cantidad { get; set; }
}