using EFAccounting;
using DTO;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace BLLAccountingDemo
{
    public class PriceManager
    {
        private Context _context;
        private readonly IMapper _mapper;
        
        public PriceManager(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<Price> GetPrices()
        {
            return _mapper.Map<List<Price>>(_context.Prices.ToList());
        }

        public Price GetPrice(int id)
        {
            return _mapper.Map<Price>(_context.Prices.Where(p => p.Id == id).Single());
        }

        public void AddPrice(Price price)
        {
            _context.Prices.Add(_mapper.Map<EFAccounting.Entities.Price>(price));
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

        public void DeletePrice(Price price) 
        {
            Price ogPrice = _mapper.Map<Price>(_context.Prices.Where(p => p.Id == price.Id).Single());
            bool deletion = !ogPrice.IsDeleted;

            _context.Prices
                .Where(p => p.Id == price.Id)
                .ExecuteUpdate(u => u
                    .SetProperty(p => p.IsDeleted, deletion)
                );
        }
    }
}
