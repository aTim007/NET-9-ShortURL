using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShortUrl.Models;
using System.Net;
using System.Text;
using System.Web;

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

                //todo: проблема с большой ссылкой с поиска гугла
                int i = query.FullURL.IndexOf("://");
                if (query !=null)
                {
                    if (i != -1)
                    {
                      query.FullURL = query.FullURL.Substring(i + 3);
                    }

                //RedirectResult("http://" + (query.FullURL);
                  var url = new UriBuilder(query.FullURL);
                    Console.WriteLine(url.Uri.ToString());
                  return Redirect(url.Uri.ToString());

                }
                return Redirect("home");
                //return query != null ? (Redirect("http://" + (i != -1 ? query.FullURL.Substring(i + 3) : query.FullURL.ToString()))) : Redirect("home");
           
            //return query!=null? Redirect("http://" + query.FullURL) : Redirect("home");
                       
            // return View();
        }

    }
}
