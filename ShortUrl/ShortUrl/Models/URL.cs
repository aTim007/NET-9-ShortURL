using Microsoft.EntityFrameworkCore;

namespace ShortUrl.Models
{
    public class URL
    {
        public string FullURL { get; set; } = string.Empty;
        public string ShortURL { get; set; } = string.Empty;
    }

    public class URLContext : DbContext
    {
        public URLContext() : base() { }
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
