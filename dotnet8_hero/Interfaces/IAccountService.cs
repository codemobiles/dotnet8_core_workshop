using dotnet8_hero.Entities;

namespace dotnet8_hero.Interfaces
{
    public interface IAccountService
    {
        Task Register(Account account);
        Task<Account> Login(string username, string password);
        string GenerateToken(Account account);
        Account GetInfo(string accessToken);

    }
}