using Microsoft.AspNetCore.Mvc;
using ShortUrl.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ShortUrl.Controllers
{
    [Route("/{ShortUrl?}")]
    public class GetFullUrlController : Controller
    {
        private readonly UrlManager _urlManager;

        public GetFullUrlController(UrlManager urlManager)
        {
            _urlManager = urlManager;
        }

        [HttpGet]
        public IActionResult Index(string shortUrl)
        {
            var result = _urlManager.GetFullUrl(shortUrl);
            if (result is null)
            {
                return Redirect("home");
            }
            return Redirect("https://" + result);
        }
    }
}
