using Microsoft.EntityFrameworkCore;

namespace ShortUrlGenerator
{
    [Index(nameof(ShortUrl), Name = "ShortURL")]
    public class URL
    {
        public string ShortUrl { get; set; } = string.Empty;
        public string FullUrl { get; set; } = string.Empty;
    }
}
