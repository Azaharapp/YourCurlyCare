using Microsoft.EntityFrameworkCore;
using YourCurlyCareApi.Models;

namespace YourCurlyCareApi.Data;

public class YourCurlyCareContext : DbContext               //clase hereda de DbContext
{
    //constructor vacio
    public YourCurlyCareContext(DbContextOptions<YourCurlyCareContext> options)
    : base(options)
    {
    }

    //todas las tablas de BD
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Cuestionario> Cuestionarios { get; set; }
    public DbSet<Respuesta> Respuestas { get; set; }
    public DbSet<Pregunta> Preguntas { get; set; }
    public DbSet<Resultado> Resultados { get; set; }
    public DbSet<ProductoEscaner> ProductoEscaners { get; set; }
    public DbSet<RegistroEscaner> RegistroEscaners { get; set; }
    public DbSet<ModeloIA> ModeloIAs { get; set; }
    public DbSet<ConsultaIA> ConsultaIAs { get; set; }

}