using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourCurlyCareApi.Models;

public enum Metodo { Tarjeta, BIzum, PayPal, Tranferencia, Contrareembolso }
public enum EstadoPago { Pendiente, Pagado, Reembolsado, Rechazado }


[Table("pago")]

public class Pago
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_pedido")]
    public int IdPedido { get; set; }
    [ForeignKey("IdPedido")]
    public virtual Pedido Pedido { get; set; } = null!;

    [Column("metodo")]
    public Metodo Metodo { get; set; }

    [Column("estado")]
    public EstadoPago Estado { get; set; }

    [Column("fecha_pago")]
    public DateTime FechaPago { get; set; }

    [Column("cantidad")]
    public int Cantidad { get; set; }
}