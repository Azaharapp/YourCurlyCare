using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourCurlyCareApi.Data;
using YourCurlyCareApi.Models;

namespace YourCurlyCareApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutoVCarritosController : ControllerBase
{

    private readonly YourCurlyCareContext _context;

    public ProdutoVCarritosController(YourCurlyCareContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductoVCarrito>>> GetProductoVCarritos()
    {
        return await _context.ProductoVCarritos.ToListAsync();
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<ProductoVCarrito>> GetProductoVCarrito(int id)
    {
        var productoVCarrito = await _context.ProductoVCarritos.FindAsync(id);

        if (productoVCarrito == null) return NotFound();

        return productoVCarrito;
    }
}
