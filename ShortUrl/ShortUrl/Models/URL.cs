using Base62;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Security.Policy;
using System.Text;
using Crc32 = System.IO.Hashing.Crc32;

namespace ShortUrl.Models
{
    public class URL
    {
        public string FullURL { get; set; } = string.Empty;
        public string ShortURL { get; set; } = string.Empty;

        Base62Converter base62 = new();

        /// <summary>
        /// Хэш полной ссылки алгоритмом CRC32 и запись получившегося значения в переменную для короткой ссылки
        /// </summary>
        public void HashURL()
        {
            var a = Encoding.UTF8.GetString(Crc32.Hash(Encoding.UTF8.GetBytes(FullURL)));
            ShortURL = base62.Encode(a);
            CheckLengthShortURL();
        }

        /// <summary>
        /// Проверяет, соответсвует ли ссылка диапазону от 7 до 10 символов
        /// </summary>
        void CheckLengthShortURL()
        {
            if (ShortURL.Length > 10)
            {
                ShortURL = ShortURL.Remove(9, ShortURL.Length - 9);
            }
            else if (ShortURL.Length < 7)
            {
                for (int num = 7 - ShortURL.Length; num > 0;)
                {
                    ShortURL = ShortURL + base62.Encode(num.ToString());
                    num = 7 - ShortURL.Length;
                }
            }
        }

        /// <summary>
        /// При одинаковом хэше и разных полных ссылках меняем хэш
        /// </summary>
        public void RepeatHashURL()
        {
            ShortURL = base62.Encode(ShortURL.Length.ToString()) + ShortURL;
            CheckLengthShortURL();
        }
    }

    public class URLContext : DbContext
    {
        public URLContext() { }
        public URLContext(DbContextOptions<URLContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseMySQL("server=localhost;user=user;password=password-007;database=urls");
        }
        public DbSet<URL> Urls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<URL>(entity =>
            {
                entity.HasKey(x => x.ShortURL);
                entity.Property(x => x.FullURL);
            });
        }

    }
}
