using DTO;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class WDaysController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public WDaysController(ILogger<HomeController> logger, HttpClient httpClient, IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClient;
            _baseUrl = configuration["ApiBaseUrl"]!;
        }

        public async Task<IActionResult> Index()
        {
            List<WDay> wDays = await _httpClient.GetFromJsonAsync<List<WDay>>(_baseUrl + "wdays/wdays");
            return View(wDays);
        }
    }
}
