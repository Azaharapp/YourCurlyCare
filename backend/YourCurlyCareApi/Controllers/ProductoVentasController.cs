using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourCurlyCareApi.Data;
using YourCurlyCareApi.Models;

namespace YourCurlyCareApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductoVentasController : ControllerBase
{
    private readonly YourCurlyCareContext _context;

    public ProductoVentasController(YourCurlyCareContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductoVenta>>> GetProductoVentas()
    {
        return await _context.ProductoVentas.ToListAsync();
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<ProductoVenta>> GetProductoVenta(int id)
    {
        var productoVenta = await _context.ProductoVentas.FindAsync(id);

        if (productoVenta == null) return NotFound();

        return productoVenta;
    }
   
}

