namespace Chatbot.AI.Api.DTOs;

public class SendMessage
{
    public string Text { get; set; } = string.Empty;
}

public class StartMessage
{
    public Guid MessageId { get; set; }
    public Guid ResponseId { get; set; }
}

public class ReceivedMessage
{
    public Guid MessageId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid ResponseId { get; set; }
    public DateTime ResponseCreatedAt { get; set; }
}

public class AppendTextMessage
{
    public int PartId { get; set; } 
    public Guid MessageId { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class CompletedMessage
{
    public Guid MessageId { get; set; }
}

public static class ChatEvents
{
    public const string ReceivedMessage = "received";
    public const string DoneMessage = "done";
    public const string AppendTextMessage = "appendText";
}