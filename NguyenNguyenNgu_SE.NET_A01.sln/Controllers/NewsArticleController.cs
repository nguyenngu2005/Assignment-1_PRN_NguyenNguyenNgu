using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repositories.Models;
using Services;
using System.Collections.Generic;
using System.Linq;

namespace NguyenNguyenNguMVC.Controllers
{
    public class NewsArticleController : Controller
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly FunewsManagementContext _context;

        public NewsArticleController(INewsArticleService newsArticleService, ICategoryService categoryService, FunewsManagementContext context)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _context = context;
        }

        // GET: Hiển thị danh sách
        public IActionResult Index()
        {
            int? role = HttpContext.Session.GetInt32("Role");
            if (role != 1 && role != 0)
            {
                return RedirectToAction("Index", "Login");
            }

            // Lấy danh sách bài viết
            var articles = _newsArticleService.GetNewsArticles();

            // Lấy danh sách Category và truyền sang View qua ViewBag để làm thẻ <select>
            ViewBag.Categories = _categoryService.GetCategories();

            // --- BẮT ĐẦU PHẦN QUẢN LÝ TAG THEO ĐỀ BÀI ---
            // 1. Lấy danh sách các tag chưa thuộc bài viết nào (cho Form Tạo mới)
            ViewBag.AvailableTags = _context.Tags
                .Where(t => !t.NewsArticles.Any())
                .ToList();

            // 2. Lấy danh sách ID của các tag đã bị chiếm dụng bởi bài viết khác
            ViewBag.TakenTagIds = _context.Tags
                .Where(t => t.NewsArticles.Any())
                .Select(t => t.TagId)
                .ToList();

            // 3. Lấy toàn bộ danh sách Tag (cho Form Cập nhật)
            ViewBag.AllTags = _context.Tags.ToList();
            // --- KẾT THÚC PHẦN QUẢN LÝ TAG ---

            return View(articles);
        }

        // POST: Tạo mới bài viết kèm theo Tags
        [HttpPost]
        public IActionResult Create(NewsArticle article, List<int> selectedTagIds)
        {
            // Lấy ID người đang đăng nhập (Staff/Admin) từ Session gán vào CreatedById
            int? accountId = HttpContext.Session.GetInt32("AccountID");
            if (accountId.HasValue)
            {
                article.CreatedById = (short)accountId.Value;
            }

            // Gán các Tag được chọn (đã lọc các tag chưa được chiếm dụng ở view)
            if (selectedTagIds != null && selectedTagIds.Any())
            {
                article.Tags = _context.Tags.Where(t => selectedTagIds.Contains(t.TagId)).ToList();
            }

            _newsArticleService.AddNewsArticle(article);
            TempData["Success"] = "Tạo bài viết thành công!";
            return RedirectToAction(nameof(Index));
        }

        // POST: Cập nhật bài viết kèm theo Tags
        [HttpPost]
        public IActionResult Edit(NewsArticle article, List<int> selectedTagIds)
        {
            // Nạp danh sách các Tag được chọn để truyền sang Service
            if (selectedTagIds != null)
            {
                article.Tags = _context.Tags.Where(t => selectedTagIds.Contains(t.TagId)).ToList();
            }

            _newsArticleService.UpdateNewsArticle(article);
            TempData["Success"] = "Cập nhật bài viết thành công!";
            return RedirectToAction(nameof(Index));
        }

        // POST: Xóa bài viết
        [HttpPost]
        public IActionResult Delete(string id)
        {
            _newsArticleService.DeleteNewsArticle(id);
            TempData["Success"] = "Đã xóa bài viết thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}