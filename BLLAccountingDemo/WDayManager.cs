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
        public List<WDay> GetWDays()
        {
            DateOnly currentDay = DateOnly.FromDateTime(DateTime.Now);
            DateOnly lastDay = currentDay.AddDays(14);

            return _mapper.Map<List<WDay>>(_context.Wdays.Where(wd => wd.Date >= currentDay && wd.Date <= lastDay).ToList());
        }

        public WDay GetWDay(int id)
        {
            return _mapper.Map<WDay>(_context.Wdays.Where(wd => wd.Id == id).Single());
        }

        /// <summary>
        /// Get ALL workdays
        /// </summary>
        /// <returns></returns>
        public List<WDay> GetAllWDays()
        {
            return _mapper.Map<List<WDay>>(_context.Wdays.ToList());
        }

        public void AddWDay(WDay wd)
        {
            EFAccounting.Entities.WDay wDay = _mapper.Map<EFAccounting.Entities.WDay>(wd);

            _context.Attach(wDay.Kid);
            _context.Attach(wDay.Price);

            _context.Wdays.Add(wDay);
            _context.SaveChanges();
        }

        public void UpdateWDay(WDay wday) 
        {
            EFAccounting.Entities.WDay updatedWday = _mapper.Map<EFAccounting.Entities.WDay>(wday);
            _context.Wdays
                .Where(wd => wd.Id == updatedWday.Id)
                .ExecuteUpdate(u => u
                    .SetProperty(p => p.Arrival, updatedWday.Arrival)
                    .SetProperty(p => p.Departure, updatedWday.Departure)
                    .SetProperty(p => p.Date, updatedWday.Date)
                );

            EFAccounting.Entities.WDay wDay = _context.Wdays.Where(w => w.Id == updatedWday.Id).Single();
            wDay.Price = updatedWday.Price;
            wDay.Kid = updatedWday.Kid;

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
