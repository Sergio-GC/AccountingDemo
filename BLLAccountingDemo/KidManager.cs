using EFAccounting;
using EFAccounting.Entities;

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
            Kid oldKid = _context.Kids.Where(k => k.Id == kid.Id).Single();

            // Update each of its attributes
            oldKid.SiblingTo = kid.SiblingTo;
            oldKid.SiblingFrom = kid.SiblingFrom;
            oldKid.BirthDate = kid.BirthDate;
            oldKid.Name = kid.Name;
            oldKid.LastName = kid.LastName;

            _context.SaveChanges();
        }

        public void RemoveKid(Kid kid)
        {
            _context.Kids.Remove(kid);
        }
        #endregion
    }
}
