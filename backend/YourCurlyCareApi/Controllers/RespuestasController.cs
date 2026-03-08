using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourCurlyCareApi.Data;
using YourCurlyCareApi.Models;

namespace YourCurlyCareApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RespuestasController : ControllerBase
{
    private readonly YourCurlyCareContext _context;

    public RespuestasController(YourCurlyCareContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Respuesta>>> GetRespuestas() => await _context.Respuestas.ToListAsync();


    [HttpGet("{id}")]
    public async Task<ActionResult<Respuesta>> GetRespuesta(int id)
    {
        var respuesta = await _context.Respuestas.FindAsync(id);

        if (respuesta == null) return NotFound();

        return respuesta;
    }
}

