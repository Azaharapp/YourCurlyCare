using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourCurlyCareApi.Models;

public enum EstadoCuestionario {Borrado, Entregado, Sin_contestar}

[Table("cuestionario")]

public class Cuestionario
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("n_pregunta")]
    public int? NPregunta { get; set; }

    [Column("fecha_creacion")]
    public DateTime FechaCreacion { get; set; }

    [Column("estado")]
    public EstadoCuestionario Estado { get; set; }
}