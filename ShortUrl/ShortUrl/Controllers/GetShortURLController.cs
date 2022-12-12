using Microsoft.AspNetCore.Mvc;
using ShortUrl.Models;

namespace ShortUrl.Controllers
{
    [Route("/home")]
    public class GetShortUrlController : Controller
    {
        private readonly UrlManager _urlManager;

        public GetShortUrlController(UrlManager urlManager)
        {
            _urlManager = urlManager;
        }

        [HttpGet]
        public IActionResult Index([FromQuery] URL? item)
        {
            item ??= new();
            if (item is not null)
            {
                if (!string.IsNullOrEmpty(item.FullUrl))
                {
                    _urlManager.GetShortUrl(ref item);
                }
            }
            return Ok(item);
        }
    }
}