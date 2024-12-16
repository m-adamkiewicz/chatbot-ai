using Chatbot.AI.Api.DTOs;

namespace Chatbot.AI.Api.Services;

public interface IChatHub
{
    Task SendReceivedMessage(ReceivedMessage @event);
    Task SendAppendTextMessage(AppendTextMessage @event);
    Task SendCompletedMessage(CompletedMessage @event);
}