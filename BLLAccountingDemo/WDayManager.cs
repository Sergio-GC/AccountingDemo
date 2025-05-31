using EFAccounting;
using EFAccounting.Entities;
using Microsoft.EntityFrameworkCore;

namespace BLLAccountingDemo
{
    public class WDayManager
    {
        private Context _context;

        public WDayManager(Context context)
        {
            _context = context;
        }

        public void AddWDay(WDay wd)
        {
            _context.Wdays.Add(wd);
            _context.SaveChanges();
        }

        public void UpdateWDay(WDay wday) 
        {
            _context.Wdays
                .Where(wd => wd.Id == wday.Id)
                .ExecuteUpdate(u => u
                    .SetProperty(p => p.Kid, wday.Kid)
                    .SetProperty(p => p.Price, wday.Price)
                    .SetProperty(p => p.Arrival, wday.Arrival)
                    .SetProperty(p => p.Departure, wday.Departure)
                    .SetProperty(p => p.Date, wday.Date)
                );
            _context.SaveChanges();
        }

        public void DeleteWDay(WDay wDay)
        {
            _context.Wdays.Where(w => w.Id == wDay.Id).ExecuteDelete();
            _context.SaveChanges();
        }


        public float CalculateAmount(DateOnly StartDate, DateOnly EndDate, List<Kid> Kids)
        {
            float amount = 0f;
            List<WDay> wdays = new List<WDay>();

            foreach (Kid k in Kids) 
            {
                wdays.AddRange(
                    _context.Wdays.Where(
                        wd => wd.Kid == k 
                        && wd.Date >= StartDate 
                        && wd.Date <= EndDate
                    )
                );
            }
            
            foreach (WDay wday in wdays) 
            {
                TimeSpan WdayHours = (TimeOnly)wday.Departure! - (TimeOnly)wday.Arrival!;
                amount += (float)WdayHours.TotalHours * wday.Price.Value;
            }

            return amount;
        }
    }
}
