using Repositories.Models;
using System.Collections.Generic;
using System.Linq;

namespace Repositories.DAOs
{
    public class CategoryDAO
    {
        private static CategoryDAO? instance;
        private static readonly object instanceLock = new object();

        // Private constructor
        private CategoryDAO() { }

        // Singleton Instance
        public static CategoryDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CategoryDAO();
                    }
                    return instance;
                }
            }
        }

        public List<Category> GetCategories(FunewsManagementContext context)
        {
            return context.Categories.ToList();
        }

        public Category? GetCategoryById(FunewsManagementContext context, short id)
        {
            return context.Categories.SingleOrDefault(c => c.CategoryId == id);
        }

        public void AddCategory(FunewsManagementContext context, Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();
        }

        public void UpdateCategory(FunewsManagementContext context, Category category)
        {
            var tracked = context.Categories.Local.FirstOrDefault(entry => entry.CategoryId == category.CategoryId);
            if (tracked != null && tracked != category)
            {
                context.Entry(tracked).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            context.Categories.Update(category);
            context.SaveChanges();
        }

        public void DeleteCategory(FunewsManagementContext context, Category category)
        {
            context.Categories.Remove(category);
            context.SaveChanges();
        }

        // Kiểm tra xem Category có đang chứa bài báo (NewsArticle) nào không
        public bool IsCategoryUsed(FunewsManagementContext context, short categoryId)
        {
            return context.NewsArticles.Any(n => n.CategoryId == categoryId);
        }

        // Tìm kiếm Category theo tên hoặc mô tả
        public List<Category> SearchCategories(FunewsManagementContext context, string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return GetCategories(context);

            keyword = keyword.ToLower();
            // Lưu ý: Cột mô tả trong DB bị đặt tên thiếu chữ 'r' (Desciption)
            return context.Categories
                .Where(c => c.CategoryName.ToLower().Contains(keyword)
                         || c.CategoryDesciption.ToLower().Contains(keyword))
                .ToList();
        }
    }
}