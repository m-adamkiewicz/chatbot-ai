namespace Chatbot.AI.Api.Services;

public interface IArtificialIntelligenceService
{
    IAsyncEnumerable<string> PromptAsync(string prompt, CancellationToken cancellationToken);
}