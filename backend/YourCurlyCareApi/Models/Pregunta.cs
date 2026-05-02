using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourCurlyCareApi.Models;

[Table("pregunta")]

public class Pregunta
{
  [Key]
  [Column("id")]
  public int Id { get; set; }

  [Column("id_cuestionario")]
  public int IdCuestionario { get; set; }                         //campo tal cual de la BD
  [ForeignKey("IdCuestionario")]
  public virtual Cuestionario Cuestionario { get; set; } = null!;

  [Required]
  [Column("enunciado")]
  public string Enunciado { get; set; } = null!;

  [Column("opcion_a")]
  public string OpcionA { get; set; } = null!;

  [Column("opcion_b")]
  public string OpcionB { get; set; } = null!;

  [Column("opcion_c")]
  public string OpcionC { get; set; } = null!;
}