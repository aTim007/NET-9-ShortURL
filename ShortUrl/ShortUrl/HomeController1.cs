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
            using var db = new URLContext();
            var query = db.Urls
                .Where(b => b.ShortURL == ShortURL)
                .FirstOrDefault();

            //return query != null ? ( Redirect("http://" + (i != -1 ? query.FullURL.Substring(i): query.FullURL.ToString()))) : Redirect("home");
            
            return query!=null? Redirect("http://" + query.FullURL) : Redirect("home");
                       
            // return View();
        }

    }
}
