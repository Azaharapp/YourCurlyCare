using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace YourCurlyCareApi.Models;

[Table("respuesta")]

public class Respuesta
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }                                          //campo tal cual de la BD
    [ForeignKey("IdUsuario")]
    public virtual Usuario Usuario { get; set; } = null!;

    [Column("id_cuestionario")]
    public int IdCuestionario { get; set; }                                     //campo tal cual de la BD
    [ForeignKey("IdCuestionario")]
    public virtual Cuestionario Cuestionario { get; set; } = null!;

    [Column("id_pregunta")]
    public int IdPregunta { get; set; }                                         //campo tal cual de la BD
    [ForeignKey("IdPregunta")]
    public virtual Pregunta Pregunta { get; set; } = null!;

    [Column("opcion")]
    public string Opcion { get; set; } = null!;

    [Column("fecha_realizacion")]
    public DateTime FechaRealizacion { get; set; }
}