using AutoMapper;
using EFAccounting;
using DTO;
using Microsoft.EntityFrameworkCore;

namespace BLLAccountingDemo
{
    public class WDayManager
    {
        private Context _context;
        private readonly IMapper _mapper;

        public WDayManager(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get workdays for the next 2 weeks
        /// </summary>
        /// <returns></returns>
        public async Task<List<WDay>> GetWDays()
        {
            DateOnly currentDay = DateOnly.FromDateTime(DateTime.Now);
            DateOnly lastDay = currentDay.AddDays(14);

            return _mapper.Map<List<WDay>>(await _context.Wdays.Where(wd => wd.Date >= currentDay && wd.Date <= lastDay).ToListAsync());
        }

        public async Task<WDay> GetWDay(int id)
        {
            return _mapper.Map<WDay>(await _context.Wdays.Where(wd => wd.Id == id).SingleAsync());
        }

        /// <summary>
        /// Get ALL workdays
        /// </summary>
        /// <returns></returns>
        public async Task<List<WDay>> GetAllWDays()
        {
            return _mapper.Map<List<WDay>>(await _context.Wdays.ToListAsync());
        }

        public void AddWDay(WDay wd)
        {
            EFAccounting.Entities.WDay wDay = _mapper.Map<EFAccounting.Entities.WDay>(wd);

            _context.Attach(wDay.Kid);
            _context.Attach(wDay.Price);

            _context.Wdays.Add(wDay);
            _context.SaveChanges();
        }

        public async Task UpdateWDay(WDay wday) 
        {
            EFAccounting.Entities.WDay updatedWday = _mapper.Map<EFAccounting.Entities.WDay>(wday);
            _context.Wdays
                .Where(wd => wd.Id == updatedWday.Id)
                .ExecuteUpdate(u => u
                    .SetProperty(p => p.Arrival, updatedWday.Arrival)
                    .SetProperty(p => p.Departure, updatedWday.Departure)
                    .SetProperty(p => p.Date, updatedWday.Date)
                );

            EFAccounting.Entities.WDay wDay = await _context.Wdays.Where(w => w.Id == updatedWday.Id).SingleAsync();
            wDay.Price = updatedWday.Price;
            wDay.Kid = updatedWday.Kid;

            _context.SaveChanges();
        }

        public void DeleteWDay(WDay wDay)
        {
            _context.Wdays.Where(w => w.Id == wDay.Id).ExecuteDelete();
        }


        public float CalculateAmount(DateOnly StartDate, DateOnly EndDate, List<Kid> Kids)
        {
            float amount = 0f;
            List<EFAccounting.Entities.WDay> wdays = new();

            foreach (Kid k in Kids) 
            {
                EFAccounting.Entities.Kid efkid = _mapper.Map<EFAccounting.Entities.Kid>(k);
                wdays.AddRange(
                    _context.Wdays.Where(
                        wd => wd.Kid == efkid 
                        && wd.Date >= StartDate 
                        && wd.Date <= EndDate
                    )
                );
            }
            
            foreach (EFAccounting.Entities.WDay wday in wdays) 
            {
                TimeSpan WdayHours = (TimeOnly)wday.Departure! - (TimeOnly)wday.Arrival!;
                amount += (float)WdayHours.TotalHours * wday.Price.Value;
            }

            return amount;
        }
    }
}
