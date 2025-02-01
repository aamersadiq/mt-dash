using BankDash.Model.Enitity;

namespace BankDash.Service.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
    }
}
