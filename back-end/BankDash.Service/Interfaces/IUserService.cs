using BankDash.Model.Dto;
using BankDash.Model.Enitity;

namespace BankDash.Service.Interfaces
{
    public interface IUserService
    {
        Task<string> LoginUserAsync(Login loginDto);
    }
}
