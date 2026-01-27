using LoanSystem.Application.Interface;

namespace LoanSystem.Application.Services
{
    public class CacheService : ICacheService
    {
        private readonly Dictionary<string, CacheItem> _cache = new();

        private class CacheItem
        {
            public object Value { get; set; } = null!;
            public DateTime ExpiresAt { get; set; }
        }

        public T? Get<T>(string key)
        {
            if (_cache.TryGetValue(key, out var item))
            {
                if (DateTime.UtcNow < item.ExpiresAt)
                {
                    return (T)item.Value;
                }
                _cache.Remove(key);
            }
            return default;
        }

        public void Set<T>(string key, T value, int durationSeconds = 30)
        {
            if (value == null) return;
            
            _cache[key] = new CacheItem
            {
                Value = value,
                ExpiresAt = DateTime.UtcNow.AddSeconds(durationSeconds)
            };
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public bool Exists(string key)
        {
            if (_cache.TryGetValue(key, out var item))
            {
                if (DateTime.UtcNow < item.ExpiresAt)
                {
                    return true;
                }
                _cache.Remove(key);
            }
            return false;
        }
    }
}
