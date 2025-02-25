using InventoryManagement.Application.Interfaces;
using InventoryManagement.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Application.Services
{
    public class CatergoryService
    {
        private readonly ICategoryService _categoryService;
        public CatergoryService(ICategoryService categoryService)
        {
            _categoryService= categoryService;
        }
        public async Task<IEnumerable<Categories>> GetAllCategories()
        {
            return await _categoryService.GetAllCategories();
        }
    }
}
