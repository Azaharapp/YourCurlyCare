using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourCurlyCareApi.Data;
using YourCurlyCareApi.Models;

namespace YourCurlyCareApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PagosController : ControllerBase
{
    private readonly YourCurlyCareContext _context;

    public PagosController(YourCurlyCareContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pago>>> GetPagos()
    {
        return await _context.Pagos.ToListAsync();
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Pago>> GetPago(int id)
    {
        var pago = await _context.Pagos.FindAsync(id);

        if (pago == null) return NotFound();

        return pago;
    }
}

