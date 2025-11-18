using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using TodoListApi.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TodoListApi.Database.Interface;
using TodoListApi.Database.Repositories;
using TodoListApi.Services.Interfaces;
using TodoListApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Registra DatabaseContext en el contenedor de Inyección de Dependencia (DI) como un servicio 'Scoped'.
// Esto permite que .NET Core inyecte automáticamente una instancia (instancie) de DatabaseContext
// en cualquier clase que lo solicite a través de su constructor.
var server = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(server)); 

//registra el servicio de autenticacion en el contenedor de inyeccion de dependencias.
builder.Services.AddAuthentication(options => {
    //se definen los esquemas(mecanismos) por defectoque el middleware de autenticacion debe usar
    //para gestionar la identidad del usuario
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //para identificar al usuario
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; //para emitir una respuesta de desafio si no usa token
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;//algo general

//configura los parametros para leer y validar el token JWT
}).AddJwtBearer(options => { 
    options.RequireHttpsMetadata = false;//indica que acepta http tambien
    options.SaveToken = true; //que almacene el token en las propiedadades de autenticacion http
    options.TokenValidationParameters = new TokenValidationParameters //requisitos de seguridad q el token debe cumplir
    {
        ValidIssuer = builder.Configuration["JwtConfig:Issuer"], //quien puede emitir el token
        ValidAudience = builder.Configuration["JwtConfig:Audience"], //para quien esta destinado el token
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]!)), //clave secreta
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITodoItemRepository, TodoItemRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHashService, HashService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
