using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
//using PortalProfesores.Api.Utilities;
//using PortalProfesores.Domain.Interfaces.Services;
using PortalProfesores.Infrastructure.Configuration;
using PortalProfesores.Infrastructure.Data;
using PortalProfesores.Infrastructure.DependencyInjection;
using Microsoft.OpenApi.Models;
using PortalProfesores.Domain.Interfaces.Services;
using PortalProfesores.Infrastructure.Data.Seeds;


var builder = WebApplication.CreateBuilder(args);

// Registrar DbContext
builder.Services.AddDbContext<PortalProfesoresDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("PortalProfesoresConnection")));

// Registrar dependencias de la aplicación
builder.Services.AddInfrastructure();

// Servicios de ASP.NET Core
builder.Services.AddControllers();
// Configuración de CORS para permitir
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
        Description = "Ingrese únicamente el token JWT."
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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
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

app.UseCors("Angular");

app.UseMiddleware<PortalProfesores.Api.Middleware.ExceptionMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

