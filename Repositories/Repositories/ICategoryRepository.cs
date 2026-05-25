using Repositories.Models;
using System.Collections.Generic;

namespace Repositories.Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetCategories();
        Category? GetCategoryById(short id);
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
        bool IsCategoryUsed(short categoryId);
        List<Category> SearchCategories(string keyword);
    }
}