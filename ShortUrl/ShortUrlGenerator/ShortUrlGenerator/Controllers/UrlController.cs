using Microsoft.AspNetCore.Mvc;
using ShortUrlGenerator;

namespace ShortUrlGenerator.Controllers
{
    [ApiController]

    public class UrlController : ControllerBase
    {
        private readonly UrlManager _urlManager;

        public UrlController(UrlManager shortUrlManager)
        {
            _urlManager = shortUrlManager;
        }
        [Route("")]
        public IActionResult Get()
        {
            return Ok("Для генерации короткой ссылки перейдите на '/shorturl?url=', для перехода по короткой ссылке: '/fullurl?url=' и введите после знака '=' вашу ссылку");
        }


        [Route("/shorturl")]
        [HttpGet]
        public IActionResult GetShortUrl([FromQuery] string url)
        {
            var result = _urlManager.GenerateShortUrl(url);
            return Ok(result);
        }

        [Route("/fullurl")]
        [HttpGet]
        public IActionResult GetFullUrl([FromQuery] string url)
        {
            Console.WriteLine("fullurl");
            return Ok(new URL());
        }

    }
}