namespace Ticket.Worker.Models;

public class Ticket
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Severity { get; set; }
    public string Author { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
