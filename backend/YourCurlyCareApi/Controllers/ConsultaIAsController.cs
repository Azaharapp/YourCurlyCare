using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourCurlyCareApi.Data;
using YourCurlyCareApi.Models;

namespace YourCurlyCareApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConsultaIAsController : ControllerBase
{
    private readonly YourCurlyCareContext _context;

    public ConsultaIAsController(YourCurlyCareContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ConsultaIA>>> GetConsultasIAs()
        => await _context.ConsultaIAs.ToListAsync();


    [HttpGet("{id}")]
    public async Task<ActionResult<ConsultaIA>> GetConsultaIA(int id)
    {
        var consultaIA = await _context.ConsultaIAs.FindAsync(id);

        if (consultaIA == null) return NotFound();

        return consultaIA;
    }
}

