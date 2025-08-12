using BLL.DTOs;
using DAL;

namespace BLL.Services.Interfaces
{
    public interface IPantherAccountService
    {
        Task<PantherAccount> Authenticate(LoginRequest request);

    }
}
