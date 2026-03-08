using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourCurlyCareApi.Models;

[Table("consultaIA")]

public class ConsultaIA
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }     //campo tal cual de la BD
    [ForeignKey("IdUsuario")]
    public virtual Usuario Usuario { get; set; } = null!;

    [Column("id_modeloIA")]
    public int IdModeloIA { get; set; }
    [ForeignKey("IdModeloIA")]
    public virtual ModeloIA ModeloIA { get; set; } = null!;
    
    [Column("pregunta")]
    public string? Pregunta { get; set; }   //? opcional, no obligatorio

    [Column("repuesta")]
    public string? Respuesta { get; set; }

    [Column("fecha_consulta")]
    public DateTime? FechaConsulta { get; set; }

}
