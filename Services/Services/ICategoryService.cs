using Repositories.Models;
using System.Collections.Generic;

namespace Services
{
    public interface ICategoryService
    {
        List<Category> GetCategories();
        Category? GetCategoryById(short id);
        void AddCategory(Category category);
        void UpdateCategory(Category category);

        // Trả về true nếu xóa thành công, false nếu bị dính ràng buộc
        bool DeleteCategory(short id);

        List<Category> SearchCategories(string keyword);
    }
}