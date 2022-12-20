using Microsoft.EntityFrameworkCore;

namespace ShortUrlGenerator
{
    public class UrlRepository : IUrlRepository
    {
        private readonly UrlContext _db;
        public UrlRepository(UrlContext context)
        {
            _db = context;
        }

        public async Task Add(URL item)
        {
            await _db.AddAsync(item);
            await _db.SaveChangesAsync();
        }

        public async Task<URL?> GetUrl(string key)
        {
            return await _db.Urls.FirstOrDefaultAsync(b => b.ShortUrl == key );
        }
    }
}
