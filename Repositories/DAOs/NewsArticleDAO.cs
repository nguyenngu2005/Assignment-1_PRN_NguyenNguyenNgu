using Microsoft.EntityFrameworkCore;
using Repositories.Models;
using System.Collections.Generic;
using System.Linq;

namespace Repositories.DAOs
{
    public class NewsArticleDAO
    {
        private static NewsArticleDAO? instance;
        private static readonly object instanceLock = new object();

        private NewsArticleDAO() { }

        public static NewsArticleDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new NewsArticleDAO();
                    }
                    return instance;
                }
            }
        }

        // Lấy danh sách bài viết (kèm theo thông tin Category, người tạo, và Tags)
        public List<NewsArticle> GetNewsArticles(FunewsManagementContext context)
        {
            return context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .ToList();
        }

        public NewsArticle? GetNewsArticleById(FunewsManagementContext context, string id)
        {
            return context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .SingleOrDefault(n => n.NewsArticleId == id);
        }

        public void AddNewsArticle(FunewsManagementContext context, NewsArticle newsArticle)
        {
            context.NewsArticles.Add(newsArticle);
            context.SaveChanges();
        }

        public void UpdateNewsArticle(FunewsManagementContext context, NewsArticle newsArticle)
        {
            var tracked = context.NewsArticles.Local.FirstOrDefault(entry => entry.NewsArticleId == newsArticle.NewsArticleId);
            if (tracked != null && tracked != newsArticle)
            {
                context.Entry(tracked).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            context.NewsArticles.Update(newsArticle);
            context.SaveChanges();
        }

        public void DeleteNewsArticle(FunewsManagementContext context, NewsArticle newsArticle)
        {
            context.NewsArticles.Remove(newsArticle);
            context.SaveChanges();
        }
        // Hàm bổ sung cho chức năng làm Report của Admin
        public List<NewsArticle> GetNewsArticlesByPeriod(FunewsManagementContext context, System.DateTime startDate, System.DateTime endDate)
        {
            return context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate)
                .OrderByDescending(n => n.CreatedDate)
                .ToList();
        }
    }
}