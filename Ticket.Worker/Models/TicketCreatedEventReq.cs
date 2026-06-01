using System;
using System.Collections.Generic;
using System.Text;

namespace Ticket.Worker.Models
{
    public class TicketCreatedEventReq
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Severity { get; set; }

        public string Author { get; set; }

    }
}
