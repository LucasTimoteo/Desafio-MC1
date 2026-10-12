using System.Text.Json.Serialization;
using Pedidos.Api.Infrastructure;
using Pedidos.Api.Services;

var builder = WebApplication.CreateBuilder(args);

const string corsPolicy = "FrontendCors";

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var origensPermitidas = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? ["http://localhost:4200"];

builder.Services.AddCors(options =>
    options.AddPolicy(corsPolicy, policy =>
        policy.WithOrigins(origensPermitidas)
            .AllowAnyHeader()
            .AllowAnyMethod()));

// Repositórios em memória: singleton para compartilhar o estado por toda a aplicação.
builder.Services.AddSingleton<IProdutoRepository, InMemoryProdutoRepository>();
builder.Services.AddSingleton<IPedidoRepository, InMemoryPedidoRepository>();

builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();

builder.Services.AddSingleton<DataSeeder>();

var app = builder.Build();

// Carga inicial: catálogo de produtos + 10.000 pedidos.
app.Services.GetRequiredService<DataSeeder>().Popular();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsPolicy);
app.MapControllers();

app.Run();
