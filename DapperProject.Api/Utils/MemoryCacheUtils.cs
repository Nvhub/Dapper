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

        public MemoryCacheEntryOptions memoryCacheEntryOptions(int slidingExpiration = 10, int AbsoluteExpiration = 50)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(slidingExpiration))
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(AbsoluteExpiration))
            .SetPriority(CacheItemPriority.Normal)
            .SetSize(1024);

            return cacheEntryOptions;
        }


    }
}
