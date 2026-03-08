using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourCurlyCareApi.Data;
using YourCurlyCareApi.Models;

namespace YourCurlyCareApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductoVPedidosController : ControllerBase
{
    private readonly YourCurlyCareContext _context;

    public ProductoVPedidosController(YourCurlyCareContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductoVPedido>>> GetProductoVPedidos()
    {
        return await _context.ProductoVPedidos.ToListAsync();
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<ProductoVPedido>> GetProductoVPedido(int id)
    {
        var productoVPedido = await _context.ProductoVPedidos.FindAsync(id);

        if (productoVPedido == null) return NotFound();

        return productoVPedido;
    }
}

