using System.Text.Json;
using Ticket.Api.Services;
using Ticket.Api.Models;
using Ticket.Api.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IRabbitMQPublisher, RabbitMQPublisher>();

builder.Services.AddScoped<ITicketService, TicketService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost(
    "/tickets",
    async (
        CreateTicketReq request,
        ITicketService service) =>
    {
        await service.CreateTicketAsync(request);

        return Results.Accepted(value: new
        {
            message =
                "Ticket enviado para processamento"
        });
    });



app.Run();