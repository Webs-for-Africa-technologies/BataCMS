using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Interfaces
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> Categories { get;}

        public Task<Category> AddCategoryAsync(Category category);

        Task<Category> GetCategoryByIdAsync(int id);

        Task UpdateCategoryAsync(Category updatedCategory);

        public Task DeleteCategoryAsync (Category category);

    }
}
