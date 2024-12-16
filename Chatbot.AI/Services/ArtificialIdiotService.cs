using System.Runtime.CompilerServices;

namespace Chatbot.AI.Api.Services;

internal sealed class ArtificialIdiotService : IArtificialIntelligenceService
{
    public async IAsyncEnumerable<string> PromptAsync(string prompt, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var sentencesToGenerate = GetResponse(prompt).Split(".").ToArray();

        foreach (var t in sentencesToGenerate)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }
            
            await Task.Delay(100, cancellationToken);
            yield return t + ".";
        }
    }

    private static string GetResponse(string prompt) {
        if(prompt.Contains('1')) return ShortResponse;
        if(prompt.Contains('2')) return MediumResponse;
        
        return LongResponse;
    }

    private const string ShortResponse = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas convallis, ipsum quis aliquam facilisis, sem metus hendrerit justo, quis pellentesque erat arcu eget tellus.";
    private const string MediumResponse = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas convallis, ipsum quis aliquam facilisis, sem metus hendrerit justo, quis pellentesque erat arcu eget tellus. Vivamus vitae commodo dolor. Nulla in ultricies orci. Nulla scelerisque quis sem in porta. Donec sollicitudin lacus a enim interdum dignissim. Aliquam ut nibh faucibus neque pretium posuere. Nullam in tempus leo, non pharetra felis. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Suspendisse in massa elit. Cras efficitur, risus in maximus bibendum, dolor nisi fermentum ante, in ornare risus quam sed lectus. Aliquam a pretium felis, in ullamcorper justo. Vivamus in leo vel dui aliquam feugiat et eu lacus.";
    private const string LongResponse = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas convallis, ipsum quis aliquam facilisis, sem metus hendrerit justo, quis pellentesque erat arcu eget tellus. Vivamus vitae commodo dolor. Nulla in ultricies orci. Nulla scelerisque quis sem in porta. Donec sollicitudin lacus a enim interdum dignissim. Aliquam ut nibh faucibus neque pretium posuere. Nullam in tempus leo, non pharetra felis. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Suspendisse in massa elit. Cras efficitur, risus in maximus bibendum, dolor nisi fermentum ante, in ornare risus quam sed lectus. Aliquam a pretium felis, in ullamcorper justo. Vivamus in leo vel dui aliquam feugiat et eu lacus. Duis ligula orci, molestie et magna et, posuere consequat ligula. Donec vel enim ut risus sollicitudin blandit eu sit amet neque.

Nunc in massa consectetur, dignissim odio non, aliquam lectus. Interdum et malesuada fames ac ante ipsum primis in faucibus. Maecenas molestie lobortis faucibus. Morbi iaculis et neque nec blandit. Morbi vulputate dolor nec blandit dignissim. Suspendisse convallis, sapien vitae molestie pharetra, metus nisi volutpat dui, ac accumsan enim sapien non lectus. In congue nec lacus eget hendrerit. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum sagittis ligula nec nibh vulputate, vel volutpat dolor interdum. Interdum et malesuada fames ac ante ipsum primis in faucibus. Fusce interdum, arcu vel sagittis gravida, ante augue fermentum mauris, quis vulputate nisl ante ac sapien.

Phasellus tincidunt dolor ante, at suscipit velit tempor vel. Etiam quis urna ultricies, lobortis dolor at, cursus neque. Pellentesque porttitor leo metus, in iaculis ex mollis ac. Mauris tincidunt libero nec sem viverra facilisis. Curabitur in faucibus leo. Donec id nibh dolor. Duis feugiat sit amet mi in vulputate. Vivamus vitae sollicitudin est, at iaculis odio. Suspendisse gravida molestie quam, non tincidunt nisl posuere vel. Donec eu bibendum felis.

Aliquam tempus ex a tortor sollicitudin, molestie semper metus viverra. Nam mollis ipsum in risus rhoncus rhoncus. Aliquam ligula orci, consequat nec elit sed, laoreet placerat lacus. Interdum et malesuada fames ac ante ipsum primis in faucibus. Morbi mollis leo nec purus dapibus, ut auctor nunc lacinia. Integer aliquam enim quis vehicula ullamcorper. Nulla ornare nibh vel turpis auctor iaculis. Morbi tempus pretium porttitor.

Donec euismod felis sem, eget laoreet dolor ultricies at. Praesent accumsan placerat sapien, sit amet congue massa ultricies at. Suspendisse potenti. Proin augue tortor, pulvinar nec odio in, feugiat malesuada lectus. Proin sed quam massa. Phasellus orci felis, aliquam sit amet luctus sit amet, faucibus et nunc. Duis gravida ut lorem eu porta. Mauris gravida pharetra suscipit. Curabitur condimentum massa sit amet posuere consectetur. Praesent felis est, fermentum sed finibus vel, eleifend cursus justo. Nulla sagittis vitae turpis ut semper.";
}