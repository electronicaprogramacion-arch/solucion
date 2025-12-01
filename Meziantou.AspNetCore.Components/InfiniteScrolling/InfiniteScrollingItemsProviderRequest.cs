using System.Threading;

namespace Meziantou.AspNetCore.Components.InfiniteScrolling;

public sealed class InfiniteScrollingItemsProviderRequest
{
    public InfiniteScrollingItemsProviderRequest(int startIndex, CancellationToken cancellationToken)
    {
        StartIndex = startIndex;
        CancellationToken = cancellationToken;
    }

    public int StartIndex { get; }

    public CancellationToken CancellationToken { get; }
}
