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

        Category AddCategory(Category category);

        Category GetCategoryById(int id);

        Category UpdateCategory(Category updatedCategory);

        void DeleteCategory(Category category);

    }
}
