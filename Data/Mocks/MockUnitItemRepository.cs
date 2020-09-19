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
        public IEnumerable<RentalAsset> unitItems { get
            {
                return new List<RentalAsset>
                {
                    new RentalAsset
                    {
                    Name = "340ml Zambezi Beer",
                    Price = 30,
                    Category = _categoryRepository.Categories.First(),
                    InStock = true
                    },

                new RentalAsset
                    {
                    Name = "340ml Castle Beer",
                    Price = 30,
                    Category = _categoryRepository.Categories.First(),
                    InStock = true
                    },
                new RentalAsset
                    {
                    Name = "340ml BlackLabel Beer",
                    Price = 30,
                    Category = _categoryRepository.Categories.First(),
                    InStock = true
                    },

                new RentalAsset
                    {
                    Name = "Cheese Burger",
                    Price = 30,
                    Category = _categoryRepository.Categories.Last(),
                    InStock = true
                    },
                new RentalAsset
                    {
                    Name = "340ml Hunters Dry Beer",
                    Price = 35,
                    Category = _categoryRepository.Categories.First(),
                    InStock = true
                    },
                new RentalAsset
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

        public RentalAsset AddAsync(RentalAsset item)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteItem(int itemId)
        {
            throw new NotImplementedException();
        }

        public RentalAsset EditItemAsync(RentalAsset updatedItem)
        {
            throw new NotImplementedException();
        }

        public RentalAsset GetItemByIdAsync(int itemId)
        {
            throw new NotImplementedException(); 
        }

        Task<RentalAsset> IUnitItemRepository.AddAsync(RentalAsset item)
        {
            throw new NotImplementedException();
        }

        Task IUnitItemRepository.EditItemAsync(RentalAsset updatedItem)
        {
            throw new NotImplementedException();
        }

        Task<RentalAsset> IUnitItemRepository.GetItemByIdAsync(int itemId)
        {
            throw new NotImplementedException();
        }
    }
}
