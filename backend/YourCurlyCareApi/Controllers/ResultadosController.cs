using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourCurlyCareApi.Data;
using YourCurlyCareApi.Models;

namespace YourCurlyCareApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ResultadosController : ControllerBase
{
    private readonly YourCurlyCareContext _context;

    public ResultadosController(YourCurlyCareContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Resultado>>> GetResultados() => await _context.Resultados.ToListAsync();


    [HttpGet("{id}")]                                       
    public async Task<ActionResult<Resultado>> GetResultado(int id) 
    {
        var resultado = await _context.Resultados.FindAsync(id);   

        if (resultado == null) return NotFound();                                  
    
        return resultado;                                     
    }
}
