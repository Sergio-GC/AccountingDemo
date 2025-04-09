using EFAccounting;
using EFAccounting.Entities;
using Microsoft.EntityFrameworkCore;

namespace BLLAccountingDemo
{
    public class PriceManager
    {
        private Context _context;
        
        public PriceManager(Context context)
        {
            _context = context;
        }

        public List<Price> GetPrices()
        {
            return _context.Prices.ToList();
        }

        public void AddPrice(Price price)
        {
            _context.Prices.Add(price);
            _context.SaveChanges();
        }

        public void UpdatePrice(Price price)
        {
            _context.Prices
                .Where(p => p.Id == price.Id)
                .ExecuteUpdate(u => u
                    .SetProperty(p => p.Value, price.Value)
                    .SetProperty(p => p.IsDeleted, price.IsDeleted)
                    .SetProperty(p => p.Label, price.Label)
            );
        }

        public void DeletePrice(Price price, bool deletion) 
        {
            _context.Prices
                .Where(p => p.Id == price.Id)
                .ExecuteUpdate(u => u
                    .SetProperty(p => p.IsDeleted, deletion)
                );
        }
    }
}
