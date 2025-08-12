using DAL.Repositories.Interfaces;

namespace DAL.Repositories.Implements
{
    public class PantherTypeRepository : GenericRepository<PantherType>, IPantherTypeRepository
    {
        public PantherTypeRepository(Su25pantherDbContext context) : base(context)
        {
        }
    }
}
