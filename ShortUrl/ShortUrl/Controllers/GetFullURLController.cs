using Microsoft.AspNetCore.Mvc;
using ShortUrl.Models;

namespace ShortUrl.Controllers
{
    [Route("/{ShortURL?}")]
    public class GetFullURLController : Controller
    {
        [HttpGet]
        public IActionResult Index(string shortURL)
        {
            using var db = new URLContext();
            var query = db.Urls
                .FirstOrDefault(b => b.ShortURL == shortURL);

            if (query is not null)
            {
                var i = query.FullURL.IndexOf("://");
                if (i != -1)
                {
                    query.FullURL = query.FullURL[(i + 3)..];
                }
                return Redirect("https://" + query.FullURL);
            }
            return Redirect("home");
        }
    }
}
