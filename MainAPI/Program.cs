using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using MainAPI.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var secretKey = builder.Configuration.GetSection("JwtSettings:SecretKey").Value;
var keyBytes = Encoding.UTF8.GetBytes(secretKey!);

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Configuración de Swashbuckle (Swagger)
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingresa el token JWT en este formato: Bearer {tu_token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// Configuración de la Base de Datos (Inyección de Dependencias)
builder.Services.AddDbContext<MainDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<MainAPI.Services.IDocenteService, MainAPI.Services.DocenteService>();
builder.Services.AddScoped<MainAPI.Services.IPortalEstudianteService, MainAPI.Services.PortalEstudianteService>();
builder.Services.AddScoped<MainAPI.Services.IAsignacionesService, MainAPI.Services.AsignacionesService>();
builder.Services.AddScoped<MainAPI.Services.IPerfilesService, MainAPI.Services.PerfilesService>();
builder.Services.AddScoped<MainAPI.Services.IPortalDocenteService, MainAPI.Services.PortalDocenteService>();
builder.Services.AddScoped<MainAPI.Services.ICarrerasService, MainAPI.Services.CarrerasService>();
builder.Services.AddScoped<MainAPI.Services.IOperativoService, MainAPI.Services.OperativoService>();
builder.Services.AddScoped<MainAPI.Services.IPersonasService, MainAPI.Services.PersonasService>();
builder.Services.AddScoped<MainAPI.Services.ICatalogosService, MainAPI.Services.CatalogosService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

var app = builder.Build();

// Interfaz gráfica de Swagger (Swashbuckle)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();