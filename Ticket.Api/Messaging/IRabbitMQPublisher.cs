namespace Ticket.Api.Messaging
{
    public interface IRabbitMQPublisher
    {
        Task PublishAsync<T>(T message, string exchangeName, string routingKey);
    }
}
