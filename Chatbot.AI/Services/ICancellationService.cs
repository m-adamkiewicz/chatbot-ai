namespace Chatbot.AI.Api.Services;

public interface ICancellationService
{
    void Add(Guid id);
    CancellationToken Get(Guid id);
    void Cancel(Guid id);
    void Remove(Guid id);
}