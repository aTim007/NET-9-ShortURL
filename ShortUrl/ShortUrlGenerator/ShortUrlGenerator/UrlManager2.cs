//using Crc32 = System.IO.Hashing.Crc32;
//using System.Text;
//using Base62;

//namespace ShortUrlGenerator
//{
//    public class UrlManager2
//    {
//        private readonly Base62Converter base62 = new();
//        UrlManager _urlManager;
//        public UrlManager2(UrlManager context)
//        {
//            _urlManager = context;
//        }

//        /// <summary>
//        /// Хэш полной ссылки алгоритмом CRC32 и запись получившегося значения в переменную для короткой ссылки
//        /// </summary>
//        public string HashURL(string FullUrl)
//        {
//            var hash = Encoding.UTF8.GetString(Crc32.Hash(Encoding.UTF8.GetBytes(FullUrl)));
//            hash = base62.Encode(hash);

//            return CheckLengthShortUrl(hash);
//        }

//        /// <summary>
//        /// Возвращает строку, длинна которой соответствует диапазону от 7 до 10 символов
//        /// </summary>
//        private string CheckLengthShortUrl(string ShortUrl)
//        {
//            do
//            {
//                switch (ShortUrl.Length)
//                {
//                    case > 10:
//                        {
//                            ShortUrl = ShortUrl[(ShortUrl.Length - 10)..];
//                            break;
//                        }
//                    case < 7:
//                        {
//                            ShortUrl += base62.Encode(ShortUrl.Length.ToString());
//                            break;
//                        }
//                }
//            }
//            while (!CheckDiapason(ShortUrl.Length));
//            return ShortUrl;
//        }

//        /// <summary>
//        /// Проверка вхождения длинны строки в диапазон от 7 до 10 символов
//        /// </summary>
//        /// <param name="length">Длинна строки</param>
//        /// <returns></returns>
//        private bool CheckDiapason(int length)
//        {
//            switch (length)
//            {
//                case > 10:
//                    return false;
//                case < 7:
//                    return false;
//            }
//            return true;
//        }

//        /// <summary>
//        /// Значение байта >127 выдает некоректные значения
//        /// </summary>
//        const int magicNumber = 127;
//        /// <summary>
//        /// При одинаковом хэше и разных полных ссылках меняем хэш
//        /// </summary>
//        public string RepeatHashURL(URL query)
//        {
//            var a = Crc32.Hash(Encoding.UTF8.GetBytes(query.FullUrl));
//            var ShortUrl = query.ShortUrl;
//            while (_urlManager.CheckShortUrl(ShortUrl))
//            {
//                var i = a.Length - 1;
//                var flag = true;
//                a[i]++;
//                while (flag && a[i] >= magicNumber)
//                {
//                    switch (i)
//                    {
//                        case 0:
//                            {
//                                if (a[i] >= magicNumber)
//                                {
//                                    a[i] = 0;
//                                    Array.Resize(ref a, a.Length + 1);
//                                    a[0] = 1;
//                                    i = a.Length - 1;
//                                    flag = false;
//                                    break;
//                                }
//                                break;
//                            }
//                        case > 0:
//                            {
//                                a[i] = 0;
//                                if (a[i - 1] >= magicNumber)
//                                {
//                                    i--;
//                                }
//                                else
//                                {
//                                    a[i - 1]++;
//                                }
//                                break;
//                            }
//                        case < 0:
//                            {
//                                flag = false;
//                                break;
//                            }
//                    }
//                }
//                var ShortUrl1 = base62.Encode(Encoding.UTF8.GetString(a));
//                ShortUrl1 = CheckLengthShortUrl(ShortUrl1);
//                if (ShortUrl != ShortUrl1)
//                {
//                    ShortUrl = ShortUrl1;
//                    continue;
//                }

//                throw new Exception("Невозможно сократить ссылку");
//            }

//            return ShortUrl;
//        }

//        private Random _random = new();
//        public string GetRandomString()
//        {
//            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
//            return new string(Enumerable.Repeat(chars, 5)
//                .Select(s => s[_random.Next(s.Length)]).ToArray());
//        }
//    }
//}
