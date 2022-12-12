using ShortUrl.Models;
using System.Security.Policy;

namespace ShortUrl
{
    public class UrlManager
    {
        private readonly IUrlRepository<URL> _repository;

        HashManager _hashManager;
        public UrlManager(IUrlRepository<URL> repository)
        {
            _repository = repository;
            _hashManager = new HashManager(this);
        }

        public void GetShortUrl(ref URL item)
        {
            if (string.IsNullOrEmpty(item.ShortUrl))
            {
                item.ShortUrl = _hashManager.HashURL(item.FullUrl);
            }

            var query = _repository.GetUrl(item.ShortUrl);

            if (query is not null)
            {
                if (query.FullUrl != item.FullUrl)
                {
                    item.ShortUrl = _hashManager.RepeatHashURL(item);
                }
                else
                {
                    return;
                }
            }
            _repository.Add(item);
        }

        public string? GetFullUrl(string ShortUrl)
        {
            var query = _repository.GetUrl(ShortUrl);

            if (query is not null)
            {
                var i = query.FullUrl.IndexOf("://");
                if (i != -1)
                {
                    return query.FullUrl[(i + 3)..];
                }
                return query.FullUrl;
            }
            return null;
        }

        public bool CheckShortUrl(string ShortUrl)
        {
            return _repository.GetUrl(ShortUrl) is not null;
        }
    }
}
