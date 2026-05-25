using Repositories.DAOs;
using Repositories.Models;

namespace Repositories.Repositories
{
    public class SystemAccountRepository : ISystemAccountRepository
    {
        private readonly FunewsManagementContext _context;

        // Tiêm FunewsManagementContext từ Program.cs vào đây
        public SystemAccountRepository(FunewsManagementContext context)
        {
            _context = context;
        }

        public SystemAccount? GetAccountByEmail(string email)
        {
            // Gọi Singleton DAO
            return SystemAccountDAO.Instance.GetAccountByEmail(_context, email);
        }
        public List<SystemAccount> GetAccounts()
            => SystemAccountDAO.Instance.GetAccounts(_context);

        public SystemAccount? GetAccountById(short id)
            => SystemAccountDAO.Instance.GetAccountById(_context, id);

        public void AddAccount(SystemAccount account)
            => SystemAccountDAO.Instance.AddAccount(_context, account);

        public void UpdateAccount(SystemAccount account)
            => SystemAccountDAO.Instance.UpdateAccount(_context, account);

        public void DeleteAccount(SystemAccount account)
            => SystemAccountDAO.Instance.DeleteAccount(_context, account);
    }
}