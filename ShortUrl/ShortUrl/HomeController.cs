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
			if (!string.IsNullOrEmpty(item.FullURL))
            {
                var a = Crc32.Hash(Encoding.UTF8.GetBytes(item.FullURL));
                item.ShortURL = Encoding.UTF8.GetString(a);
            }
            ViewData["Title"] = "Home page";
			return View(item);
		}

		// POST: HomeController
		[HttpPost]
		public ActionResult Create([FromForm] URL item)
		{
			try
			{
				var a = Crc32.Hash(Encoding.UTF8.GetBytes(item.FullURL));
				item.ShortURL = Encoding.UTF8.GetString(a);
				return Ok(item);
                //return RedirectToAction(nameof(Index), routeValues: item);
			}
			catch
			{
				return View();
			}
		}
	}
}