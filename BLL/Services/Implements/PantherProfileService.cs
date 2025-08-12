using BLL.DTOs;
using BLL.Services.Interfaces;
using DAL;
using DAL.Repositories.Interfaces;

namespace BLL.Services.Implements
{
    public class PantherProfileService : IPantherProfileService
    {
        private readonly IPantherProfileRepository _pantherProfileRepository;

        public PantherProfileService(IPantherProfileRepository pantherProfileRepository)
        {
            _pantherProfileRepository = pantherProfileRepository;
        }

        private void MapToPantherProfile(PantherProfileRequest request, PantherProfile pantherProfile)
        {
            pantherProfile.PantherTypeId = request.PantherTypeId;
            pantherProfile.PantherName = request.PantherName;
            pantherProfile.Weight = request.Weight;
            pantherProfile.Characteristics = request.Characteristics;
            pantherProfile.Warning = request.Warning;
            pantherProfile.ModifiedDate = DateTime.Now;
        }

        public async Task CreatePantherProfileAsync(PantherProfileRequest request)
        {
            var newPanther = new PantherProfile();
            MapToPantherProfile(request, newPanther);
            await _pantherProfileRepository.AddAsync(newPanther);
        }

        public async Task<int> DeletePantherProfileAsync(int id)
        {
            var panther = await _pantherProfileRepository.GetFirstOrDefaultAsync(l => l.PantherProfileId == id);
            if (panther == null)
            {
                throw new KeyNotFoundException($"PantherProfile with ID {id} not found.");

            }
            return await _pantherProfileRepository.DeleteAsync(panther);
        }

        public async Task<PantherProfile> GetPantherProfileByIdAsync(int id)
        {
            return await _pantherProfileRepository.GetFirstOrDefaultAsync(l => l.PantherProfileId == id, l => l.PantherType);
        }

        public async Task<(IEnumerable<PantherProfile> pantherProfiles, int totalCount, int totalPages, bool hasPrevious, bool hasNext)> GetPagedPantherProfilesAsync(int pageNumber, int pageSize)
        {
            return await _pantherProfileRepository.GetPagedAsync(pageNumber, pageSize, null, l => l.PantherType, l => l.ModifiedDate, true);
        }

        public async Task<(IEnumerable<PantherProfile> pantherProfiles, int totalCount, int totalPages, bool hasPrevious, bool hasNext)> SearchPantherProfilesAsync(int pageNumber, int pageSize, double? weight, string? pantherTypeName)
        {
            System.Linq.Expressions.Expression<Func<PantherProfile, bool>>? filter = null;

            if (weight.HasValue || !string.IsNullOrEmpty(pantherTypeName))
            {
                filter = l => (weight.HasValue && l.Weight == weight.Value) ||
                             (!string.IsNullOrEmpty(pantherTypeName) && l.PantherType != null && l.PantherType.PantherTypeName != null && l.PantherType.PantherTypeName.Contains(pantherTypeName));
            }

            return await _pantherProfileRepository.GetPagedAsync(pageNumber, pageSize, filter, l => l.PantherType, l => l.ModifiedDate, true);
        }

        public async Task UpdatePantherProfileAsync(int id, PantherProfileRequest request)
        {
            var panther = await _pantherProfileRepository.GetFirstOrDefaultAsync(l => l.PantherProfileId == id);
            if (panther == null)
            {
                throw new KeyNotFoundException($"PantherProfile with ID {id} not found.");

            }
            MapToPantherProfile(request, panther);
            await _pantherProfileRepository.UpdateAsync(panther);
        }
    }
}
