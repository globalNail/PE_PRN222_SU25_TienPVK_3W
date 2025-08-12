using BLL.DTOs;
using DAL;

namespace BLL.Services.Interfaces
{
    public interface IPantherProfileService
    {
        Task<(IEnumerable<PantherProfile> pantherProfiles, int totalCount, int totalPages, bool hasPrevious, bool hasNext)> GetPagedPantherProfilesAsync(int pageNumber, int pageSize);
        Task<(IEnumerable<PantherProfile> pantherProfiles, int totalCount, int totalPages, bool hasPrevious, bool hasNext)> SearchPantherProfilesAsync(int pageNumber, int pageSize, double? weight, string? PantherTypeName);
        Task<PantherProfile> GetPantherProfileByIdAsync(int id);
        Task CreatePantherProfileAsync(PantherProfileRequest request);
        Task UpdatePantherProfileAsync(int id, PantherProfileRequest request);
        Task<int> DeletePantherProfileAsync(int id);
    }
}
