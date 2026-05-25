using Repositories.Models;

namespace Services
{
    public interface ISystemAccountService
    {
        SystemAccount? Authenticate(string email, string password);
        List<SystemAccount> GetAccounts();
        SystemAccount? GetAccountById(short id);
        SystemAccount? GetAccountByEmail(string email);
        void AddAccount(SystemAccount account);
        void UpdateAccount(SystemAccount account);
        void DeleteAccount(short id);
    }
}