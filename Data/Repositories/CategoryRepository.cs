using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task DeleteCategoryAsync (Category category)
        {
            _appDbContext.Categories.Remove(category);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _appDbContext.Categories.FirstOrDefaultAsync(p => p.CategoryId == id);
        }

        public async Task UpdateCategoryAsync (Category updatedCategory)
        {
            _appDbContext.Categories.Update(updatedCategory);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<Category> AddCategoryAsync (Category category)
        {
             await _appDbContext.Categories.AddAsync(category);
            await _appDbContext.SaveChangesAsync();
            return category;
        }

    }
}
