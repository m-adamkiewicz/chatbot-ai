using System.Collections.Concurrent;

namespace Chatbot.AI.Api.Services;

internal sealed class CancellationService : ICancellationService
{
    private readonly ConcurrentDictionary<Guid, CancellationTokenSource> _cancellationTokens = new();
    
    public CancellationToken Get(Guid id) => _cancellationTokens.TryGetValue(id, out var source) ? source.Token : CancellationToken.None;

    public void Add(Guid id) => _cancellationTokens.TryAdd(id, new CancellationTokenSource());

    public void Remove(Guid id) => _cancellationTokens.TryRemove(id, out _);

    public void Cancel(Guid id)
    {
        if (_cancellationTokens.TryGetValue(id, out var source))
        {
            source.Cancel();    
        }
    }

}