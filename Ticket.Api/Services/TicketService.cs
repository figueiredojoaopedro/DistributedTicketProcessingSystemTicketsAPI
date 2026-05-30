using Ticket.Api.Messaging;
using Ticket.Api.Models;
namespace Ticket.Api.Services
{
    public class TicketService : ITicketService
    {
        private readonly IRabbitMQPublisher _publisher;

        public TicketService(IRabbitMQPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task CreateTicketAsync(CreateTicketReq ticket) {
            await _publisher.PublishAsync(ticket, "amq.direct", "tickets");
        }
    }
}
