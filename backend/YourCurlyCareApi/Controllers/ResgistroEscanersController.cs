using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourCurlyCareApi.Data;
using YourCurlyCareApi.Models;

namespace YourCurlyCareApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ResgistroEscanersController : ControllerBase
{
    private readonly YourCurlyCareContext _context;

    public ResgistroEscanersController(YourCurlyCareContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RegistroEscaner>>> GetRegistroEscaners()
        => await _context.RegistroEscaners.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<RegistroEscaner>> GetRegistroEscaner(int id)
    {
        var registroEscaner = await _context.RegistroEscaners.FindAsync(id);

        if (registroEscaner == null) return NotFound();

        return registroEscaner;
    }
}

