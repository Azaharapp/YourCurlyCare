using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourCurlyCareApi.Data;
using YourCurlyCareApi.Models;

namespace YourCurlyCareApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PreguntasController : ControllerBase
{
    private readonly YourCurlyCareContext _context;

    public PreguntasController(YourCurlyCareContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pregunta>>> GetPreguntas() => await _context.Preguntas.ToListAsync();


    [HttpGet("{id}")]
    public async Task<ActionResult<Pregunta>> GetPregunta(int id)
    {
        var pregunta = await _context.Preguntas.FindAsync(id);

        if (pregunta == null) return NotFound();

        return pregunta;
    }
}

