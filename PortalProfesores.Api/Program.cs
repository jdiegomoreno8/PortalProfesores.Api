using Microsoft.EntityFrameworkCore;
using PortalProfesores.Infrastructure.Data;
using PortalProfesores.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Registrar DbContext
builder.Services.AddDbContext<PortalProfesoresDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("PortalProfesoresConnection")));

// Registrar dependencias de la aplicación
builder.Services.AddInfrastructure();

// Servicios de ASP.NET Core
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();