using Ticket.Worker.Models;

namespace Ticket.Worker.Services;

public interface ITicketRepository : IAsyncDisposable
{
    Task SaveAsync(Models.Ticket ticket);
    Task EnsureDatabaseAsync();
}
