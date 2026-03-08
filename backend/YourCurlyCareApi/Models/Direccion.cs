using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourCurlyCareApi.Models;

[Table("direccion")]

public class Direccion
{
    [Key]                                   //indica que es la clave primaria
    [Column("id")]
    public int Id { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }     //campo tal cual de la BD
    [ForeignKey("IdUsuario")]               
    //propiedad virtual de la clase Usuario - permite la relacion entre ambas clases
    public virtual Usuario Usuario { get; set; } = null!;

    [Column("pais")]
    public string Pais { get; set; } = null!;

    [Required]
    [Column("ciudad")]
    public string Ciudad { get; set; } = null!;


    [Required]
    [Column("calle")]
    public string Calle { get; set; } = null!;


    [Required]
    [Column("numero")]
    public int Numero { get; set; }

    [Required]
    [Column("cp")]
    public int Cp { get; set; }


    [Column("bloque")]
    public int? Bloque { get; set; }

    [Column("escalera")]
    public int? Escalera { get; set; }

    [Column("puerta")]
    public char? Puerta { get; set; }

}