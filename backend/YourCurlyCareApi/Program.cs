using Microsoft.EntityFrameworkCore;
using YourCurlyCareApi.Data;

var builder = WebApplication.CreateBuilder(args);

//--
// se lee la configuracion de appsetting.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<YourCurlyCareContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
//--

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();              //para que reconozca a los controladores
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//permite conexion entre c# y angular por temma de puertos
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PermitirAngular");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
