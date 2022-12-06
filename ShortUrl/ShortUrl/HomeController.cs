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
            item ??= new();

            using var db = new URLContext();

            if (!string.IsNullOrEmpty(item.FullURL))
            {
                //todo: подумать, где лучше обрезать ссылку
                
                item.ChangeFullURL();

                //хэш полной ссылки
                if (string.IsNullOrEmpty(item.ShortURL)) item.HashURL();

                var query = db.Urls
                    .Where(b => b.ShortURL== item.ShortURL)
                    .FirstOrDefault();

                if (query == null)
                {
                    URL url1 = new URL { FullURL = item.FullURL, ShortURL = item.ShortURL };
                    db.Urls.Add(url1);
                    db.SaveChanges();
                }
                else if (query.FullURL != item.FullURL)
                {
                    item.RepeatHashURL();
                    return Index(item);
                }
               
            }
            ViewData["message"] = "yes";
            return View(db.Urls.ToList());
            //ViewData["Title"] = "Home page";
            //return View(item);
            //return Ok(item);          
        }

        // POST: HomeController
        [HttpPost]
		public ActionResult Create([FromForm] URL item)
		{
			try
			{   item ??= new();
                if (!string.IsNullOrEmpty(item.FullURL))
                {
                    item.HashURL();
                    //return Ok(item);
                    return RedirectToAction(nameof(Index), routeValues: item);
                } else
                return Redirect("home");
			}
			catch
			{
				return View();
			}
		}

		

    }
}