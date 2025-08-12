using DAL.Repositories.Interfaces;

namespace DAL.Repositories.Implements
{
    public class PantherAccountRepository : GenericRepository<PantherAccount>, IPantherAccountRepository
    {
        public PantherAccountRepository(Su25pantherDbContext context) : base(context)
        {
        }
    }
}
