using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourCurlyCareApi.Models;
using YourCurlyCareApi.Data;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        var emailExiste = await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email);

        if (emailExiste) return BadRequest("Este email ya está registrado.");

        //encripta contraseña de usuario
        usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);

        usuario.FechaRegistro = DateTime.Now;
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
    }

    [HttpPost("login")]                                     //inicio de sesion
    public async Task<ActionResult> Login(LoginRequest request)
    {
        //buscar al usuario por su email
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (usuario == null) return BadRequest("Credenciales incorrectas. Parece que el email no es correcto.");

        bool passwordValida = BCrypt.Net.BCrypt.Verify(request.Password, usuario.Password);
        if (!passwordValida) return BadRequest("Credenciales incorrectas. Parece que la contraseña no es correcta");

        string tokenFinal = GenerarTokenJwt(usuario);
        return Ok(new
        {
            mensaje = "¡Login exitoso!",
            token = tokenFinal,
            username = usuario.Username
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
}