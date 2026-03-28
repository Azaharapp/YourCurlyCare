using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourCurlyCareApi.Models;

[Table("resultado")]
public class Resultado
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_cuestionario")]
    public int IdCuestionario { get; set; }                             //campo tal cual de la BD
    [ForeignKey("IdCuestionario")]
    public virtual Cuestionario Cuestionario { get; set; } = null!;

    [Column("resultado")]
    public string? Enunciado { get; set; }
    
    [Column("fecha_realizacion")]
    public DateTime FechaRealizacion { get; set; }

}
