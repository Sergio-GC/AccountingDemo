using DTO;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class KidsController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public KidsController(ILogger<HomeController> logger, HttpClient httpClient, IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClient;
            _baseUrl = configuration["ApiBaseUrl"]!;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Kids()
        {
            List<Kid> kids = new List<Kid>();
            kids = await _httpClient.GetFromJsonAsync<List<Kid>>(_baseUrl + "kids");

            return View(kids);
        }
    }
}
