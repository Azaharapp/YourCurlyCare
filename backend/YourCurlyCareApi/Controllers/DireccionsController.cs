using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourCurlyCareApi.Data;
using YourCurlyCareApi.Models;

namespace YourCurlyCareApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DireccionsController : ControllerBase
{
    private readonly YourCurlyCareContext _context;

    public DireccionsController(YourCurlyCareContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Direccion>>> GetDireccions()
    {
        return await _context.Direccions.ToListAsync();
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Direccion>> GetDireccion(int id)
    {
        var direccion = await _context.Direccions.FindAsync(id);

        if (direccion == null) return NotFound();

        return direccion;
    }
}

