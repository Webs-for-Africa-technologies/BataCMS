using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _appDbContext;
        public CategoryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Category> Categories => _appDbContext.Categories;

        public void DeleteCategory(Category category)
        {
            _appDbContext.Categories.Remove(category);
            _appDbContext.SaveChanges();
        }

        public Category GetCategoryById(int id)
        {
            return _appDbContext.Categories.FirstOrDefault(p => p.CategoryId == id);
        }

        public Category UpdateCategory(Category updatedCategory)
        {
            _appDbContext.SaveChanges();
            return updatedCategory;
        }

        Category ICategoryRepository.AddCategory(Category category)
        {
            _appDbContext.Categories.AddAsync(category);
            _appDbContext.SaveChanges();
            return category;
        }

    }
}
