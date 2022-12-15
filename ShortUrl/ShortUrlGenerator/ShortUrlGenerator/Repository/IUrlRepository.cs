
namespace ShortUrlGenerator
{
    public interface IUrlRepository<T> where T: URL
    {
        T? GetUrl(string key);
        void Add(T item);
    }
}
