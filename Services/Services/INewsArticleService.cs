using Repositories.Models;
using System.Collections.Generic;

namespace Services
{
    public interface INewsArticleService
    {
        List<NewsArticle> GetNewsArticles();
        NewsArticle? GetNewsArticleById(string id);
        void AddNewsArticle(NewsArticle newsArticle);
        void UpdateNewsArticle(NewsArticle newsArticle);
        void DeleteNewsArticle(string id);
        List<NewsArticle> GetNewsArticlesByPeriod(System.DateTime startDate, System.DateTime endDate);
    }
}