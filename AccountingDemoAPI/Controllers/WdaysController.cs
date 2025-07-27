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

        [HttpPost("updateTime")]
        public async Task<IActionResult> UpdateArrival([FromBody] WDay wday, [FromQuery] bool IsArrival)
        {
            if(IsArrival)
                wday.Arrival = RoundTime(IsArrival);
            else
                wday.Departure = RoundTime(IsArrival);

            try
            {
                await _manager.UpdateWDay(wday);
                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }

        }

        private TimeOnly RoundTime(bool up)
        {
            TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);
            int minutes = currentTime.Minute;
            int minutesMod5 = minutes % 5;

            if (minutesMod5 != 0)
            {
                if (up)
                {
                    currentTime = currentTime.AddMinutes(5 - minutesMod5);
                }
                else
                {
                    currentTime = currentTime.AddMinutes(-minutesMod5);
                }
            }

            return currentTime;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateWDay(WDay wd)
        {
            await _manager.UpdateWDay(wd);
            return Ok();
        }

        [HttpPost]
        public void CreateWDay(WDay wd)
        {
            _manager.AddWDay(wd);
        }

        [HttpDelete("delete/{id}")]
        public void DeleteWDay(int id) 
        {
            _manager.DeleteWDay(id);
        }
    }
}
