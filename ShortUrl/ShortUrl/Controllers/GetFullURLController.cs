using Microsoft.AspNetCore.Mvc;
using ShortUrl.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ShortUrl.Controllers
{
    [Route("/{ShortURL?}")]
    public class GetFullURLController : Controller
    {
        [HttpGet]
        public IActionResult Index(string shortURL)
        {
            var result = URLManager.GetFullUrl(shortURL);
            if (result is null)
            {
                return Redirect("home");
            }
            return Redirect("https://" + result);
        }
    }
}
