using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Mocks
{
    public class MockUnitItemRepository : IUnitItemRepository
    {
        private readonly ICategoryRepository _categoryRepository = new MockCategoryRepository(); 
        public IEnumerable<unitItem> unitItems { get
            {
                return new List<unitItem>
                {
                    new unitItem
                    {
                    Name = "340ml Zambezi Beer",
                    Price = 30,
                    Category = _categoryRepository.Categories.First(),
                    InStock = true
                    },

                new unitItem
                    {
                    Name = "340ml Castle Beer",
                    Price = 30,
                    Category = _categoryRepository.Categories.First(),
                    InStock = true
                    },
                new unitItem
                    {
                    Name = "340ml BlackLabel Beer",
                    Price = 30,
                    Category = _categoryRepository.Categories.First(),
                    InStock = true
                    },

                new unitItem
                    {
                    Name = "Cheese Burger",
                    Price = 30,
                    Category = _categoryRepository.Categories.Last(),
                    InStock = true
                    },
                new unitItem
                    {
                    Name = "340ml Hunters Dry Beer",
                    Price = 35,
                    Category = _categoryRepository.Categories.First(),
                    InStock = true
                    },
                new unitItem
                    {
                    Name = "Buffalo Wings",
                    Price = 25,
                    Category = _categoryRepository.Categories.Last(),
                    InStock = true
                    },

                };
            }
            set { }  
        }

        public unitItem Add(unitItem item)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteItem(int itemId)
        {
            throw new NotImplementedException();
        }

        public unitItem EditItem(unitItem updatedItem)
        {
            throw new NotImplementedException();
        }

        public unitItem GetItemById(int itemId)
        {
            throw new NotImplementedException(); 
        }
    }
}
