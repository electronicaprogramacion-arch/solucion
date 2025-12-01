using System.Collections.Generic;
using System.Threading.Tasks;

namespace Meziantou.AspNetCore.Components.InfiniteScrolling;

public delegate Task<IEnumerable<T>> InfiniteScrollingItemsProviderRequestDelegate<T>(InfiniteScrollingItemsProviderRequest context);
