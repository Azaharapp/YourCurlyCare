using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourCurlyCareApi.Data;
using YourCurlyCareApi.Models;

namespace YourCurlyCareApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ModeloIAsController : ControllerBase
{
    private readonly YourCurlyCareContext _context;

    public ModeloIAsController(YourCurlyCareContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ModeloIA>>> GetModeloIAs()
        => await _context.ModeloIAs.ToListAsync();


    [HttpGet("{id}")]
    public async Task<ActionResult<ModeloIA>> GetModeloIA(int id)
    {
        var modeloIA = await _context.ModeloIAs.FindAsync(id);

        if (modeloIA == null) return NotFound();

        return modeloIA;
    }
}

