using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Policlínico.API.Profiles;
using Policlínico.Infrastructure.Data;
using Policlínico.Application.Interfaces;
using Policlínico.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Configurar DbContext
builder.Services.AddDbContext<PoliclínicoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 Registrar servicios
builder.Services.AddScoped<IConsultaService, ConsultaService>();

// 🔹 Controladores y JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// 🔹 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔹 AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// 🔹 Swagger en entorno dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
