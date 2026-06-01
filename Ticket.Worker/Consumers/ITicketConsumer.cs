using System;
using System.Collections.Generic;
using System.Text;

namespace Ticket.Worker.Consumers
{
    public interface ITicketConsumer
    {
        Task StartAsync(string exchangeName, string routingKey);

        //Task ProcessAsync();
    }
}
