using Crc32 = System.IO.Hashing.Crc32;
using System.Text;
using Base62;

namespace ShortUrlGenerator
{
    public class UrlManager
    {
        private readonly IUrlRepository<URL> _repository;

        public UrlManager(IUrlRepository<URL> repository)
        {
            _repository = repository;
        }        
        public string GenerateShortUrl(string fullUrl)
        {
            var hash = Crc32.Hash(Encoding.UTF8.GetBytes(fullUrl));
            while (true)
            {
                var shortUrl = ConvertToBase62(hash);
                var query = _repository.GetUrl(shortUrl);
                if (query is null)
                {
                    _repository.Add(new URL()
                    {
                        FullUrl = fullUrl,
                        ShortUrl= shortUrl
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
        
        //переделать на д?кримент
        private void ChangeHash(ref byte[] bytes)
        {
           for (int i = bytes.Length-1; i >=0; i-- ) //???
            {
                if (bytes[i] < byte.MaxValue)
                {
                    bytes[i]++;
                    return;
                }
                bytes[i] = 0;
            }

           Array.Resize(ref bytes, bytes.Length+1);
            bytes[0] = 1;
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
    }
}
