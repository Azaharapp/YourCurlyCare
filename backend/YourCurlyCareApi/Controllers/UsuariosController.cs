using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourCurlyCareApi.Models;
using YourCurlyCareApi.Data;

namespace YourCurlyCareApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase            //clase heredada de ControllerBase
{
    private readonly YourCurlyCareContext _context;         //atributo privado de solo lectura de tipo YourCurlyCareContext

    public UsuariosController(YourCurlyCareContext context) // constructor
    {
        _context = context;
    }


    [HttpGet]                                               //permite usar el metodo get  --> lee
    //metodo asincrono(async) que devuelve una promesa(Task):
    //ActionResult devuelve codigos de estado HTTP 
    //IEnumerable permite devolver una lista
    public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
    {
        return await _context.Usuarios.ToListAsync();       //devuelve todos los usuarios en una lista asincrona(await)
    }


    // buscar usuario por Id
    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null) return NotFound();

        return usuario;
    }


    //registrar un nuevo uuario --> metodo post
    [HttpPost]
    public async Task<ActionResult<Usuario>> RegistrarUsuario(Usuario usuario)
    {
        /* SQL Puro --> Es igual que lo de abajo 
        var emailCount = await _context.Database.SqlQueryRaw<int>(
            "SELECT COUNT(*) as Value FROM usuarios WHERE email = {0}", 
            usuario.Email
        ).SingleAsync();*/

        // version con funciones de Entity Framework  - Hago esta porque es más segura
        //variable almacena boolean si el email existe o no 
        var emailExiste = await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email);

        //si el email ya existe, lanza error --> no es mejor hacer un throw new error, es lo que yo vi en clase o eso es solo por consola?
        if (emailExiste) return BadRequest("Este email ya está registrado.");

        //encriptar contraseña de usuario
        usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);               

        /*await _context.Database.ExecuteSqlRawAsync(
            "INSERT INTO usuarios (nombre, apellido, username, email, password, fecha_registro) VALUES ({0}, {1}, {2}, {3}, {4}, {5})", 
            usuario.Nombre, usuario.Apellido, usuario.Username, usuario.Email, usuario.Password, usuario.FechaRegistro
        ); */
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
    }

}