using AutoMapper;
using EFAccounting;
using DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

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

        public void AddBulkWDays(WDaySubmission submission)
        {
            List<EFAccounting.Entities.Kid> kids = _context.Kids.Where(k => submission.KidsIds.Contains(k.Id)).ToList();
            EFAccounting.Entities.Price price = _context.Prices.Where(p => p.Id == submission.PriceId).Single();

            List<EFAccounting.Entities.WDay> wdays = new();
            foreach(var kid in kids)
            {
                for(int i = 0; i < submission.Arrivals.Count; i++)
                {
                    wdays.Add(new EFAccounting.Entities.WDay
                    {
                        Kid = kid,
                        Price = price,
                        Arrival = submission.Arrivals[i],
                        Departure = submission.Departures[i],
                        Date = submission.Date
                    });
                }
            }

            _context.Wdays.AddRange(wdays);
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

            EFAccounting.Entities.WDay ogwDay = await _context.Wdays.Where(w => w.Id == updatedWday.Id).SingleAsync();
            ogwDay.Price = await _context.Prices
                .Where(p => p.Id == updatedWday.Price.Id)
                .SingleAsync();

            ogwDay.Kid = await _context.Kids
                .Where(k => k.Id == updatedWday.Kid.Id)
                .SingleAsync();

            _context.SaveChanges();
        }

        public void DeleteWDay(int wdId)
        {
            _context.Wdays.Where(w => w.Id == wdId).ExecuteDelete();
        }

        public async Task<List<InvoiceSummary>> GetSummary(InvoiceFormData formData)
        {
            List<int> ids = new();
            if(formData.KidSelectinoId == -1)
            {
                // Get the Id of all kids
                ids.AddRange(_context.Kids.Select(k => k.Id).ToList());
            }
            else
            {
                // Get the Ids of the kid's siblings
                if (!formData.WithSiblings.Equals("false", StringComparison.OrdinalIgnoreCase))
                    ids.AddRange(_context.SiblingRelationships.Where(s => s.FromKidId == formData.KidSelectinoId).Select(sr => sr.ToKidId).ToList());
                
                ids.Add(formData.KidSelectinoId);
            }

            DateOnly startDate = DateOnly.Parse(formData.PeriodFrom);
            DateOnly endDate = DateOnly.Parse(formData.PeriodTo);

            List<EFAccounting.Entities.WDay> wdays = 
                _context.Wdays.Where(wd => ids.Contains(wd.Kid.Id) && wd.Date >= startDate && wd.Date <= endDate).ToList();

            var groupedWdays = wdays.GroupBy(gb => new { gb.Date, gb.Kid });

            List<InvoiceSummary> summary = new();
            float totalAmounts = 0f;
            float totalHours = 0f;

            foreach(var group in groupedWdays.OrderBy(o => o.Key.Date).ThenBy(tb => tb.Key.Kid.Name))
            {
                string label = "";
                float hoursForKid = 0f;
                float amountForKid = 0f;

                foreach (var item in group)
                {
                    label += $"from {item.Arrival:HH\\:mm} to {item.Departure:HH\\:mm} ";

                    TimeSpan time = (TimeSpan)(item.Departure - item.Arrival);
                    // Convert the time into a float and round it to the previous 2 decimal points
                    float timeHours = (float)Math.Round((Decimal)((float)time.TotalHours), 2, MidpointRounding.ToNegativeInfinity);
                    

                    hoursForKid += timeHours;
                    amountForKid += timeHours * item.Price.Value;
                }

                totalHours += hoursForKid;
                totalAmounts += amountForKid;

                summary.Add(new InvoiceSummary
                {
                    Label = label,
                    Hours = hoursForKid,
                    Amount = amountForKid,
                    Date = group.Key.Date,
                    KidName = group.Key.Kid.Name,
                    TotalAmount = 0f, // Temp 0 value, will be updated at the end
                    TotalHours = 0f // Temp 0 value, will be updated at the end
                });
            }

            // Update the totals
            foreach (var sum in summary)
            {
                sum.TotalAmount = totalAmounts;
                sum.TotalHours = totalHours;
            }

            return summary;
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
