using BLL.Services.Interfaces;
using DAL;
using DAL.Repositories.Interfaces;

namespace BLL.Services.Implements
{
    public class PantherTypeService : IPantherTypeService
    {
        private readonly IGenericRepository<PantherType> _repo;

        public PantherTypeService(IGenericRepository<PantherType> repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<PantherType>> GetAllPantherTypesAsync()
        {
            return await _repo.GetAllAsync();
        }
    }
}
