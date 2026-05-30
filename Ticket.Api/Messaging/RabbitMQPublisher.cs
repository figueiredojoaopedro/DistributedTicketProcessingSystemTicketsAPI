using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Ticket.Api.Messaging
{
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        public async Task PublishAsync<T>(T message, string exchangeName, string routingKey)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using var connection = await factory.CreateConnectionAsync();

            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "tickets", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var jsonMessage = JsonSerializer.Serialize(message);

            var body = Encoding.UTF8.GetBytes(jsonMessage);

            await channel.BasicPublishAsync(exchange: exchangeName, routingKey: routingKey, body: body);

            Console.WriteLine($"Message sent succesfully {jsonMessage}");
        }
    }
}
