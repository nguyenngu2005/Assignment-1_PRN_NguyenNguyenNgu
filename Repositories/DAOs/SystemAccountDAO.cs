using Repositories.Models;
using System.Linq;

namespace Repositories.DAOs
{
    public class SystemAccountDAO
    {
        private static SystemAccountDAO? instance;
        private static readonly object instanceLock = new object();

        // Private constructor for blocking using new to create new object
        private SystemAccountDAO() { }

        // Singleton Pattern
        public static SystemAccountDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new SystemAccountDAO();
                    }
                    return instance;
                }
            }
        }

        // Function to get account by Email for Login
        public SystemAccount? GetAccountByEmail(FunewsManagementContext context, string email)
        {
            return context.SystemAccounts?.SingleOrDefault(a => a.AccountEmail == email);
        }
        // Các hàm bổ sung cho chức năng Quản lý của Admin
        public List<SystemAccount> GetAccounts(FunewsManagementContext context)
        {
            return context.SystemAccounts.ToList();
        }

        public SystemAccount? GetAccountById(FunewsManagementContext context, short id)
        {
            return context.SystemAccounts.SingleOrDefault(a => a.AccountId == id);
        }

        public void AddAccount(FunewsManagementContext context, SystemAccount account)
        {
            context.SystemAccounts.Add(account);
            context.SaveChanges();
        }

        public void UpdateAccount(FunewsManagementContext context, SystemAccount account)
        {
            var tracked = context.SystemAccounts.Local.FirstOrDefault(entry => entry.AccountId == account.AccountId);
            if (tracked != null && tracked != account)
            {
                context.Entry(tracked).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            context.SystemAccounts.Update(account);
            context.SaveChanges();
        }

        public void DeleteAccount(FunewsManagementContext context, SystemAccount account)
        {
            context.SystemAccounts.Remove(account);
            context.SaveChanges();
        }
    }
}