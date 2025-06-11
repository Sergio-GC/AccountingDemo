using BLLAccountingDemo;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace AccountingDemoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WdaysController : ControllerBase
    {
        private WDayManager _manager;

        public WdaysController(WDayManager manager)
        {
            _manager = manager;
        }

        [HttpGet("wdays")]
        public async Task<ActionResult<List<WDay>>> GetWDays()
        {
            return await _manager.GetWDays();
        }

        [HttpGet("wday/{id}")]
        public async Task<ActionResult<WDay>> GetWday(int id)
        {
            return await _manager.GetWDay(id);
        }

        [HttpGet("allwdays")]
        public async Task<ActionResult<List<WDay>>> GetAllWDays()
        {
            return await _manager.GetAllWDays();
        }

        [HttpPut]
        public void UpdateWDay(WDay wd)
        {
            _manager.UpdateWDay(wd);
        }

        [HttpPost]
        public void CreateWDay(WDay wd)
        {
            _manager.AddWDay(wd);
        }

        [HttpDelete]
        public void DeleteWDay(WDay wd) 
        {
            _manager.DeleteWDay(wd);
        }
    }
}
