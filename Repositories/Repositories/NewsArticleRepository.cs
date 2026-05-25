using Repositories.DAOs;
using Repositories.Models;
using System.Collections.Generic;

namespace Repositories.Repositories
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        private readonly FunewsManagementContext _context;

        public NewsArticleRepository(FunewsManagementContext context)
        {
            _context = context;
        }

        public List<NewsArticle> GetNewsArticles()
            => NewsArticleDAO.Instance.GetNewsArticles(_context);

        public NewsArticle? GetNewsArticleById(string id)
            => NewsArticleDAO.Instance.GetNewsArticleById(_context, id);

        public void AddNewsArticle(NewsArticle newsArticle)
            => NewsArticleDAO.Instance.AddNewsArticle(_context, newsArticle);

        public void UpdateNewsArticle(NewsArticle newsArticle)
            => NewsArticleDAO.Instance.UpdateNewsArticle(_context, newsArticle);

        public void DeleteNewsArticle(NewsArticle newsArticle)
            => NewsArticleDAO.Instance.DeleteNewsArticle(_context, newsArticle);
        public List<NewsArticle> GetNewsArticlesByPeriod(System.DateTime startDate, System.DateTime endDate)
            => NewsArticleDAO.Instance.GetNewsArticlesByPeriod(_context, startDate, endDate);
    }

}