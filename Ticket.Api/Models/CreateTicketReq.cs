namespace Ticket.Api.Models
{
    public class CreateTicketReq
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Severiry { get; set; }

        public string Author { get; set; }
    }
}
