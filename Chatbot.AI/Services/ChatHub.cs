using Chatbot.AI.Api.DTOs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Chatbot.AI.Api.Services;

internal sealed class ChatHub(IMediator mediator, IHubContext<ChatHub> hubContext) : Hub, IChatHub
{
    public async Task SendMessage(SendMessage @event) => await mediator.Send(new Handlers.SendMessage(@event.Text));

    public async Task StartMessage(StartMessage @event) => await mediator.Send(new Handlers.StartMessage(@event.MessageId, @event.ResponseId));

    public async Task SendReceivedMessage(ReceivedMessage @event) => await hubContext.Clients.All.SendAsync(ChatEvents.ReceivedMessage, @event);

    public async Task SendAppendTextMessage(AppendTextMessage @event) => await hubContext.Clients.All.SendAsync(ChatEvents.AppendTextMessage, @event);

    public async Task SendCompletedMessage(CompletedMessage @event) => await hubContext.Clients.All.SendAsync(ChatEvents.DoneMessage, @event);
}
