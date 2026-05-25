using Repositories.Models;

namespace Repositories.Repositories
{
    public interface ISystemAccountRepository
    {
        SystemAccount? GetAccountByEmail(string email);
        List<SystemAccount> GetAccounts();
        SystemAccount? GetAccountById(short id);
        void AddAccount(SystemAccount account);
        void UpdateAccount(SystemAccount account);
        void DeleteAccount(SystemAccount account);
    }

}