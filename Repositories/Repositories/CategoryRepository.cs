using Repositories.DAOs;
using Repositories.Models;
using System.Collections.Generic;

namespace Repositories.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FunewsManagementContext _context;

        // Tiêm DbContext thông qua Dependency Injection
        public CategoryRepository(FunewsManagementContext context)
        {
            _context = context;
        }

        public List<Category> GetCategories()
            => CategoryDAO.Instance.GetCategories(_context);

        public Category? GetCategoryById(short id)
            => CategoryDAO.Instance.GetCategoryById(_context, id);

        public void AddCategory(Category category)
            => CategoryDAO.Instance.AddCategory(_context, category);

        public void UpdateCategory(Category category)
            => CategoryDAO.Instance.UpdateCategory(_context, category);

        public void DeleteCategory(Category category)
            => CategoryDAO.Instance.DeleteCategory(_context, category);

        public bool IsCategoryUsed(short categoryId)
            => CategoryDAO.Instance.IsCategoryUsed(_context, categoryId);

        public List<Category> SearchCategories(string keyword)
            => CategoryDAO.Instance.SearchCategories(_context, keyword);
    }
}