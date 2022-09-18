using Microsoft.Extensions.Caching.Memory;

namespace DapperProject.Api.Utils
{
    public class MemoryCacheUtils
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheUtils(IMemoryCache cache)
        {
            _cache = cache;
        }

        public MemoryCacheEntryOptions memoryCacheEntryOptions()
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(10))
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(50))
            .SetPriority(CacheItemPriority.Normal)
            .SetSize(1024);

            return cacheEntryOptions;
        }


    }
}
