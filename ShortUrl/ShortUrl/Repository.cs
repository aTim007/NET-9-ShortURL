using ShortUrl.Models;

namespace ShortUrl
{
    interface IUrlRepository<T> where T : URL
    {
        T? GetUrl(string key);
        void Add(T item);
    }

    public class UrlRepository : IUrlRepository<URL>
    {
        private readonly UrlContext _db;

        public UrlRepository(UrlContext context)
        {
            _db = context;
        }
        public URL? GetUrl(string key)
        {
            return _db.Urls.FirstOrDefault(b => b.ShortUrl == key);
        }

        public void Add(URL item)
        {
            _db.Urls.Add(item);
            _db.SaveChanges();
        }
    }
}
