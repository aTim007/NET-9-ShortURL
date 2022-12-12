using Microsoft.EntityFrameworkCore;
using ShortUrl.Models;

#pragma warning disable CS8618

namespace ShortUrl
{
    public class UrlContext : DbContext
    {
        public UrlContext(DbContextOptions<UrlContext> options) : base(options)
        {

        }

        public DbSet<URL> Urls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<URL>(entity =>
            {
                entity.HasKey(x => x.ShortUrl);
                entity.Property(x => x.FullUrl);
            });
        }
    }
}
