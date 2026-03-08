using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourCurlyCareApi.Models;

[Table("modeloIA")]

public class ModeloIA
{

    [Key]                                   //indica que es la clave primaria
    [Column("id")]
    public int Id { get; set; }

    [Column("nombre")]
    public string? Nombre { get; set; }

    [Column("descripcion")]
    public string? Descripcion { get; set; }   //? opcional, no obligatorio

    [Column("version")]
    public double? Version { get; set; }

    [Column("fecha_entrenamiento")]
    public DateTime? FechaEntrenamiento { get; set; }
}
