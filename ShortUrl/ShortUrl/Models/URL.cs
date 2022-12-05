using Microsoft.EntityFrameworkCore;
using System;
namespace ShortUrl.Models
{
	public class URL
	{
       // public int Id { get; set; }

        public string FullURL { get; set; }

		//public ??? Hash { get; set; }
		public string ShortURL { get; set; }
	}

	public class URLContext : DbContext
	{
		public URLContext() { }
		public URLContext(DbContextOptions<URLContext> options) : base (options) 
		{
			
		}
		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
   //        if(!options.IsConfigured)
			//{
				options.UseMySQL("server=localhost;user=user;password=password-007;database=urls");				
			//}
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
