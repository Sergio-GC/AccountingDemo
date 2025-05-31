using EFAccounting;
using EFAccounting.Entities;
using Microsoft.EntityFrameworkCore;

namespace BLLAccountingDemo
{
    public class KidManager
    {
        private Context _context;

        public KidManager(Context context)
        {
            _context = context;
        }

        #region CRUD METHODS
        public void CreateKid(Kid kid)
        {
            _context.Kids.Add(kid);
            _context.SaveChanges();
        }

        public List<Kid> GetKids()
        {
            List<Kid> result = _context.Kids.ToList();
            return result;
        }

        public Kid GetKid(int kidId)
        {
            Kid kid = _context.Kids.Where(k => k.Id == kidId).Single();
            return kid;
        }

        public void UpdateKid(Kid kid)
        {
            _context.Kids.Where(k => k.Id == kid.Id).ExecuteUpdate(
                u => u.SetProperty(p => p.SiblingTo, kid.SiblingTo)
                .SetProperty(p => p.SiblingFrom, kid.SiblingFrom)
                .SetProperty(p => p.BirthDate, kid.BirthDate)
                .SetProperty(p => p.LastName, kid.LastName)
                .SetProperty(p => p.Name, kid.Name)
                );

            _context.SaveChanges();
        }

        public void RemoveKid(Kid kid, bool siblings)
        {
            if(siblings)
            {
               // Get list of siblings
               List<Kid> kidsSiblings = kid.SiblingFrom!.ToList();

                // Delete them (soft deletion)
                foreach(Kid k in kidsSiblings)
                {
                    bool ogValue = k.IsDeleted;
                    _context.Kids.Where(ki => ki.Id == k.Id).ExecuteUpdate(u => u.SetProperty(p => p.IsDeleted, !ogValue));
                }
            }

            bool deleted = kid.IsDeleted;
            _context.Kids.Where(k => k.Id == kid.Id).ExecuteUpdate(u => u.SetProperty(p => p.IsDeleted, !deleted));


            _context.SaveChanges();
        }
        #endregion
    }
}
