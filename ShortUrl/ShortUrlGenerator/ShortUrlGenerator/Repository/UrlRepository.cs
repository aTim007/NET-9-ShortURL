namespace ShortUrlGenerator
{
    public class UrlRepository : IUrlRepository<URL>
    {
        private readonly UrlContext _db;
        public UrlRepository(UrlContext context)
        {
            _db = context;
        }

        public void Add(URL item)
        {
            _db.Add(item);
            _db.SaveChanges();
        }

        public URL? GetUrl(string key)
        {
            return _db.Urls.FirstOrDefault(b => b.ShortUrl == key);
        }
    }
}
