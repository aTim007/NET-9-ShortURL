using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;
using ShortUrl.Models;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.IO.Hashing;
using Crc32 = System.IO.Hashing.Crc32;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ShortUrl
{
	[Route("/home")]
	public class HomeController : Controller
	{       
        [HttpGet]
        public IActionResult Index([FromQuery] URL? item)
        {
            item ??= new URL
            {
                FullURL = string.Empty,
                ShortURL = string.Empty,
            };
            using var db = new URLContext();
            if (!string.IsNullOrEmpty(item.FullURL))
            {
                var a = Crc32.Hash(Encoding.UTF8.GetBytes(item.FullURL));
                item.ShortURL = Encoding.UTF8.GetString(a);

                var query = db.Urls
                    .Where(b => b.ShortURL== item.ShortURL)
                    .FirstOrDefault();

                if (query == null)
                {
                    URL url1 = new URL { FullURL = item.FullURL, ShortURL = item.ShortURL };
                    db.Urls.Add(url1);
                    db.SaveChanges();
                }
               
            }
            return View(db.Urls.ToList());
            // return View(item);
            //ViewData["Title"] = "Home page";
            //return View(item);
            //return Ok(item);          
        }

        // POST: HomeController
        [HttpPost]
		public ActionResult Create([FromForm] URL item)
		{
			try
			{
				var a = Crc32.Hash(Encoding.UTF8.GetBytes(item.FullURL));
				item.ShortURL = Encoding.UTF8.GetString(a);
				//return Ok(item);
                return RedirectToAction(nameof(Index), routeValues: item);
			}
			catch
			{
				return View();
			}
		}

		

    }
}