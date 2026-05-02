using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourCurlyCareApi.Models;

[Table("registroEscaner")]

public class RegistroEscaner
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_productoE")]
    public int IdProductoE { get; set; }
    [ForeignKey("IdProductoE")]
    public virtual ProductoEscaner ProductoEscaner { get; set; } = null!;

    [Column("id_usuario")]
    public int IdUsuario { get; set; }                                      //campo tal cual de la BD
    [ForeignKey("IdUsuario")]
    public virtual Usuario Usuario { get; set; } = null!;

    [Column("fecha_escaner")]
    public DateTime FechaEscaner { get; set; }

    [Column("respuesta")]
    public bool Respuesta { get; set; }
}
