using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourCurlyCareApi.Models;
using YourCurlyCareApi.Data;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace YourCurlyCareApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase            //clase heredada de ControllerBase
{
    private readonly YourCurlyCareContext _context;         //atributo privado de solo lectura de tipo YourCurlyCareContext
    private readonly IConfiguration _config;

    //constructor
    public UsuariosController(YourCurlyCareContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpGet]                                               //permite usar el metodo get  --> lee
    //metodo asincrono(async) que devuelve una promesa(Task):
    //ActionResult: devuelve codigos de estado HTTP 
    //IEnumerable: permite devolver una lista
    public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
    {
        return await _context.Usuarios.ToListAsync();       //devuelve todos los usuarios en una lista asincrona(await)
    }

    [HttpGet("{id}")]                                       // buscar usuario por Id
    public async Task<ActionResult<Usuario>> GetUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null) return NotFound();

        return usuario;
    }

    [HttpPost]                                                  //registrar un nuevo uuario --> metodo post
    public async Task<ActionResult<Usuario>> RegistrarUsuario(Usuario usuario)
    {
        //variable almacena booleano si el email existe o no 
        var emailExiste = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == usuario.Email);

        if (emailExiste != null)
        {
            if (emailExiste.CuentaActiva) return BadRequest("Este email ya está registrado.");

            //si usuario quiere volver a crearse una cuanta despues de eliminarla, seguimos manteniendo sus antiguos datos
            emailExiste.Username = usuario.Username;            // por si quiere cambiar el nombre
            emailExiste.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
            emailExiste.FechaRegistro = DateTime.Now;           // actualizamos a la nueva fecha
            emailExiste.EmailConfirmado = false;
            emailExiste.CodigoVerificacion = new Random().Next(100000, 999999).ToString();

            await _context.SaveChangesAsync();
            EnviarEmailConfirmacion(emailExiste.Email, emailExiste.CodigoVerificacion);

            return Ok(new { requiereConfirmacion = true, mensaje = "Verifica tu email." });
        }

        //registro de usuario normal - si haber eliminado la cuenta
        //encripta contraseña de usuario
        usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
        usuario.FechaRegistro = DateTime.Now;
        usuario.EmailConfirmado = false;
        usuario.CuentaActiva = true;
        usuario.CodigoVerificacion = new Random().Next(100000, 999999).ToString();

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        EnviarEmailConfirmacion(usuario.Email, usuario.CodigoVerificacion);

        return Ok(new { requiereConfirmacion = true });
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginRequest request)
    {
        //buscar al usuario por su email
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (usuario == null || !usuario.CuentaActiva) return BadRequest("La cuenta ha sido desactivada.");

        if (!usuario.EmailConfirmado) return BadRequest("Debes confirmar tu email antes de iniciar sesión.");

        bool passwordValida = BCrypt.Net.BCrypt.Verify(request.Password, usuario.Password);
        if (!passwordValida) return BadRequest("Credenciales incorrectas. Parece que la contraseña no es correcta");

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            //SameSite = SameSiteMode.Strict
            SameSite = SameSiteMode.None,
            Path = "/"
        };

        // si se marca recordar, la sesion dura 7 dias, si se marca la sision muere al cerrar el navegador
        if (request.Recordar) cookieOptions.Expires = DateTime.Now.AddDays(7);

        Response.Cookies.Append("usuario_actividad", usuario.Id.ToString(), cookieOptions);

        string tokenFinal = GenerarTokenJwt(usuario);

        return Ok(new
        {
            token = tokenFinal,
            username = usuario.Username,
            rol = usuario.Rol,
            userId = usuario.Id
        });
    }

    private string GenerarTokenJwt(Usuario usuario)
    {
        SymmetricSecurityKey claveSegura = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        SigningCredentials credenciales = new SigningCredentials(claveSegura, SecurityAlgorithms.HmacSha256);

        Claim[] datosUsuario = new Claim[]                  //datos que iran dentro del token
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Name, usuario.Nombre)
        };

        //estructura del token
        JwtSecurityToken tokenJwt = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: datosUsuario,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: credenciales);

        return new JwtSecurityTokenHandler().WriteToken(tokenJwt);
    }

    [HttpPost("confirmar")]
    public async Task<IActionResult> ConfirmarEmail([FromBody] ConfirmarRequest request)
    {
        try
        {
            if (request == null || string.IsNullOrEmpty(request.Codigo))
                return BadRequest("El código es obligatorio.");

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.CodigoVerificacion == request.Codigo.Trim());

            if (usuario == null) return BadRequest("El código es incorrecto.");

            usuario.EmailConfirmado = true;
            usuario.CuentaActiva = true;
            usuario.CodigoVerificacion = null;

            _context.Entry(usuario).Property(u => u.EmailConfirmado).IsModified = true;
            _context.Entry(usuario).Property(u => u.CodigoVerificacion).IsModified = true;

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Ya puedes iniciar sesión" });
        }
        catch (Exception e)
        {
            var errorReal = e.InnerException != null ? e.InnerException.Message : e.Message;

            return StatusCode(500, new { error = "Error de base de datos", detalle = errorReal });
        }
    }

    private void EnviarEmailConfirmacion(string emailDestino, string codigo)
    {
        var emailEmisor = _config["EmailSettings:User"];
        var passwordEmisor = _config["EmailSettings:Pass"];

        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(emailEmisor, passwordEmisor),
            EnableSsl = true,
        };

        var mensaje = new MailMessage
        {
            From = new MailAddress(emailEmisor, "YourCurlyCare"),
            Subject = "Confirma tu cuenta de YourCurlyCare",
            Body = $@"
            <html>
                <body>
                    <h2>¡Bienvenida a YourCurlyCare!</h2>
                    <p>Para activar tu cuenta, introduce el siguiente código en el apartado de validación de la web:</p>
                    <h1 style='color: #d8a481;'>{codigo}</h1>
                    <p>Si no te has registrado, puedes ignorar este correo.</p>
                </body>
            </html>",
            IsBodyHtml = true,
        };

        mensaje.To.Add(emailDestino);
        smtpClient.Send(mensaje);
    }

    [HttpPost("olvidar-pass")]
    public async Task<IActionResult> OlvidePassword([FromBody] OlvideRequest request)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (usuario == null) return Ok(new { mensaje = "Comprueba tu email" });

        usuario.CodigoVerificacion = new Random().Next(100000, 999999).ToString();
        await _context.SaveChangesAsync();

        EnviarEmailRecuperacion(usuario.Email, usuario.CodigoVerificacion);
        return Ok(new { mensaje = "Código de recuperación enviado." });
    }

    [HttpPost("reset-pass")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == request.Email && u.CodigoVerificacion == request.Codigo);

        if (usuario == null) return BadRequest("Código o email incorrectos.");

        usuario.Password = BCrypt.Net.BCrypt.HashPassword(request.NuevaPassword);
        usuario.CodigoVerificacion = null;

        await _context.SaveChangesAsync();
        return Ok(new { mensaje = "Contraseña actualizada con éxito." });
    }


    private void EnviarEmailRecuperacion(string emailDestino, string codigo)
    {
        var emailEmisor = _config["EmailSettings:User"];
        var passwordEmisor = _config["EmailSettings:Pass"];

        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(emailEmisor, passwordEmisor),
            EnableSsl = true,
        };

        var mensaje = new MailMessage
        {
            From = new MailAddress(emailEmisor, "YourCurlyCare"),
            Subject = "Recupera tu contraseña de YourCurlyCare",
            Body = $@"
            <html>
                <body>
                    <h2>Restablecer contraseña</h2>
                    <p>Has solicitado cambiar tu contraseña. Introduce el siguiente código en el apartado de validación de la web:</p>
                    <h1 style='color: #d8a481;'>{codigo}</h1>
                    <p>Si no has sido tú, ignora este mensaje.</p>
                </body>
            </html>",
            IsBodyHtml = true,
        };

        mensaje.To.Add(emailDestino);
        smtpClient.Send(mensaje);
    }

    [HttpPut("{id}")]                                       //editar los usuarios en el panel admin
    public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] System.Text.Json.JsonElement datosRecibidos)
    {
        var usuarioExistente = await _context.Usuarios.FindAsync(id);
        if (usuarioExistente == null) return NotFound();

        //si el campo existe, guarda el contenido en la variable, creando una temporal con lo que ha escrito el usuario
        if (datosRecibidos.TryGetProperty("nombre", out var nombre))
            usuarioExistente.Nombre = nombre.GetString();

        if (datosRecibidos.TryGetProperty("apellido", out var apellido))
            usuarioExistente.Apellido = apellido.GetString();

        if (datosRecibidos.TryGetProperty("username", out var username))
            usuarioExistente.Username = username.GetString();

        if (datosRecibidos.TryGetProperty("email", out var email))
            usuarioExistente.Email = email.GetString();

        await _context.SaveChangesAsync();
        return NoContent();                                 //si todo va bien no devuelve nada
    }

    [HttpDelete("{id}")]                                    //eliminar cuenta usuario desde el panel de administrador
    public async Task<IActionResult> EliminarUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return NotFound();

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [Authorize]
    [HttpPost("solicitar-eliminacion")]
    public async Task<IActionResult> SolicitarEliminacion()
    {
        try
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized(new { mensaje = "No autorizado" });

            int userId = int.Parse(userIdClaim.Value);

            var usuario = await _context.Usuarios.FindAsync(userId);
            if (usuario == null) return NotFound(new { mensaje = "Usuario no encontrado" });

            usuario.CodigoVerificacion = new Random().Next(100000, 999999).ToString();
            await _context.SaveChangesAsync();

            EnviarEmailEliminacion(usuario.Email, usuario.CodigoVerificacion);

            return Ok(new { mensaje = "Se ha enviado un código a tu email." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno", detalle = ex.Message });
        }
    }

    [HttpPost("confirmar-eliminacion")]
    public async Task<IActionResult> ConfirmarEliminacion([FromBody] string codigo)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.CodigoVerificacion == codigo);
        if (usuario == null) return BadRequest("Código incorrecto.");

        usuario.CuentaActiva = false;
        usuario.CodigoVerificacion = null;
        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Cuenta desactivada correctamente." });
    }

    private void EnviarEmailEliminacion(string emailDestino, string codigo)
    {
        var emailEmisor = _config["EmailSettings:User"];
        var passwordEmisor = _config["EmailSettings:Pass"];

        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(emailEmisor, passwordEmisor),
            EnableSsl = true,
        };

        var mensaje = new MailMessage
        {
            From = new MailAddress(emailEmisor, "YourCurlyCare"),
            Subject = "Eliminación de tu cuenta en YourCurlyCare",
            Body = $@"
            <html>
                <body>
                    <h2>Eliminación de cuenta</h2>
                    <p>Has solicitado la eliminacion de la tu cuenta en la plataforma YourCurlyCare. Introduce el siguiente código en el apartado de validación de la web:</p>
                    <h1 style='color: #d8a481;'>{codigo}</h1>
                    <p>Si no has sido túo no quieres eliminarla, ignora este mensaje.</p>
                </body>
            </html>",
            IsBodyHtml = true,
        };

        mensaje.To.Add(emailDestino);
        smtpClient.Send(mensaje);
    }
}