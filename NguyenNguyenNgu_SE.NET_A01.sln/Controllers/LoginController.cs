using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Services;
using NguyenNguyenNguMVC.Models;

namespace NguyenNguyenNguMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly ISystemAccountService _accountService;
        private readonly IConfiguration _configuration;

        public LoginController(ISystemAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // If already authenticated via Session, direct them to their dashboard directly
            if (HttpContext.Session.GetString("Email") != null)
            {
                int? role = HttpContext.Session.GetInt32("Role");
                return RedirectBasedOnRole(role ?? -1);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Rule 1: Authenticate against the default Admin account from appsettings.json
            var adminEmail = _configuration["AdminAccount:Email"] ?? "admin@FUNewsManagementSystem.org";
            var adminPassword = _configuration["AdminAccount:Password"] ?? "@@abc123@@";

            if (model.Email == adminEmail && model.Password == adminPassword)
            {
                HttpContext.Session.SetString("Email", model.Email);
                HttpContext.Session.SetInt32("Role", 0); // 0: Admin
                HttpContext.Session.SetInt32("AccountId", 0);

                return RedirectToAction("Index", "SystemAccount");
            }

            // Rule 2: Authenticate against regular accounts stored in the database
            var account = _accountService.Authenticate(model.Email, model.Password);

            if (account != null)
            {
                int role = account.AccountRole ?? -1;

                // Save user identity properties inside standard Global Session
                HttpContext.Session.SetString("Email", account.AccountEmail ?? "");
                HttpContext.Session.SetInt32("Role", role);
                HttpContext.Session.SetInt32("AccountId", account.AccountId);

                return RedirectBasedOnRole(role);
            }

            // Authentication failed: render high-fidelity glass toast/alert message
            ViewBag.ErrorMessage = "Invalid email or password. Please try again.";
            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Discard active session parameters
            return RedirectToAction("Index");
        }

        private IActionResult RedirectBasedOnRole(int role)
        {
            return role switch
            {
                0 => RedirectToAction("Index", "SystemAccount"), // Admin -> Redirect to Account/Report Management
                1 => RedirectToAction("Index", "Category"),      // Staff -> Redirect to Category/News Article Management
                2 => RedirectToAction("Index", "NewsFeed"),      // Lecturer -> Redirect to News Feed (View Active News Only)
                _ => RedirectToAction("Index", "Home")
            };
        }
    }
}