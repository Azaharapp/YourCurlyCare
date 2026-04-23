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

            var resultadoDB = new Resultado
            {
                IdCuestionario = lote.CuestionarioId,
                ResultadoFinal = resultadoFinal,
                FechaRealizacion = DateTime.Now
            };
            _context.Resultados.Add(resultadoDB);

            //var cuestionario = await _context.Cuestionarios.FindAsync(lote.CuestionarioId);
            //if (cuestionario != null) cuestionario.Estado = EstadoCuestionario.Entregado;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(new { mensaje = "Test procesado", resultado = resultadoFinal });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return BadRequest("Error al procesar el test: " + ex.Message);
        }
    }


    private string CalcularTipoRizo(List<DetalleRespuesta> respuestas)
    {
        int a = respuestas.Count(r => r.Opcion == "a");
        int b = respuestas.Count(r => r.Opcion == "b");
        int c = respuestas.Count(r => r.Opcion == "c");

        //empate de opciones o diferencia de 1 punto
        if (Math.Abs(a - b) <= 1 && a > c && b > c) return "Mixto 2C-3A";
        if (Math.Abs(b - c) <= 1 && b > a && c > a) return "Mixto 3C-4A";

        //si la opcion es la más elegida a diferencia de más de 1 punto
        if (a >= b && a >= c) return "Tipo 2";
        if (b >= a && b >= c) return "Tipo 3";
        if (c >= a && c >= b) return "Tipo 4";


        return "";
    }


    [HttpGet("{id}/preguntas")]
    public async Task<ActionResult<IEnumerable<Pregunta>>> GetPreguntas(int id)
    {
        var listaPreguntas = await _context.Preguntas
            .Where(p => p.IdCuestionario == id)
            .ToListAsync();

        if (listaPreguntas == null || listaPreguntas.Count == 0)
        {
            return NotFound();
        }

        return Ok(listaPreguntas);
    }
}


