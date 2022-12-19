using Microsoft.AspNetCore.Mvc;
using ShortUrlGenerator;
using System.Text;

namespace ShortUrlGenerator.Controllers
{
    [ApiController]

    public class UrlController : ControllerBase
    {
        private readonly UrlManager _urlManager;
        private readonly StringBuilder _strBuilder = new();

        public UrlController(UrlManager shortUrlManager)
        {
            _urlManager = shortUrlManager;
        }

        [Route("")]
        public IActionResult Get()
        {
            return Ok(Message("��� ��������� �������� ������ ��������� �� '/shorturl?url=', ��� �������� �� �������� ������: '/fullurl?url=' � ������� ����� ����� '=' ���� ������"));
        }
        private IActionResult Get(string message)
        {
            return Ok(Message(message));
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
            var result = _urlManager.GetFullUrl(url);
            return result is null ? Get("��������� ���� ������") : Redirect("https://" + result);
        }
        private string Message(string message)
        {
            _strBuilder.Clear();
            _strBuilder.Append(message);
            return _strBuilder.ToString();
        }
    }
}