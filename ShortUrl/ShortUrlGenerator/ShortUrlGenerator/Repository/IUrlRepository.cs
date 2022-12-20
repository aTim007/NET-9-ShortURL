
namespace ShortUrlGenerator
{
    public interface IUrlRepository :BaseRepository<URL>
    {
    }

    public interface BaseRepository<T> 
    {
        Task<T?> GetUrl(string key);
        Task Add(T item);
    } 
}
