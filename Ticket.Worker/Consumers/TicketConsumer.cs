using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Ticket.Worker.Models;
using Ticket.Worker.Services;

namespace Ticket.Worker.Consumers
{
    public class TicketConsumer : ITicketConsumer, IAsyncDisposable
    {
        private const string queueName = "tickets";
        private IConnection _connection;
        private IChannel _channel;
        private readonly ITicketRepository _ticketRepo;

        public TicketConsumer(ITicketRepository ticketRepo)
        {
            _ticketRepo = ticketRepo;
        }

        public async Task StartAsync(string exchangeName, string routingKey)
        {
            await _ticketRepo.EnsureDatabaseAsync();

            var factory = new ConnectionFactory() { HostName = "localhost" };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

            await _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            await _channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: routingKey, arguments: null);

            await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (sender, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();

                    var message = Encoding.UTF8.GetString(body);

                    var json = JsonSerializer.Deserialize<TicketCreatedEventReq>(message);

                    Console.WriteLine($"Message has been received successfully {message}");

                    await _ticketRepo.SaveAsync(new Models.Ticket
                    {
                        Title = json.Title,
                        Description = json.Description,
                        Severity = json.Severity,
                        Author = json.Author,
                        CreatedAt = DateTime.UtcNow
                    });

                    Console.WriteLine($"Ticket '{json.Title}' saved to SQLite.");

                    await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);

                    await Task.CompletedTask;
                } catch (JsonException JeX) {
                    Console.WriteLine($"[Consumer] Error on deserialize the request: {JeX}");
                    if (_channel.IsOpen){
                        await _channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);
                    }
                    throw;
                } catch (Exception ex)
                {
                    Console.WriteLine($"[Consumer] Error on processing the message: {ex}");

                    if (_channel.IsOpen) { 
                        await _channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                    }
                    throw;
                }
            };


            await _channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);

            Console.WriteLine("Consumer initiated. Waiting for new messages... ");
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel is not null)
            {
                await _channel.CloseAsync();
                _channel.Dispose();
            }
            if (_connection is not null)
            {
                await _connection.CloseAsync();
                _connection.Dispose();
            }
            await _ticketRepo.DisposeAsync();
        }
    }
}
