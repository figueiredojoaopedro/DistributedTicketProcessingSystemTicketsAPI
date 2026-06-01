using Ticket.Worker.Consumers;
using Ticket.Worker.Data;
using Ticket.Worker.Services;

await using var db = new TicketDbContext();
ITicketRepository repo = new TicketRepository(db);
await using var consumer = new TicketConsumer(repo);

await consumer.StartAsync("ticketsExchange.direct", "tickets");

Console.WriteLine("Press any key to exit...");

Console.ReadKey();