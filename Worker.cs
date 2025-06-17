using System.Text;
using System.Text.Json;
using PedidoConsumer.Data;
using PedidoConsumer.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private IModel _channel;

    public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;

        var factory = new ConnectionFactory() { HostName = "localhost" };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        _channel.QueueDeclare(
            queue: "filaPedido",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var pedidoDto = JsonSerializer.Deserialize<PedidoDTO>(json);

            var pedido = new Pedido
            {
            Nome = pedidoDto.NomeCliente,
            Quantidade = pedidoDto.Quantidade
            };


            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            db.Pedidos.Add(pedido);
            await db.SaveChangesAsync();
            
            _logger.LogInformation($"Pedido salvo no banco: {pedido.Nome}");
        };

        _channel.BasicConsume(
            queue: "filaPedido",
            autoAck: true,
            consumer: consumer);

        return Task.CompletedTask;
    }
}
