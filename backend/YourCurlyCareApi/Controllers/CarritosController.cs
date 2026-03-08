using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourCurlyCareApi.Data;
using YourCurlyCareApi.Models;

namespace YourCurlyCareApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CarritosController : ControllerBase
{
    private readonly YourCurlyCareContext _context;

    public CarritosController(YourCurlyCareContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Carrito>>> GetCarritos()
    {
        return await _context.Carritos.ToListAsync();
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Carrito>> GetCarrito(int id)
    {
        var carrito = await _context.Carritos.FindAsync(id);

        if (carrito == null) return NotFound();

        return carrito;
    }
}

