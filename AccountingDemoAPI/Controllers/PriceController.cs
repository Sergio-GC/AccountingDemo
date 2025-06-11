using BLLAccountingDemo;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace AccountingDemoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PriceController : ControllerBase
    {
        private readonly PriceManager _manager;

        public PriceController(PriceManager manager)
        {
            _manager = manager;
        }

        [HttpGet("prices")]
        public async Task<ActionResult<List<Price>>> GetPrices()
        {
            return await _manager.GetPrices();
        }

        [HttpGet("price/{id}")]
        public async Task<ActionResult<Price>> GetPrice(int id)
        {
            return await _manager.GetPrice(id);
        }

        [HttpPost]
        public void AddPrice(Price price)
        {
            _manager.AddPrice(price);
        }

        [HttpPut]
        public void UpdatePrice(Price price)
        {
            _manager.UpdatePrice(price);
        }

        [HttpDelete]
        public void DeletePrice(Price price)
        {
            _manager.DeletePrice(price);
        }
    }
}
