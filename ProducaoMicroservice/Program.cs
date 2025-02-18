using Amazon.SQS;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using ProducaoMicroservice.Adapters.Database.Extensions;
using ProducaoMicroservice.Adapters.Database.PostgreSQL;
using ProducaoMicroservice.Adapters.Messaging.Sqs;
using ProducaoMicroservice.Adapters.Persistence;
using ProducaoMicroservice.Core.Ports;
using ProducaoMicroservice.Core.UseCases;

var builder = WebApplication.CreateBuilder(args);

// Configuração do PostgreSQL
builder.Services.AddDbContext<ProducaoContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

// Repositórios
builder.Services.AddScoped<IProducaoRepository, ProducaoRepository>();

// Casos de uso 
builder.Services.AddScoped<ProducaoUseCase>();

// Configuração do SQS
builder.Services.AddSingleton<IAmazonSQS>(sp =>
{
    return new AmazonSQSClient(
        "test", "test", // Credenciais para o LocalStack
        new AmazonSQSConfig { ServiceURL = "http://host.docker.internal:4566" }
    );
});

builder.Services.AddScoped<ISqsService, SqsService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do FastEndpoints
builder.Services.AddFastEndpoints();

var app = builder.Build();

// Aguarde LocalStack antes de iniciar a escuta da fila
var cancellationTokenSource = new CancellationTokenSource();
app.Lifetime.ApplicationStarted.Register(async () =>
{
    using (var scope = app.Services.CreateScope())
    {
        var sqsService = scope.ServiceProvider.GetRequiredService<ISqsService>();
        await sqsService.EsperarLocalStackAsync(); // Aguarda o LocalStack ficar pronto

        var listenerService = scope.ServiceProvider.GetRequiredService<ProducaoUseCase>();
        await listenerService.StartListeningAsync(cancellationTokenSource.Token);
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

// Configuração de endpoints (Roteamento)
app.UseFastEndpoints();

app.Run();
