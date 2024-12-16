using Chatbot.AI.Api.DAL;
using Chatbot.AI.Api.DTOs;
using Chatbot.AI.Api.Services;
using MediatR;

namespace Chatbot.AI.Api.Handlers;

public record SendMessage(string Text) : IRequest
{
}

internal sealed class SendMessageHandler(IChatHub chatHub, ICancellationService cancellationService, ChatbotDbContext context) : IRequestHandler<SendMessage>
{
    public async Task Handle(SendMessage request, CancellationToken cancellationToken)
    {
        var message = new Message()
        {
            Id = Guid.CreateVersion7(), CreatedAt = DateTime.UtcNow, Author = Author.Human, Text = request.Text
        };

        var response = new Message()
        {
            Id = Guid.CreateVersion7(), CreatedAt = DateTime.UtcNow, Author = Author.AI, Text = string.Empty
        };
        
        context.AddRange(message, response);
        await context.SaveChangesAsync(cancellationToken);
        await chatHub.SendReceivedMessage(new ReceivedMessage { MessageId = message.Id, CreatedAt = message.CreatedAt, ResponseId = response.Id, ResponseCreatedAt = response.CreatedAt });
        
        cancellationService.Add(response.Id);
    }
}