using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShortUrl.Models;
using System.Text;

namespace ShortUrl
{
    [Route("/{ShortURL?}")]
    public class HomeController1 : Controller
    {
        [HttpGet]
        public IActionResult Index(string ShortURL)
        {
            //return View();
            using var db = new URLContext();
            var query = db.Urls
                .Where(b => b.ShortURL == ShortURL)
                .FirstOrDefault();

            if (query != null)
            {
                return Redirect("http://" + query.FullURL.ToString());
            }

            //todo: доделать переход на полную ссылку
            return Redirect("http://" + query.FullURL.ToString());
            // return View();
        }

    }
}
