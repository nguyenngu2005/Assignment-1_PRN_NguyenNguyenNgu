using Microsoft.Extensions.Configuration;
using Repositories.Models;
using Repositories.Repositories;

namespace Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly ISystemAccountRepository _repository;
        private readonly IConfiguration _configuration;

        // Tiêm Repository và Configuration (để đọc appsettings) vào Service
        public SystemAccountService(ISystemAccountRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public SystemAccount? Authenticate(string email, string password)
        {
            // Thêm dấu ? vào sau string để báo cho C# biết giá trị này có thể null
            string? adminEmail = _configuration["AdminAccount:Email"];
            string? adminPassword = _configuration["AdminAccount:Password"];

            if (email == adminEmail && password == adminPassword)
            {
                return new SystemAccount
                {
                    AccountEmail = email,
                    AccountRole = 0
                };
            }

            var account = _repository.GetAccountByEmail(email);

            if (account != null && account.AccountPassword == password)
            {
                return account;
            }

            return null;
        }
        public SystemAccount? GetAccountByEmail(string email)
        {
            return _repository.GetAccountByEmail(email);
        }
        public List<SystemAccount> GetAccounts()
    => _repository.GetAccounts();

        public SystemAccount? GetAccountById(short id)
            => _repository.GetAccountById(id);

        public void AddAccount(SystemAccount account)
            => _repository.AddAccount(account);

        public void UpdateAccount(SystemAccount account)
            => _repository.UpdateAccount(account);

        public void DeleteAccount(short id)
        {
            var account = _repository.GetAccountById(id);
            if (account != null)
            {
                _repository.DeleteAccount(account);
            }
        }
    }
}