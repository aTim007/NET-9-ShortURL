using Base62;
using System.Text;
using Crc32 = System.IO.Hashing.Crc32;
using ShortUrl.Models;
using System.Security.Policy;
using System.Collections;
using System.Numerics;
using ShortUrl;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ShortUrl
{
    public static class HashManager
    {
        private static readonly Base62Converter base62 = new();

        /// <summary>
        /// Хэш полной ссылки алгоритмом CRC32 и запись получившегося значения в переменную для короткой ссылки
        /// </summary>
        public static string HashURL(string FullURL)
        {
            var hash = Encoding.UTF8.GetString(Crc32.Hash(Encoding.UTF8.GetBytes(FullURL)));
            hash = base62.Encode(hash);

            return CheckLengthShortURL(hash);
        }

        /// <summary>
        /// Возвращает строку, длинна которой соответствует диапазону от 7 до 10 символов
        /// </summary>
        private static string CheckLengthShortURL(string ShortURL)
        {
            do
            {
                switch (ShortURL.Length)
                {
                    case > 10:
                        {
                            ShortURL = ShortURL[..10];
                            break;
                        }
                    case < 7:
                        {
                            ShortURL += base62.Encode(ShortURL.Length.ToString());
                            break;
                        }
                }
            }
            while (!CheckDiapason(ShortURL.Length));
            return ShortURL;
        }

        /// <summary>
        /// Проверка вхождения длинны строки в диапазон от 7 до 10 символов
        /// </summary>
        /// <param name="length">Длинна строки</param>
        /// <returns></returns>
        private static bool CheckDiapason(int length)
        {
            switch (length)
            {
                case > 10:
                    return false;
                case < 7:
                    return false;
            }
            return true;
        }

        /// <summary>
        /// При одинаковом хэше и разных полных ссылках меняем хэш
        /// </summary>
        public static string RepeatHashURL(URL query)
        {
            var a = Crc32.Hash(Encoding.UTF8.GetBytes(query.FullURL));
            var result = query;
            var ShortUrl = result.ShortURL;
            while (result != null)
            {
                var i = a.Length - 1;
                while (a[i] >= 127)
                {
                    a[i] = 0;
                    i--;
                }
                a[i]++;
                ShortUrl = base62.Encode(Encoding.UTF8.GetString(a));
                ShortUrl = CheckLengthShortURL(ShortUrl);
                result = URLManager.AddShortUrlInDB(ShortUrl);
            }
            return ShortUrl;
        }
    }
}
