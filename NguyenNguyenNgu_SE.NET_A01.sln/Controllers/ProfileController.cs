using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Repositories.Models;
using Services;

namespace NguyenNguyenNguMVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ISystemAccountService _accountService;
        private readonly IConfiguration _configuration;

        public ProfileController(ISystemAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        // GET: Hiển thị thông tin cá nhân
        public IActionResult Index()
        {
            // Lấy ID người dùng hiện tại từ Session
            int? accountId = HttpContext.Session.GetInt32("AccountId");
            var emailInSession = HttpContext.Session.GetString("Email");

            if (accountId == null || string.IsNullOrEmpty(emailInSession))
            {
                return RedirectToAction("Index", "Login");
            }

            // Bảo mật/Hỗ trợ: Tài khoản Admin mặc định cấu hình từ appsettings.json
            var adminEmail = _configuration["AdminAccount:Email"] ?? "admin@FUNewsManagementSystem.org";
            if (accountId == 0 || emailInSession.Equals(adminEmail, System.StringComparison.OrdinalIgnoreCase))
            {
                var adminAccount = new SystemAccount
                {
                    AccountId = 0,
                    AccountEmail = adminEmail,
                    AccountName = "System Administrator",
                    AccountRole = 0,
                    AccountPassword = _configuration["AdminAccount:Password"] ?? "@@abc123@@"
                };
                return View(adminAccount);
            }

            // Đối với các tài khoản lưu trữ dưới cơ sở dữ liệu
            var account = _accountService.GetAccountById((short)accountId.Value);
            if (account == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return View(account);
        }

        // POST: Xử lý cập nhật thông tin
        [HttpPost]
        public IActionResult UpdateProfile(SystemAccount updatedAccount)
        {
            int? accountId = HttpContext.Session.GetInt32("AccountId");
            var emailInSession = HttpContext.Session.GetString("Email");

            if (accountId == null || string.IsNullOrEmpty(emailInSession))
            {
                return RedirectToAction("Index", "Login");
            }

            // 1. Bảo mật: Không cho phép chỉnh sửa tài khoản Admin mặc định từ appsettings.json
            var adminEmail = _configuration["AdminAccount:Email"] ?? "admin@FUNewsManagementSystem.org";
            if (accountId == 0 || emailInSession.Equals(adminEmail, System.StringComparison.OrdinalIgnoreCase))
            {
                TempData["Error"] = "Cập nhật thất bại: Hồ sơ Quản trị viên mặc định được quản lý cố định thông qua cấu hình hệ thống (appsettings.json) để đảm bảo an ninh!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // 2. Cập nhật tài khoản trong DB
                var existingAccount = _accountService.GetAccountById((short)accountId.Value);
                if (existingAccount != null)
                {
                    // Kiểm tra xem Email mới nhập có bị trùng với người khác không
                    var checkEmail = _accountService.GetAccountByEmail(updatedAccount.AccountEmail ?? "");
                    if (checkEmail != null && checkEmail.AccountId != existingAccount.AccountId)
                    {
                        TempData["Error"] = "Cập nhật thất bại: Email này đã được sử dụng bởi một tài khoản khác!";
                        return RedirectToAction(nameof(Index));
                    }

                    // Cập nhật các trường cho phép sửa
                    existingAccount.AccountName = updatedAccount.AccountName;
                    existingAccount.AccountEmail = updatedAccount.AccountEmail;

                    // Chỉ cập nhật mật khẩu nếu người dùng có nhập
                    if (!string.IsNullOrEmpty(updatedAccount.AccountPassword))
                    {
                        existingAccount.AccountPassword = updatedAccount.AccountPassword;
                    }

                    _accountService.UpdateAccount(existingAccount);
                    TempData["Success"] = "Cập nhật hồ sơ cá nhân thành công!";

                    // Cập nhật lại Session Email hiển thị trên góc màn hình
                    HttpContext.Session.SetString("Email", existingAccount.AccountEmail ?? "");
                }
                else
                {
                    TempData["Error"] = "Không tìm thấy thông tin tài khoản tương ứng trong hệ thống!";
                }
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = "Lỗi hệ thống khi cập nhật: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}