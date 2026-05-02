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
        var producto = await _context.ProductoEscaners.FirstOrDefaultAsync(p => p.CodigoBarras == codigo);

        if (producto == null) return NotFound();

        return Ok(producto);
    }

    [HttpPost("escanear")]                                                      //para la url, no tiene que ser igual al nombre de la funcion
    public async Task<ActionResult<ProductoEscaner>> RegistrarAccion([FromBody] DatosEscaneo datos)
    {
        var producto = await _context.ProductoEscaners.FirstOrDefaultAsync(p => p.CodigoBarras == datos.CodigoBarras);

        if (producto == null)                                                  
        {
            producto = new ProductoEscaner
            {
                CodigoBarras = datos.CodigoBarras,
                Nombre = datos.Nombre,
                Marca = datos.Marca,
                Ingredientes = datos.Ingredientes,
                Alcohol = datos.Alcohol,
                Silicona = datos.Silicona,
                Sulfato = datos.Sulfato,
                EsApto = datos.EsApto
            };

            _context.ProductoEscaners.Add(producto);
            await _context.SaveChangesAsync();
        }

        var nuevoRegistro = new RegistroEscaner
        {
            IdProductoE = producto.Id,
            IdUsuario = datos.UsuarioId,
            FechaEscaner = DateTime.Now,
            Respuesta = producto.EsApto
        };

        _context.RegistroEscaners.Add(nuevoRegistro);
        await _context.SaveChangesAsync();

        return Ok(producto);
    }

    [HttpGet("buscar")]
    public async Task<ActionResult<IEnumerable<ProductoEscaner>>> BuscarProductos([FromQuery] string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return BadRequest("El input búsqueda no puede estar vacío.");

        var resultados = await _context.ProductoEscaners
            .Where(p => p.Nombre.Contains(input) || p.Marca.Contains(input))
            .Take(15).ToListAsync();

        return resultados;
    }
}

