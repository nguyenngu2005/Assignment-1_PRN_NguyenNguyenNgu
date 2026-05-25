using Repositories.Models;
using System.Collections.Generic;

namespace Repositories.Repositories
{
    public interface INewsArticleRepository
    {
        List<NewsArticle> GetNewsArticles();
        NewsArticle? GetNewsArticleById(string id);
        void AddNewsArticle(NewsArticle newsArticle);
        void UpdateNewsArticle(NewsArticle newsArticle);
        void DeleteNewsArticle(NewsArticle newsArticle);
        List<NewsArticle> GetNewsArticlesByPeriod(System.DateTime startDate, System.DateTime endDate);
    }
}