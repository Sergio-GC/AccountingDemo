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
        public List<WDay> GetWDays()
        {
            return _manager.GetWDays();
        }

        [HttpGet("wday/{id}")]
        public WDay GetWday(int id)
        {
            return _manager.GetWDay(id);
        }

        [HttpGet("allwdays")]
        public List<WDay> GetAllWDays()
        {
            return _manager.GetAllWDays();
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
