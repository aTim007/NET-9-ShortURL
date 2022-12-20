using Crc32 = System.IO.Hashing.Crc32;
using System.Text;
using Base62;

namespace ShortUrlGenerator
{
    public class UrlManager
    {
        private readonly IUrlRepository _repository;

        public UrlManager(IUrlRepository repository)
        {
            _repository = repository;
        }
        public async Task<string> GenerateShortUrl(string fullUrl)
        {
            var hash = GetHash(fullUrl);
            while (true)
            {
                var shortUrl = ConvertToBase62(hash);
                var query = await _repository.GetUrl(shortUrl);
                if (query is null)
                {
                    await _repository.Add(new URL()
                    {
                        FullUrl = fullUrl,
                        ShortUrl = shortUrl
                    });
                    return shortUrl;
                }

                if (query.FullUrl == fullUrl)
                {
                    return shortUrl;
                }
                ChangeHash(ref hash);
            }
        }
        public async Task<string?> GetFullUrl(string shortUrl)
        {
            var query = await _repository.GetUrl(shortUrl);
            if (query is not null)
            {
                var i = query.FullUrl.IndexOf("://");
                if (i > -1)
                {
                    return query.FullUrl[(i + 3)..];
                }
            }
            return query?.FullUrl;
        }
        private byte[] GetHash(string url)
        {
            var bytes = Crc32.Hash(Encoding.UTF8.GetBytes(url));
            byte[] hash = bytes;
            Array.Resize(ref hash, 10);
            for (int i = bytes.Length, j = 0; i < maxLenght; i++, j++)
            {
                if (j >= bytes.Length)
                {
                    j = 0;
                }
                hash[i] = (byte)alphabet[bytes[j] % alphabetLenght];
            }
            return hash;
        }
        //переделать на декримент
        private static void ChangeHash(ref byte[] bytes)
        {
            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                if (bytes[i] < byte.MaxValue)
                {
                    bytes[i]++;
                    return;
                }
                bytes[i] = 0;
            }
            //Array.Resize(ref bytes, bytes.Length+1);
            //bytes[0] = 1;
        }

        private const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private const int maxLenght = 10;
        private readonly int alphabetLenght = alphabet.Length;
        private string ConvertToBase62(byte[] bytes)
        {
            var strBuilder = new StringBuilder();
            for (int i = 0; i < maxLenght; i++)
            {
                strBuilder.Append(i < bytes.Length ? alphabet[bytes[i] % alphabetLenght] : alphabet[0]);
            }
            return strBuilder.ToString();
        }

        private readonly Random _random = new();
        public string GetRandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, _random.Next(50, 100))
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
