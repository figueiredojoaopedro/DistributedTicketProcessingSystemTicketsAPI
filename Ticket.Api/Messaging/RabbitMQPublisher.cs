using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Ticket.Api.Messaging
{
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        public async Task PublishAsync<T>(T message, string exchangeName, string routingKey)
        {
            const string queueName = "tickets";

            var factory = new ConnectionFactory() { HostName = "localhost" };

            await using var connection = await factory.CreateConnectionAsync();

            await using var channel = await connection.CreateChannelAsync();
            
            await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

            await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            await channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: routingKey, arguments: null);

            var jsonMessage = JsonSerializer.Serialize(message);

            var body = Encoding.UTF8.GetBytes(jsonMessage);

            var properties = new BasicProperties { Persistent = true, ContentType = "application/json", ContentEncoding="utf-8"};

            await channel.BasicPublishAsync(exchange: exchangeName, routingKey: routingKey, mandatory: false, basicProperties: properties,  body: body);

            Console.WriteLine($"Message sent succesfully {jsonMessage}");
        }
    }
}
