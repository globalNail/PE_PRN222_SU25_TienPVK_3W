using BLL.DTOs;
using BLL.Services.Interfaces;
using DAL;
using DAL.Repositories.Interfaces;

namespace BLL.Services.Implements
{
    public class PantherAccountService : IPantherAccountService
    {
        private readonly IGenericRepository<PantherAccount> _repo;

        public PantherAccountService(IGenericRepository<PantherAccount> repo)
        {
            _repo = repo;
        }

        public async Task<PantherAccount> Authenticate(LoginRequest request)
        {
            var _pantherAccount = await _repo.GetFirstOrDefaultAsync(
                x => x.Email == request.Email && x.Password == request.Password);
            return _pantherAccount;
        }
    }
}
