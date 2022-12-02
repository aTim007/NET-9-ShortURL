using System;
namespace ShortUrl.Models
{
	public class URL
	{
		public int Id { get; set; }
		public string FullURL { get; set; }

		//public ??? Hash { get; set; }
		public string ShortURL { get; set; }
	}
}
