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

            _context.Kids.Where(k => k.Id == updatedKid.Id).ExecuteUpdate(
                p => p.SetProperty(p => p.BirthDate, updatedKid.BirthDate)
                .SetProperty(p => p.LastName, updatedKid.LastName)
                .SetProperty(p => p.Name, updatedKid.Name)
                .SetProperty(p => p.IsDeleted, updatedKid.IsDeleted)
            );
        }

        public async Task UpdateSiblings(List<int> ids)
        {
            // 1. Clear the siblings of all the current siblings of the baseKid
            EFAccounting.Entities.Kid kid = await _context.Kids.Where(k => k.Id == ids[0]).SingleAsync();

            // Only delete the sibling relationships if the kid already has them
            if(kid.Siblings.Count() > 0)
            {
                await _context.SiblingRelationships.Where(sr => ids.Contains(sr.FromKidId) || ids.Contains(sr.ToKidId)).ExecuteDeleteAsync();
                await _context.SaveChangesAsync();
            }

            // 2. Create the new relationships
            await _UpdateRelationships(ids);
            await _context.SaveChangesAsync();
        }

        private async Task _UpdateRelationships(List<int> siblingsIds)
        {
            if (siblingsIds.Count() < 2)
                return;

            // Prepare the data
            int baseKidId = siblingsIds[0];
            siblingsIds.RemoveAt(0);

            // Create the relationships bidirectionnally
            foreach (int id in siblingsIds)
            {
                _context.SiblingRelationships.Add(new EFAccounting.Entities.SiblingRelationship { FromKidId = baseKidId, ToKidId = id });
                _context.SiblingRelationships.Add(new EFAccounting.Entities.SiblingRelationship { ToKidId = baseKidId, FromKidId = id });
            }

            // Repeat for the rest of siblings
            await _UpdateRelationships(siblingsIds);
        }

        public async Task RemoveKid(int Id, bool siblings)
        {
            Kid ogKid = _profile.Map<Kid>(_context.Kids.Where(k => k.Id == Id).Single());

            if(siblings)
            {
                // Get list of siblings
                List<Kid> kidsSiblings = new();
                List<int> fromIds = await _context.SiblingRelationships.Where(r => r.FromKidId == Id).Select(s => s.ToKidId).ToListAsync();

                kidsSiblings = _profile.Map<List<Kid>>(await _context.Kids.Where(k => fromIds.Contains(k.Id)).ToListAsync()).ToList();

                // Delete them (soft deletion)
                foreach(Kid k in kidsSiblings)
                {
                    bool ogValue = k.IsDeleted;
                    _context.Kids.Where(ki => ki.Id == k.Id).ExecuteUpdate(u => u.SetProperty(p => p.IsDeleted, !ogValue));
                }
            }

            bool deleted = ogKid.IsDeleted;
            _context.Kids.Where(k => k.Id == Id).ExecuteUpdate(u => u.SetProperty(p => p.IsDeleted, !deleted));


            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
