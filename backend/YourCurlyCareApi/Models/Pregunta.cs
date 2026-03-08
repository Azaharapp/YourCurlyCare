using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourCurlyCareApi.Models;

[Table("pregunta")]

public class Pregunta
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }     //campo tal cual de la BD
    [ForeignKey("IdUsuario")]
    public virtual Usuario Usuario { get; set; } = null!;

    [Column("id_cuestionario")]
    public int IdCuestionario { get; set; }     //campo tal cual de la BD
    [ForeignKey("IdCuestionario")]
    public virtual Cuestionario Cuestionario { get; set; } = null!;

    [Required]
    [Column("enunciado")]
    public string Enunciado { get; set; } = null!;


}