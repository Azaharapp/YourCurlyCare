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

    [HttpGet]                                                                   //listado de todos los productos
    public async Task<ActionResult<IEnumerable<ProductoEscaner>>> GetProductoEscaners()
        => await _context.ProductoEscaners.ToListAsync();


    [HttpGet("{id:int}")]                                                       //busquedas por id
    public async Task<ActionResult<ProductoEscaner>> GetProductoEscaner(int id)
    {
        var productoEscaner = await _context.ProductoEscaners.FindAsync(id);

        if (productoEscaner == null) return NotFound();

        return productoEscaner;
    }


    [HttpGet("{codigo}")]                                                       //devuelve el codigo de barras del producto escaneado
    public async Task<ActionResult<ProductoEscaner>> GetProducto(string codigo)
    {
        var producto = await _context.ProductoEscaners
            .FirstOrDefaultAsync(p => p.CodigoBarras == codigo);

        if (producto == null) return NotFound();

        return Ok(producto);
    }




}

