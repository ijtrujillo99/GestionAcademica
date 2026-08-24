using GestionAcademica.Api.Comun.Errores;
using GestionAcademica.Api.Infraestructura;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = RespuestasProblema.Validacion;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Gestión Académica API",
        Version = "v1",
        Description = "Monolito modular: catálogos, académico, cursos, asistencia y evaluación."
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("PwaLocal", policy =>
        policy.WithOrigins("http://localhost:5173", "https://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddGestionAcademica(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ManejadorExcepcionesMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("PwaLocal");
app.UseAuthorization();
app.MapControllers();

app.Run();
