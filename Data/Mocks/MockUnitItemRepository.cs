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

        public unitItem AddAsync(unitItem item)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteItem(int itemId)
        {
            throw new NotImplementedException();
        }

        public unitItem EditItemAsync(unitItem updatedItem)
        {
            throw new NotImplementedException();
        }

        public unitItem GetItemByIdAsync(int itemId)
        {
            throw new NotImplementedException(); 
        }

        Task<unitItem> IUnitItemRepository.AddAsync(unitItem item)
        {
            throw new NotImplementedException();
        }

        Task IUnitItemRepository.EditItemAsync(unitItem updatedItem)
        {
            throw new NotImplementedException();
        }

        Task<unitItem> IUnitItemRepository.GetItemByIdAsync(int itemId)
        {
            throw new NotImplementedException();
        }
    }
}
