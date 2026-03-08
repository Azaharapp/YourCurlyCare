using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourCurlyCareApi.Models;


[Table("carrito")]

public class Carrito
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }     
    [ForeignKey("IdUsuario")]
    public virtual Usuario Usuario { get; set; } = null!;
}