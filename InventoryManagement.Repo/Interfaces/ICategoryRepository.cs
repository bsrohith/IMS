using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Models.Entities;

namespace InventoryManagement.Repo.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Categories>> GetAllCategories();
    }
}
