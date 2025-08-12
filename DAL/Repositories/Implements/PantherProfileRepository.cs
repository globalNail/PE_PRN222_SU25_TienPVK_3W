using DAL.Repositories.Interfaces;

namespace DAL.Repositories.Implements
{
    public class PantherProfileRepository : GenericRepository<PantherProfile>, IPantherProfileRepository
    {
        public PantherProfileRepository(Su25pantherDbContext context) : base(context)
        {
        }
    }
}
