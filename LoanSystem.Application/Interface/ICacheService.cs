namespace LoanSystem.Application.Interface
{
    public interface ICacheService
    {
        T? Get<T>(string key);
        void Set<T>(string key, T value, int durationSeconds = 30);
        void Remove(string key);
        bool Exists(string key);
    }
}
