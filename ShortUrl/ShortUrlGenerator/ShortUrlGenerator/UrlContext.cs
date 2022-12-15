using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8618
namespace ShortUrlGenerator
{
    public class UrlContext : DbContext
    {
        public UrlContext( DbContextOptions<UrlContext> options ) :base(options) { }

        public DbSet<URL> Urls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<URL>(entity =>
            {
                entity.HasKey(e => e.ShortUrl);
                entity.Property(e => e.FullUrl);
            });
        }
    }
}
