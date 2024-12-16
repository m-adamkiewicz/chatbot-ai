using Chatbot.AI.Api.DAL;

namespace Chatbot.AI.Api.DTOs;

public class MessageDto
{
    public Guid Id { get; set; }
    public Author Author { get; set; }
    public DateTime CreatedAt { get; set; }
    public MessageContentDto? Content { get; set; }
}

public class MessageContentDto
{
    public string Text { get; set; } = string.Empty;
    public Reaction? Reaction { get; set; }
}