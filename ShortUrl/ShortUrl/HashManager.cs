using Base62;
using System.Text;
using Crc32 = System.IO.Hashing.Crc32;
using ShortUrl.Models;
namespace ShortUrl
{
    public class HashManager
    {
        private readonly Base62Converter base62 = new();

        private readonly URL url = new();
        public HashManager(ref URL item) { url = item; }

        /// <summary>
        /// Хэш полной ссылки алгоритмом CRC32 и запись получившегося значения в переменную для короткой ссылки
        /// </summary>
        public void HashURL()
        {
            var a = Encoding.UTF8.GetString(Crc32.Hash(Encoding.UTF8.GetBytes(url.FullURL)));
            url.ShortURL = base62.Encode(a);
            CheckLengthShortURL();
        }

        /// <summary>
        /// Проверяет, соответсвует ли ссылка диапазону от 7 до 10 символов
        /// </summary>
        private void CheckLengthShortURL()
        {
            var lengthShortURL = url.ShortURL.Length;
            switch (lengthShortURL)
            {
                case > 10:
                    {
                        url.ShortURL = url.ShortURL[..10];
                        break;
                    }
                case < 7:
                    {
                        var flag = true;
                        while (flag)
                        {
                            url.ShortURL += base62.Encode(lengthShortURL.ToString());
                            lengthShortURL = url.ShortURL.Length;
                            if (lengthShortURL >= 7)
                            {
                                flag = false;
                            }
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// При одинаковом хэше и разных полных ссылках меняем хэш
        /// </summary>
        public void RepeatHashURL()
        {
            url.ShortURL = base62.Encode(url.ShortURL) + url.ShortURL;
            CheckLengthShortURL();
        }
    }
}
