using Microsoft.AspNetCore.Mvc;
using Services;
using System.Linq;

namespace NguyenNguyenNguMVC.Controllers
{
    public class NewsFeedController : Controller
    {
        private readonly INewsArticleService _newsArticleService;

        public NewsFeedController(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        public IActionResult Index()
        {
            // Lấy danh sách bài viết, lọc ra những bài đang Active (Published) và sắp xếp mới nhất lên đầu
            var publishedNews = _newsArticleService.GetNewsArticles()
                                .Where(n => n.NewsStatus == true)
                                .OrderByDescending(n => n.CreatedDate)
                                .ToList();

            return View(publishedNews);
        }
    }
}