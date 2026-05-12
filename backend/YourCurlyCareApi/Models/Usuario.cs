using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace YourCurlyCareApi.Models;

[Table("usuario")]                          //nombre exacto de la tabla en la BD
//define que tanto username como email son unique 
[Index(nameof(Username), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]

public class Usuario
{
    [Key]                                   //indica que es la clave primaria
    [Column("id")]                          //nombre del atributo en la tabla de la base de datos
    public int Id { get; set; }

    [Required]                              //es obligatorio --> not null
    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Column("apellido")]
    public string? Apellido { get; set; }   //? opcional, no obligatorio

    [Required]
    [Column("username")]
    public string Username { get; set; } = null!;

    [Required]
    [Column("email")]
    [EmailAddress(ErrorMessage = "Introduce un correo electrónico válido")]
    public string Email { get; set; } = null!;

    [Required]
    [Column("password")]
    public string Password { get; set; } = null!;

    [Column("fecha_registro")]
    public DateTime? FechaRegistro { get; set; }

    [Column("email_confirmado")]
    public bool EmailConfirmado { get; set; } = false;

    [Column("codigo_verificacion")]
    public string? CodigoVerificacion { get; set; }

    [Column("rol")]
    public string Rol { get; set; } = "Usuario";

    [Column("cuenta_activa")]                           //para saber si se ha eliminado o no
    public bool CuentaActiva { get; set; } = true; 

    [Column("es_valida")]                               //para saber si ha validado la cuenta o no con el codigo de autenticacion
    public bool EsValida { get; set; } = false; 
}