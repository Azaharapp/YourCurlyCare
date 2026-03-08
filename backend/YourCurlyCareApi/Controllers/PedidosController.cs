using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourCurlyCareApi.Data;
using YourCurlyCareApi.Models;

namespace YourCurlyCareApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PedidosController : ControllerBase
{
    private readonly YourCurlyCareContext _context;

    public PedidosController(YourCurlyCareContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
    {
        return await _context.Pedidos.ToListAsync();
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Pedido>> GetPedido(int id)
    {
        var pedido = await _context.Pedidos.FindAsync(id);

        if (pedido == null) return NotFound();

        return pedido;
    }
}

