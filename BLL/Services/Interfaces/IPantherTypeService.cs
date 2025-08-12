
using DAL;

namespace BLL.Services.Interfaces
{
    public interface IPantherTypeService
    {
        Task<IEnumerable<PantherType>> GetAllPantherTypesAsync();
    }
}
