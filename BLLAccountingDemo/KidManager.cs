using EFAccounting;
using DTO;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace BLLAccountingDemo
{
    public class KidManager
    {
        private Context _context;
        private readonly IMapper _profile;

        public KidManager(Context context, IMapper profile)
        {
            _context = context;
            _profile = profile;
        }

        #region CRUD METHODS
        public void CreateKid(Kid kid)
        {
            _context.Kids.Add(_profile.Map<EFAccounting.Entities.Kid>(kid));
            _context.SaveChanges();
        }

        public async Task<List<Kid>> GetKids()
        {
            return _profile.Map<List<Kid>>(await _context.Kids.ToListAsync());
        }

        public async Task<Kid> GetKid(int kidId)
        {
            Kid kid = _profile.Map<Kid>(await _context.Kids.Where(k => k.Id == kidId).SingleAsync());
            return kid;
        }

        public void UpdateKid(Kid kid)
        {
            EFAccounting.Entities.Kid updatedKid = _profile.Map<EFAccounting.Entities.Kid>(kid);
            bool hasSiblings = updatedKid.Siblings.Count > 0;

            _context.Kids.Where(k => k.Id == updatedKid.Id).ExecuteUpdate(
                p => p.SetProperty(p => p.BirthDate, updatedKid.BirthDate)
                .SetProperty(p => p.LastName, updatedKid.LastName)
                .SetProperty(p => p.Name, updatedKid.Name)
                .SetProperty(p => p.IsDeleted, updatedKid.IsDeleted)
                .SetProperty(p => p.Siblings, updatedKid.Siblings)
                );

            //_context.SaveChanges();
        }

        public async Task RemoveKid(Kid kid, bool siblings)
        {
            Kid ogKid = _profile.Map<Kid>(_context.Kids.Where(k => k.Id == kid.Id).Single());

            if(siblings)
            {
                // Get list of siblings
                List<Kid> kidsSiblings = new();
                List<int> fromIds = await _context.SiblingRelationships.Where(r => r.FromKidId == kid.Id).Select(s => s.ToKidId).ToListAsync();

                kidsSiblings = _profile.Map<List<Kid>>(await _context.Kids.Where(k => fromIds.Contains(k.Id)).ToListAsync()).ToList();

                // Delete them (soft deletion)
                foreach(Kid k in kidsSiblings)
                {
                    bool ogValue = k.IsDeleted;
                    _context.Kids.Where(ki => ki.Id == k.Id).ExecuteUpdate(u => u.SetProperty(p => p.IsDeleted, !ogValue));
                }
            }

            bool deleted = kid.IsDeleted;
            _context.Kids.Where(k => k.Id == kid.Id).ExecuteUpdate(u => u.SetProperty(p => p.IsDeleted, !deleted));


            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
