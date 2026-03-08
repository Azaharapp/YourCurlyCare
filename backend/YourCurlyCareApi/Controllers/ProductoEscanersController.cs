using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourCurlyCareApi.Data;
using YourCurlyCareApi.Models;

namespace YourCurlyCareApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductoEscanersController : ControllerBase
{
    private readonly YourCurlyCareContext _context;

    public ProductoEscanersController(YourCurlyCareContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductoEscaner>>> GetProductoEscaners()
        => await _context.ProductoEscaners.ToListAsync();


    [HttpGet("{id}")]
    public async Task<ActionResult<ProductoEscaner>> GetProductoEscaner(int id)
    {
        var productoEscaner = await _context.ProductoEscaners.FindAsync(id);

        if (productoEscaner == null) return NotFound();

        return productoEscaner;
    }
}

