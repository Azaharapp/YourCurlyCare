using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourCurlyCareApi.Models;

[Table("productoV_pedido")]

public class ProductoVPedido
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_pedido")]
    public int IdPedido { get; set; }
    [ForeignKey("IdPedido")]
    public virtual Pedido Pedido { get; set; } = null!;

    [Column("id_productoV")]
    public int IdProductoV { get; set; }
    [ForeignKey("IdProductoV")]
    public virtual ProductoVenta ProductoVenta { get; set; } = null!;

    [Column("cantidad")]
    public int Cantidad { get; set; }
}