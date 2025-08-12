using BLL.Services.Interfaces;
using DAL;
using DAL.Repositories.Interfaces;

namespace BLL.Services.Implements
{
    public class PantherTypeService : IPantherTypeService
    {
        private readonly IPantherTypeRepository _pantherTypeRepository;

        public PantherTypeService(IPantherTypeRepository pantherTypeRepository)
        {
            _pantherTypeRepository = pantherTypeRepository;
        }

        public async Task<IEnumerable<PantherType>> GetAllPantherTypesAsync()
        {
            return await _pantherTypeRepository.GetAllAsync();
        }
    }
}
