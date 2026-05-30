using Ticket.Api.Models;

namespace Ticket.Api.Services
{
    public interface ITicketService
    {
        Task CreateTicketAsync(CreateTicketReq request);
    }
}
