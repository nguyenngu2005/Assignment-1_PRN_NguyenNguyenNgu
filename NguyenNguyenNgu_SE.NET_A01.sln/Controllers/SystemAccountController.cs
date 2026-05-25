using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Repositories.Models;
using Services;

namespace NguyenNguyenNguMVC.Controllers

{
    public class SystemAccountController : Controller
    {
        private readonly ISystemAccountService _accountService;
        private readonly INewsArticleService _newsArticleService;

        // Cập nhật lại Hàm khởi tạo để nhận cả 2 Service
        public SystemAccountController(ISystemAccountService accountService, INewsArticleService newsArticleService)
        {
            _accountService = accountService;
            _newsArticleService = newsArticleService;
        }

        // GET: Giao diện tạo báo cáo thống kê
        public IActionResult Report(System.DateTime? startDate, System.DateTime? endDate)
        {
            // Bảo mật: Chỉ Admin mới được xem
            int? role = HttpContext.Session.GetInt32("AccountId"); // Hoặc dùng key "Role" tùy cấu hình phân quyền của em
            int? realRole = HttpContext.Session.GetInt32("Role");
            if (realRole != 0) return RedirectToAction("Index", "Login");

            var reportData = new List<NewsArticle>();

            // Nếu Admin có chọn cả ngày bắt đầu và ngày kết thúc thì tiến hành lọc
            if (startDate.HasValue && endDate.HasValue)
            {
                // Để lọc chính xác trọn vẹn ngày kết thúc, ta chỉnh mốc thời gian cuối ngày thành 23:59:59
                var endOfDate = endDate.Value.Date.AddDays(1).AddTicks(-1);
                reportData = _newsArticleService.GetNewsArticlesByPeriod(startDate.Value, endOfDate);

                // Giữ lại ngày đã chọn hiển thị lên ô Input của View
                ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
                ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");
            }

            return View(reportData);
        }
        // GET: Hiển thị danh sách tài khoản
        public IActionResult Index()
        {
            // Bảo mật: Chỉ cho phép Admin (Role = 0) truy cập
            int? role = HttpContext.Session.GetInt32("Role");
            if (role != 0)
            {
                return RedirectToAction("Index", "Login");
            }

            var accounts = _accountService.GetAccounts();
            return View(accounts);
        }

        // POST: Xử lý Thêm tài khoản mới
        // POST: Xử lý Thêm tài khoản mới
        [HttpPost]
        public IActionResult Create(SystemAccount account)
        {
            try
            {
                // 1. Bắt lỗi trùng ID
                if (_accountService.GetAccountById(account.AccountId) != null)
                {
                    TempData["Error"] = $"Thêm thất bại: Mã tài khoản ID '{account.AccountId}' đã tồn tại trong hệ thống!";
                    return RedirectToAction(nameof(Index));
                }

                // 2. Bắt lỗi trùng Email
                var checkEmail = _accountService.GetAccountByEmail(account.AccountEmail ?? ""); if (checkEmail != null)
                {
                    TempData["Error"] = $"Thêm thất bại: Email '{account.AccountEmail}' đã được sử dụng!";
                    return RedirectToAction(nameof(Index));
                }

                if (ModelState.IsValid)
                {
                    _accountService.AddAccount(account);
                    TempData["Success"] = "Thêm tài khoản mới thành công!";
                }
                else
                {
                    TempData["Error"] = "Dữ liệu Form nhập vào không hợp lệ.";
                }
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = "Lỗi hệ thống trong quá trình thêm mới: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Xử lý Cập nhật tài khoản
        [HttpPost]
        public IActionResult Edit(SystemAccount account)
        {
            try
            {
                // Bắt lỗi trùng Email (Trường hợp họ đổi sang một Email đã có sẵn của người khác)
                var checkEmail = _accountService.GetAccountByEmail(account.AccountEmail ?? "");

                // Nếu tìm thấy email đó trong DB, nhưng ID lại không phải của chính tài khoản đang sửa
                if (checkEmail != null && checkEmail.AccountId != account.AccountId)
                {
                    TempData["Error"] = $"Cập nhật thất bại: Email '{account.AccountEmail}' đang được sử dụng bởi một tài khoản khác!";
                    return RedirectToAction(nameof(Index));
                }

                if (ModelState.IsValid)
                {
                    _accountService.UpdateAccount(account);
                    TempData["Success"] = "Cập nhật tài khoản thành công!";
                }
                else
                {
                    TempData["Error"] = "Dữ liệu Form nhập vào không hợp lệ.";
                }
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = "Lỗi hệ thống trong quá trình cập nhật: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Xử lý Xóa tài khoản
        [HttpPost]
        public IActionResult Delete(short id)
        {
            // Bảo mật bổ sung: Không cho phép Admin tự xóa chính mình dựa trên Session id
            int? currentAdminId = HttpContext.Session.GetInt32("AccountID");
            if (currentAdminId.HasValue && currentAdminId.Value == id)
            {
                TempData["Error"] = "Bạn không thể tự xóa tài khoản Admin đang đăng nhập!";
                return RedirectToAction(nameof(Index));
            }

            _accountService.DeleteAccount(id);
            TempData["Success"] = "Đã xóa tài khoản thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}