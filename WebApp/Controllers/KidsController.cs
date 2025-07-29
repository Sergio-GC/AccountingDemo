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
            if (Id == null || Id <= 0)
            {
                ModelState.AddModelError("", "Id cannot be null");
                return RedirectToAction("Kids");
            }
            await _PopulateViewBag();

            Kid kid = await _httpClient.GetFromJsonAsync<Kid>(_baseUrl + $"kids/Kid/{Id}");
            return View(kid);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Kid k, List<int> SiblingIds)
        {
            // 0. Initial check
            if(k == null)
            {
                Console.WriteLine("Error at initial check");
                await _PopulateViewBag();
                ModelState.AddModelError("", "Kid cannot be null");
                return View(k);
            }

            // 1. setup the list of ids for the sibling relationships and await for the response of the api
            SiblingIds.Insert(0, k.Id);

            HttpResponseMessage siblingsUpdateResponse = await _httpClient.PutAsJsonAsync($"{_baseUrl}kids/updateSiblings", SiblingIds);
            if (!siblingsUpdateResponse.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", $"There was an unexpected error: {siblingsUpdateResponse.StatusCode}");
                return View(k);
            }

            // 2. Update only the kid properties
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"{_baseUrl}kids/updateKid", k);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", $"There was an unexpected error: {response.StatusCode}");
                return View(k);
            }

            return RedirectToAction("Kids");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            Console.WriteLine($"Id: {Id}");
            if(Id == null || Id <= 0)
            {
                ModelState.AddModelError("", "Id cannot be null");
                return RedirectToAction("Kids");
            }

            HttpResponseMessage response = await _httpClient.DeleteAsync(_baseUrl + $"kids/delete/{Id}?siblings={false}");
            if (!response.IsSuccessStatusCode)
                ModelState.AddModelError("", $"Unexpected error: {response.StatusCode}");

            return RedirectToAction("Kids");
        }

        private async Task _PopulateViewBag()
        {
            ViewBag.kids = await _httpClient.GetFromJsonAsync<List<Kid>>(_baseUrl + "kids");
        }
    }
}
