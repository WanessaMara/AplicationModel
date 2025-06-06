using Aplicacao;
using CadastroPessoas.Infraestrutura;
using Dominio.Interface;
using Infraestrutura.Configuracao;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AplicacaoDbContext>(options =>
options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

builder.Services.AddScoped<UsuarioServico>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();