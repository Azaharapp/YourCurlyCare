using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourCurlyCareApi.Models;

public enum Estado{Entregado, Confirmado, En_preparacio, Enviado, Cancelado, Devuelto, Fallido}

[Table("pedido")]

public class Pedido
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }     //campo tal cual de la BD
    [ForeignKey("IdUsuario")]               
    public virtual Usuario Usuario { get; set; } = null!;

    [Column("id_direccion")]
    public int IdDireccion { get; set; }     //campo tal cual de la BD
    [ForeignKey("IdDireccion")]               
    public virtual Direccion Direccion { get; set; } = null!;

    [Column("fecha_pedido")]
    public DateTime FechaPedido { get; set; }

    [Column("precio_final")]
    public decimal PrecioFinal { get; set; }

    [Column("gastos_envio")]
    public decimal GastosEnvio { get; set; }

    [Column("estado")]
    public Estado Estado { get; set; }
}