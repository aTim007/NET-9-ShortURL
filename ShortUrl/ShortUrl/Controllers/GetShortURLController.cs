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
            if (item is not null)
            {
                if (!string.IsNullOrEmpty(item.FullURL))
                {
                    item.ShortURL = URLManager.GetShortUrl(item);
                }
            }
            return Ok(item);
        }
    }
}