using Chatbot.AI.Api.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chatbot.AI.Api.Handlers;

public record ReactMessage(Guid Id, Reaction? Reaction) : IRequest
{
}

internal sealed class ReactMessageHandler(ChatbotDbContext context) : IRequestHandler<ReactMessage>
{
    public async Task Handle(ReactMessage request, CancellationToken cancellationToken)
    {
        var message = await context.Messages.FirstOrDefaultAsync(x => x.Id == request.Id && x.Author == Author.AI, cancellationToken: cancellationToken);
        if (message is null) return;
        
        message.Reaction = request.Reaction;
        await context.SaveChangesAsync(cancellationToken);
    }
}