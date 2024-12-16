using Chatbot.AI.Api.DAL;
using Chatbot.AI.Api.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chatbot.AI.Api.Handlers;

public record CancelMessage(Guid Id) : IRequest
{
}

internal sealed class CancelMessageHandler(ICancellationService cancellationService, ChatbotDbContext context) : IRequestHandler<CancelMessage>
{
    public async Task Handle(CancelMessage request, CancellationToken cancellationToken)
    {
        var message = await context.Messages.FirstOrDefaultAsync(m => m.Id == request.Id && m.Author == Author.AI, cancellationToken: cancellationToken);
        if (message is null) return;
        
        cancellationService.Cancel(request.Id);
    }
}