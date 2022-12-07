using Microsoft.AspNetCore.Mvc;
using ShortUrl.Models;

namespace ShortUrl.Controllers
{
    [Route("/home")]
    public class GetShortURLController : Controller
    {
        [HttpGet]
        public IActionResult Index([FromQuery] URL? item)
        {
            item ??= new();
            using var db = new URLContext();
            HashManager hash = new(ref item);
            if (!string.IsNullOrEmpty(item.FullURL))
            {
                if (string.IsNullOrEmpty(item.ShortURL))
                {
                    hash.HashURL();
                }

                var flag = true;
                while (flag)
                {
                    var query = db.Urls
                        .FirstOrDefault(b => b.ShortURL == item.ShortURL);

                    if (query is null)
                    {
                        URL url1 = new() { FullURL = item.FullURL, ShortURL = item.ShortURL };
                        db.Urls.Add(url1);
                        db.SaveChanges();
                        flag = false;
                    }
                    else if (query.FullURL != item.FullURL)
                    {
                        hash.RepeatHashURL();
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            return Ok(item);
        }
    }
}