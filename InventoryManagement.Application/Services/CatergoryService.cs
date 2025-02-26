using InventoryManagement.Application.Interfaces;
using InventoryManagement.Models.Entities;
using InventoryManagement.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Application.Services
{
    public class CatergoryService:ICategoryService
    {
        private readonly ICategoryRepository _categoryreporitory;
        public CatergoryService(ICategoryRepository categoryrepository)
        {
            _categoryreporitory= categoryrepository;
        }

        public async Task CreateCategory(Categories categories)
        {
           await _categoryreporitory.CreateCategory(categories);
        }

        public async Task<List<Categories>> GetAllCategories()
        {
            return (List<Categories>)await _categoryreporitory.GetAllCategories();
        }
    }
}
