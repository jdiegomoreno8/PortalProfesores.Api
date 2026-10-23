using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PortalProfesores.Infrastructure.Configuration;
using PortalProfesores.Infrastructure.Data;
using PortalProfesores.Infrastructure.DependencyInjection;
using Microsoft.OpenApi.Models;
using PortalProfesores.Domain.Interfaces.Services;
using PortalProfesores.Infrastructure.Data.Seeds;
using PortalProfesores.Infrastructure.BackgroundServices;
using PortalProfesores.Infrastructure.Services.Pagos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PortalProfesoresDbContext>(options =>
options.UseSqlServer(
builder.Configuration.GetConnectionString("PortalProfesoresConnection"))
#if DEBUG
.EnableDetailedErrors()
.LogTo(Console.WriteLine, LogLevel.Information)
#endif
);
// Registrar dependencias de la aplicaci�n
builder.Services.AddInfrastructure();

// Pasarela de pagos simulada
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<
        IPasarelaPagoService,
        PasarelaPagoSimulada>();
}
builder.Services.AddHostedService<ReservaVencimientoService>();

// Servicios de ASP.NET Core

builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errores = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .SelectMany(x => x.Value!.Errors)
                .Select(x => string.IsNullOrWhiteSpace(x.ErrorMessage)
                    ? "El valor enviado no es válido."
                    : x.ErrorMessage)
                .Distinct()
                .ToList();

            var response = new
            {
                success = false,
                statusCode = StatusCodes.Status400BadRequest,
                message = "La solicitud contiene datos inválidos.",
                errors = errores
            };

            return new BadRequestObjectResult(response);
        };
    });

// Configuraci�n de CORS para permitir
// que el Frontend Angular consuma la API.
builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Portal Profesores API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese unicamente el token JWT."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.Configure<JwtSettings>(
builder.Configuration.GetSection("Jwt"));

var jwtSettings = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtSettings>()!;


builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,

                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Key))
            };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                // Evita que ASP.NET Core escriba
                // su respuesta 401 predeterminada.
                context.HandleResponse();

                if (context.Response.HasStarted)
                    return;

                context.Response.StatusCode =
                    StatusCodes.Status401Unauthorized;

                context.Response.ContentType =
                    "application/json; charset=utf-8";

                var response = new
                {
                    success = false,
                    statusCode = StatusCodes.Status401Unauthorized,
                    message = "Debes iniciar sesión para acceder a este recurso."
                };

                await context.Response.WriteAsJsonAsync(response);
            },

            OnForbidden = async context =>
            {
                if (context.Response.HasStarted)
                    return;

                context.Response.StatusCode =
                    StatusCodes.Status403Forbidden;

                context.Response.ContentType =
                    "application/json; charset=utf-8";

                var response = new
                {
                    success = false,
                    statusCode = StatusCodes.Status403Forbidden,
                    message = "No tienes permisos para realizar esta operación."
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        };
    });


builder.Services.AddHttpContextAccessor();

var app = builder.Build();

//Seed Administrator

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services
        .GetRequiredService<PortalProfesoresDbContext>();

    var passwordService = services
        .GetRequiredService<IPasswordService>();

    await AdministradorSeed.InicializarAsync(
        context,
        passwordService,
        builder.Configuration);
}


// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("Angular");

app.UseMiddleware<PortalProfesores.Api.Middleware.ExceptionMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

