using Repositories.Models;
using Repositories.Repositories;
using System.Collections.Generic;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        // Tiêm Repository vào Service
        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public List<Category> GetCategories()
            => _repository.GetCategories();

        public Category? GetCategoryById(short id)
            => _repository.GetCategoryById(id);

        public void AddCategory(Category category)
            => _repository.AddCategory(category);

        public void UpdateCategory(Category category)
            => _repository.UpdateCategory(category);

        // Logic nghiệp vụ xử lý việc Xóa
        public bool DeleteCategory(short id)
        {
            // Kiểm tra xem danh mục có bài báo nào không
            if (_repository.IsCategoryUsed(id))
            {
                return false; // Không cho phép xóa, trả về false
            }

            var category = _repository.GetCategoryById(id);
            if (category != null)
            {
                _repository.DeleteCategory(category);
                return true; // Xóa thành công
            }

            return false;
        }

        public List<Category> SearchCategories(string keyword)
            => _repository.SearchCategories(keyword);
    }
}