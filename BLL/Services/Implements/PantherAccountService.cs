using BLL.DTOs;
using BLL.Services.Interfaces;
using DAL;
using DAL.Repositories.Interfaces;

namespace BLL.Services.Implements
{
    public class PantherAccountService : IPantherAccountService
    {
        private readonly IPantherAccountRepository _pantherAccountRepository;

        public PantherAccountService(IPantherAccountRepository pantherAccountRepository)
        {
            _pantherAccountRepository = pantherAccountRepository;
        }

        public async Task<PantherAccount> Authenticate(LoginRequest request)
        {
            var _pantherAccount = await _pantherAccountRepository.GetFirstOrDefaultAsync(
                x => x.Email == request.Email && x.Password == request.Password);
            return _pantherAccount;
        }
    }
}
