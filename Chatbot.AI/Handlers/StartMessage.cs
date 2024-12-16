using System.Text;
using Chatbot.AI.Api.DAL;
using Chatbot.AI.Api.DTOs;
using Chatbot.AI.Api.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chatbot.AI.Api.Handlers;

public record StartMessage(Guid MessageId, Guid ResponseId) : IRequest
{
}

internal sealed class StartMessageHandler(IArtificialIntelligenceService aiService, ICancellationService cancellationService, IChatHub chatHub, ChatbotDbContext context) : IRequestHandler<StartMessage>
{
    public async Task Handle(StartMessage request, CancellationToken cancellationToken)
    {
        var message = await context.Messages.FirstOrDefaultAsync(x => x.Id == request.MessageId && x.Author == Author.Human, cancellationToken);
        if (message is null) return;

        var response = await context.Messages.FirstOrDefaultAsync(x => x.Id == request.ResponseId && x.Author == Author.AI, cancellationToken);
        if (response is null) return;

        var messageCancellationToken = cancellationService.Get(response.Id);
        var builder = new StringBuilder();
        var i = 0;

        await foreach (var text in aiService.PromptAsync(message.Text, cancellationToken))
        {
            if (messageCancellationToken.IsCancellationRequested)
            {
                await SaveResponseAsync(response, builder, cancellationToken);
                return;
            };

            builder.Append(text);
            await chatHub.SendAppendTextMessage(new AppendTextMessage() { MessageId = response.Id, Text = text, PartId = i++});
        }

        await SaveResponseAsync(response, builder, cancellationToken);
    }

    private async Task SaveResponseAsync(Message response, StringBuilder builder, CancellationToken cancellationToken)
    {
        response.Text = builder.ToString();
        await context.SaveChangesAsync(cancellationToken);
        await chatHub.SendCompletedMessage(new CompletedMessage() { MessageId = response.Id });

        cancellationService.Remove(response.Id);
    }
}