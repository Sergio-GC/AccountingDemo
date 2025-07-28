using DTO;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

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

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await _PopulateViewBag();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Kid k)
        {
            if (k == null)
            {
                await _PopulateViewBag();

                ModelState.AddModelError("", "Kid cannot be null.");
                return View(k);
            }

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_baseUrl + "kids", k);
            if (!response.IsSuccessStatusCode)
            {
                await _PopulateViewBag();

                ModelState.AddModelError("", $"There was an error during the creation of the kid: {response.StatusCode}");
                return View(k);
            }

            return RedirectToAction("Kids");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            if(Id == null || Id <= 0)
            {
                ModelState.AddModelError("", "Id cannot be null");
                return RedirectToAction("Kids");
            }

            Kid kid = await _httpClient.GetFromJsonAsync<Kid>(_baseUrl + $"kids/Kid/{Id}");
            return View(kid);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Kid k)
        {
            if(k == null)
            {
                await _PopulateViewBag();
                ModelState.AddModelError("", "Kid cannot be null");
                return View(k);
            }

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_baseUrl + "kids", k);
            if (!response.IsSuccessStatusCode)
            {
                await _PopulateViewBag();
                ModelState.AddModelError("", $"There was an unexpected error: {response.StatusCode}");

                return View(k);
            }

            return RedirectToAction("Kids");
        }

        private async Task _PopulateViewBag()
        {
            ViewBag.kids = await _httpClient.GetFromJsonAsync<List<Kid>>(_baseUrl + "kids");
        }
    }
}
