using Microsoft.EntityFrameworkCore;
using Ticket.Worker.Models;

namespace Ticket.Worker.Data;

public class TicketDbContext : DbContext
{
    public DbSet<Models.Ticket> Tickets => Set<Models.Ticket>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=tickets.db");
    }
}
