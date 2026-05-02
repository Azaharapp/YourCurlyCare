using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourCurlyCareApi.Data;
using YourCurlyCareApi.Models;

namespace YourCurlyCareApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CuestionariosController : ControllerBase
{
    private readonly YourCurlyCareContext _context;

    public CuestionariosController(YourCurlyCareContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cuestionario>>> GetCuestionarios()
    {
        return await _context.Cuestionarios.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cuestionario>> GetCuestionario(int id)
    {
        var cuestionario = await _context.Cuestionarios.FindAsync(id);

        if (cuestionario == null) return NotFound("El cuestionario no existe");

        return cuestionario;
    }

    [HttpPost("guardar-lote")]                                                              
    public async Task<IActionResult> GuardarLote([FromBody] RespuestaLote lote)
    {
        //using hace que cuando termine la funcion, destruye este objeto de transaccion y libera los recursos
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            foreach (var r in lote.Respuestas)
            {
                var nuevaRespuesta = new Respuesta
                {
                    IdUsuario = lote.UsuarioId,
                    IdCuestionario = lote.CuestionarioId,
                    IdPregunta = r.PreguntaId,
                    Opcion = r.Opcion,
                    FechaRealizacion = DateTime.Now
                };
                _context.Respuestas.Add(nuevaRespuesta);
            }

            string resultadoFinal = CalcularTipoRizo(lote.Respuestas);

            var resultadoBD = new Resultado
            {
                IdCuestionario = lote.CuestionarioId,
                ResultadoFinal = resultadoFinal,
                FechaRealizacion = DateTime.Now
            };
            _context.Resultados.Add(resultadoBD);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(new { resultadoFinal });
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            return BadRequest("Error al procesar el test: " + e.Message);
        }
    }

    private string CalcularTipoRizo(List<DetalleRespuesta> respuestas)
    {
        //se almacena cuantas vesces se ha marcada cada opcion de respuesta
        int a = respuestas.Count(r => r.Opcion == "a");
        int b = respuestas.Count(r => r.Opcion == "b");
        int c = respuestas.Count(r => r.Opcion == "c");

        //si el valor absoluto de dos opciones es menor o igual a 1 y son mayores que la tercera opcion 
        if (Math.Abs(a - b) <= 1 && a > c && b > c) return "Mixto 2C-3A";
        if (Math.Abs(b - c) <= 1 && b > a && c > a) return "Mixto 3C-4A";

        //si la opcion es la más elegida
        if (a >= b && a >= c) return "Tipo 2";
        if (b >= a && b >= c) return "Tipo 3";
        if (c >= a && c >= b) return "Tipo 4";

        return "";                                             
    }

    [HttpGet("{id}/preguntas")] 
    public async Task<ActionResult<IEnumerable<Pregunta>>> GetPreguntas(int id)
    {
        var listaPreguntas = await _context.Preguntas.Where(p => p.IdCuestionario == id).ToListAsync();

        if (listaPreguntas == null || listaPreguntas.Count == 0) return NotFound();
    
        return Ok(listaPreguntas);
    }
}


