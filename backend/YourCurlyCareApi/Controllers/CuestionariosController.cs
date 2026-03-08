using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourCurlyCareApi.Data;
using YourCurlyCareApi.Models;

namespace YourCurlyCareApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CuestionariosController : ControllerBase
{
    private readonly YourCurlyCareContext _context;

    public CuestionariosController(YourCurlyCareContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cuestionario>>> GetCuestionarios()
    {
        return await _context.Cuestionarios.ToListAsync();
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Cuestionario>> GetCuestionario(int id)
    {
        var cuestionario = await _context.Cuestionarios.FindAsync(id);

        if (cuestionario == null) return NotFound();

        return cuestionario;
    }
}


