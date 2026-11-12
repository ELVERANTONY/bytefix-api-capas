using AutoMapper;
using ByteFix.Datos.Data;
using ByteFix.Datos.Repositories;
using ByteFix.Negocio.Mapping;
using ByteFix.Negocio.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
    options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0;
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Info.Title = "Taller de Soporte Técnico (ByteFix)";
        document.Info.Description =
            "API REST en capas para la gestión de técnicos y reparaciones de equipos.";
        return Task.CompletedTask;
    });
});

// La cadena de conexión NO se versiona. Se configura en
// appsettings.Development.json (local, ignorado por git) o en la variable
// de entorno ConnectionStrings__ByteFixDB.
var cadenaConexion = builder.Configuration.GetConnectionString("ByteFixDB");
if (string.IsNullOrWhiteSpace(cadenaConexion))
    throw new InvalidOperationException(
        "Falta la cadena de conexión 'ByteFixDB'. Configúrala en appsettings.Development.json " +
        "o en la variable de entorno ConnectionStrings__ByteFixDB.");

builder.Services.AddDbContext<ByteFixContext>(options => options.UseSqlServer(cadenaConexion));

// Inyección de dependencias: repositorios, servicios y AutoMapper.
builder.Services.AddScoped<ITecnicoRepository, TecnicoRepository>();
builder.Services.AddScoped<IReparacionRepository, ReparacionRepository>();
builder.Services.AddScoped<ITecnicoService, TecnicoService>();
builder.Services.AddScoped<IReparacionService, ReparacionService>();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

var app = builder.Build();

// Swagger / OpenAPI: solo se documentan los DTOs.
app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Taller de Soporte Técnico (ByteFix)");
    options.RoutePrefix = "swagger";
    options.DocumentTitle = "Taller de Soporte Técnico (ByteFix)";
});

app.UseAuthorization();
app.MapControllers();

app.Run();
