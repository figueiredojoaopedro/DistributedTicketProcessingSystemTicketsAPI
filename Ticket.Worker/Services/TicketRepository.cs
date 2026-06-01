using Ticket.Worker.Data;

namespace Ticket.Worker.Services;

public class TicketRepository : ITicketRepository
{
    private readonly TicketDbContext _db;

    public TicketRepository(TicketDbContext db)
    {
        _db = db;
    }

    public async Task SaveAsync(Models.Ticket ticket)
    {
        _db.Tickets.Add(ticket);
        await _db.SaveChangesAsync();
    }

    public async Task EnsureDatabaseAsync()
    {
        await _db.Database.EnsureCreatedAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _db.DisposeAsync();
    }
}
