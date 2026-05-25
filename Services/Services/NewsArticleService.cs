using Repositories.Models;
using Repositories.Repositories;
using System;
using System.Collections.Generic;

namespace Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _repository;

        public NewsArticleService(INewsArticleRepository repository)
        {
            _repository = repository;
        }

        public List<NewsArticle> GetNewsArticles() => _repository.GetNewsArticles();

        public NewsArticle? GetNewsArticleById(string id) => _repository.GetNewsArticleById(id);

        public void AddNewsArticle(NewsArticle newsArticle)
        {
            // Tự động gán ngày tạo lúc thêm mới
            newsArticle.CreatedDate = DateTime.Now;
            _repository.AddNewsArticle(newsArticle);
        }

        public void UpdateNewsArticle(NewsArticle newsArticle)
        {
            // Lấy bài viết cũ lên để cập nhật từng trường, tránh lỗi tracking của Entity Framework
            var existing = _repository.GetNewsArticleById(newsArticle.NewsArticleId);
            if (existing != null)
            {
                existing.NewsTitle = newsArticle.NewsTitle;
                existing.Headline = newsArticle.Headline;
                existing.NewsContent = newsArticle.NewsContent;
                existing.NewsSource = newsArticle.NewsSource;
                existing.CategoryId = newsArticle.CategoryId;
                existing.NewsStatus = newsArticle.NewsStatus;

                // Cập nhật bộ sưu tập Tags trong Change Tracker
                existing.Tags.Clear();
                foreach (var tag in newsArticle.Tags)
                {
                    existing.Tags.Add(tag);
                }

                // Tự động gán ngày cập nhật lúc sửa
                existing.ModifiedDate = DateTime.Now;

                _repository.UpdateNewsArticle(existing);
            }
        }

        public void DeleteNewsArticle(string id)
        {
            var article = _repository.GetNewsArticleById(id);
            if (article != null)
            {
                _repository.DeleteNewsArticle(article);
            }
        }
        public List<NewsArticle> GetNewsArticlesByPeriod(System.DateTime startDate, System.DateTime endDate)
            => _repository.GetNewsArticlesByPeriod(startDate, endDate);
    }
}