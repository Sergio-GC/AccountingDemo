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
            Kid oldKid = _context.Kids.Where(k => k.Id == kid.Id).Single();

            // Update each of its attributes
            oldKid.SiblingTo = kid.SiblingTo;
            oldKid.SiblingFrom = kid.SiblingFrom;
            oldKid.BirthDate = kid.BirthDate;
            oldKid.Name = kid.Name;
            oldKid.LastName = kid.LastName;

            _context.SaveChanges();
        }

        public void RemoveKid(Kid kid, bool siblings)
        {
            if(siblings)
            {
                // Get the kid's siblings
                List<Kid> kidSiblings = kid.SiblingFrom!;

                /*
                 * No longuer needed as using soft delete
                 * 
                 * 
                // Remove self from siblings list
                if (kidSiblings.Contains(kid))
                    kidSiblings.RemoveAll(k => k.Id == kid.Id);
                 *
                 *
                // Remove the registered siblings (kid objects still exist)
                kid.SiblingFrom!.Clear();
                kid.SiblingTo!.Clear();
                _context.SaveChanges();
                */


                // Delete original kid --> Set soft deletion to true
                //_context.Remove(kid);
                _context.Kids
                    .Where(k => k.Id == kid.Id)
                    .ExecuteUpdate(b => b
                        .SetProperty(u => u.IsDeleted, true)
                    );
                _context.SaveChanges();

                // Delete original kid's siblings
                foreach(Kid k in kidSiblings)
                {
                    // Delete the other siblings, in case of >2
                    if(k.SiblingFrom?.Count > 0)
                    {
                        RemoveKid(k, true);
                    }
                    // Delete the current kid
                    else
                    {
                        //_context.Remove(k);
                        _context.Kids
                            .Where(ki => ki.Id == k.Id)
                            .ExecuteUpdate(b => b
                                .SetProperty(u => u.IsDeleted, true)
                        );
                        _context.SaveChanges();
                    }
                }
            }
            else
            {
                // Delete possible sibling relationships
                /* kid.SiblingTo?.Clear();
                 kid.SiblingFrom?.Clear();
                 _context.SaveChanges();*/

                // Remove kid
                //_context.Remove(kid);
                _context.Kids
                    .Where(k => k.Id == kid.Id)
                    .ExecuteUpdate(b => b
                        .SetProperty(u => u.IsDeleted, true)
                    );
                _context.SaveChanges();
            }
        }
        #endregion
    }
}
