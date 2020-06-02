using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Mocks
{
    public class MockCategoryRepository : ICategoryRepository
    {
        public IEnumerable<Category> Categories { get
            {
                return new List<Category>
                {
                    new Category { CategoryName = "Beverage", Description="Drink offered at the bar"},
                    new Category { CategoryName = "Food", Description="Food offered by the resturant"}

                };
            } 
        }
    }
}
