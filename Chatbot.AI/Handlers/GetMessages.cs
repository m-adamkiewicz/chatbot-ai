using Chatbot.AI.Api.DAL;
using Chatbot.AI.Api.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chatbot.AI.Api.Handlers;

public record GetMessages : IRequest<IReadOnlyCollection<MessageDto>>
{
}

internal sealed class GetChatsHandler(ChatbotDbContext context) : IRequestHandler<GetMessages, IReadOnlyCollection<MessageDto>>
{
    public async Task<IReadOnlyCollection<MessageDto>> Handle(GetMessages request, CancellationToken cancellationToken)
    {
        return await context.Messages
            .Select(x => new MessageDto
            {
                Id = x.Id,
                Author = x.Author,
                CreatedAt = x.CreatedAt,
                Content = new MessageContentDto()
                {
                    Text = x.Text,
                    Reaction = x.Reaction
                }
            })
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}