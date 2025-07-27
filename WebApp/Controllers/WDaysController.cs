using DTO;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

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

        public async Task<IActionResult> Calendar()
        {
            List<WDay> wDays = await _httpClient.GetFromJsonAsync<List<WDay>>(_baseUrl + "wdays/wdays");
            return View(wDays);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateViewBag();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(WDay wd, int kidId, int priceId)
        {
            await PopulateViewBag();

            // First, check if Kid exists
            Kid? kid = await _httpClient.GetFromJsonAsync<Kid>(_baseUrl + "kids/kid/" + kidId);

            if(kid == null)
            {
                ModelState.AddModelError("", "The selected kid does not exist");
                return View();
            }
            wd.Kid = kid;

            // Later, check if price exists
            Price? price = await _httpClient.GetFromJsonAsync<Price>(_baseUrl + "price/price/" + priceId);
            if (price == null)
            {
                ModelState.AddModelError("", "The selected price does not exist");
                return View();
            }
            wd.Price = price;

            // Then, check that, if present, departure > arrival
            if(wd.Arrival != null && wd.Departure != null){
                if(wd.Arrival >= wd.Departure)
                {
                    ModelState.AddModelError("", "The arrival hour cannot be after the departure hour.");
                    return View("Index");
                }
            }

            // Finally, prepare the API request and send it to create the WDay
            string jsonWDay = JsonSerializer.Serialize(wd);
            StringContent content = new StringContent(jsonWDay, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "Wdays", content);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", $"Error while creating a new WDay: {response.StatusCode}");
                return View();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateTime(WDay wday, bool IsArrival)
        {
            if(wday == null)
            {
                ModelState.AddModelError("", "WDay cannot be empty.");
                return RedirectToAction("Index");
            }

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_baseUrl + "wdays/updateTime?IsArrival=" + IsArrival, wday);
            if (!response.IsSuccessStatusCode)
                ModelState.AddModelError("", $"There was an error while updating the arrival time. Error code: {response.StatusCode}");

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if(id == null)
            {
                ModelState.AddModelError("", "WDay cannot be empty.");
                return RedirectToAction("Index");
            }

            await PopulateViewBag();
            WDay wday = await _httpClient.GetFromJsonAsync<WDay>(_baseUrl + $"wdays/wday/{id}");
            return View(wday);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(WDay wday, int kidId, int priceId)
        {
            if (wday == null)
            {
                await PopulateViewBag();
                ModelState.AddModelError("", "WDay cannot be empty.");
                return View(wday);
            }

            // First, check if Kid exists
            Kid? kid = await _httpClient.GetFromJsonAsync<Kid>(_baseUrl + "kids/kid/" + kidId);
            if (kid == null)
            {
                ModelState.AddModelError("", "The selected kid does not exist");
                return View();
            }
            wday.Kid = kid;

            // Later, check if price exists
            Price? price = await _httpClient.GetFromJsonAsync<Price>(_baseUrl + "price/price/" + priceId);
            if (price == null)
            {
                ModelState.AddModelError("", "The selected price does not exist");
                return View();
            }
            wday.Price = price;

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_baseUrl + "wdays", wday);
            if (!response.IsSuccessStatusCode)
            {
                await PopulateViewBag();
                ModelState.AddModelError("", $"There was an error while updating the WDay: {response.StatusCode}");
                return View(wday);
            }

            return RedirectToAction("Index");
        }


        private async Task PopulateViewBag()
        {
            List<Kid> kids = await _httpClient.GetFromJsonAsync<List<Kid>>(_baseUrl + "kids");
            List<Price> prices = await _httpClient.GetFromJsonAsync<List<Price>>(_baseUrl + "price/prices");

            ViewBag.kids = kids;
            ViewBag.prices = prices;
        }
    }
}
