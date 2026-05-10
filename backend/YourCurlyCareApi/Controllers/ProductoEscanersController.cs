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

        if (producto == null) return NotFound(null);

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
                EsApto = datos.EsApto,
                IdUsuario = datos.UsuarioId
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

    //Panel de admin para productos --> Mezcla las tablas productoEscaner y registroEscaner de la BD
    [HttpGet("admin")]
    public async Task<ActionResult> GetProductosAdmin()
    {
        var lista = await _context.ProductoEscaners
            .GroupJoin(_context.RegistroEscaners,                       //GroupJoin es como left join en SQL
                  p => p.Id,
                  r => r.IdProductoE,
                  (p, registros) => new { p, registros })
            .SelectMany(
                temp => temp.registros.DefaultIfEmpty(),                //si no hay registros rellena con null
                (temp, r) => new ProductosAdmin
                {
                    Id = temp.p.Id,
                    CodigoBarras = temp.p.CodigoBarras,
                    Nombre = temp.p.Nombre,
                    Marca = temp.p.Marca,
                    Ingredientes = temp.p.Ingredientes,
                    Silicona = temp.p.Silicona,
                    Alcohol = temp.p.Alcohol,
                    Sulfato = temp.p.Sulfato,
                    EsApto = temp.p.EsApto,
                    IdUsuario = temp.p.IdUsuario,
                    FechaRegistro = r != null ? r.FechaEscaner : DateTime.MinValue
                }).ToListAsync();

        return Ok(lista);
    }

    [HttpPut("{id}")]                                                       //editar los productos en el panel admin
    public async Task<IActionResult> ActualizarProducto(int id, [FromBody] System.Text.Json.JsonElement datosRecibidos)
    {
        var productoExistente = await _context.ProductoEscaners.FindAsync(id);
        if (productoExistente == null) return NotFound();

        if (datosRecibidos.TryGetProperty("nombre", out var nombre))
            productoExistente.Nombre = nombre.GetString();

        if (datosRecibidos.TryGetProperty("marca", out var marca))
            productoExistente.Marca = marca.GetString();

        if (datosRecibidos.TryGetProperty("ingredientes", out var ingredientes))
            productoExistente.Ingredientes = ingredientes.GetString();

        if (datosRecibidos.TryGetProperty("silicona", out var sil))
            productoExistente.Silicona = sil.GetBoolean();

        if (datosRecibidos.TryGetProperty("alcohol", out var alc))
            productoExistente.Alcohol = alc.GetBoolean();

        if (datosRecibidos.TryGetProperty("sulfato", out var sul))
            productoExistente.Sulfato = sul.GetBoolean();

        await _context.SaveChangesAsync();
        return NoContent();                                 //si todo va bien no devuelve nada
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarProducto(int id)
    {
        var producto = await _context.ProductoEscaners.FindAsync(id);
        if (producto == null) return NotFound();

        _context.ProductoEscaners.Remove(producto);
        await _context.SaveChangesAsync();
        return Ok();
    }
}

