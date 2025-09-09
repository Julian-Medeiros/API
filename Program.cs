using System.Text.Json.Serialization;
using API.Context;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Adiciona a conexão com o SQL Server utilizando a string em appsettings.Development.json
builder.Services.AddDbContext<TarefaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))
);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer(); // Rastreia os endpoints e adiciona na documentação
builder.Services.AddSwaggerGen(); // Gera o Swagger

// Faz os Enums retornarem os nomes e não os numeros (0 = Pendente, 1 = Finalizado)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Habilita o middleware do Swagger
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
        c.RoutePrefix = string.Empty; // Swagger será acessível na raiz: http://localhost:<Porta>/
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();