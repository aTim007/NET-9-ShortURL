using ShortUrl.Models;
using System.Security.Policy;

namespace ShortUrl
{
    public static class URLManager
    {
        public static string GetShortUrl(URL item)
        {
            if (string.IsNullOrEmpty(item.ShortURL))
            {
                item.ShortURL = HashManager.HashURL(item.FullURL);
            }


            var query = AddShortUrlInDB(item.ShortURL);

            if (query is not null)
            {
                if (query.FullURL != item.FullURL)
                {
                    item.ShortURL = HashManager.RepeatHashURL(query);                    
                }
                else
                {
                    return item.ShortURL;
                }
            }
            AddUrlInDb(item);
            return item.ShortURL;
        }

        public static string? GetFullUrl(string ShortUrl)
        {
            var query = AddShortUrlInDB(ShortUrl);

            if (query is not null)
            {
                var i = query.FullURL.IndexOf("://");
                if (i != -1)
                {
                    return query.FullURL[(i + 3)..];
                }
                return query.FullURL;
            }
            return null;
        }
        public static URL? AddShortUrlInDB(string ShortURL)
        {
            using var db = new URLContext();
            var query = db.Urls
                       .FirstOrDefault(b => b.ShortURL == ShortURL);
            return query;
        }

        public static void AddUrlInDb(URL item)
        {
            using var db = new URLContext();
            db.Urls.Add(item);
            db.SaveChanges();
        }
    }
}
