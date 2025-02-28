using InventoryManagement.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Repo.Interfaces
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<Suppliers>> GetAllSuppliers();
        Task<Suppliers> GetSuppliersById(int id);

        Task<Suppliers> GetSupplierByUserIdAsync(int userId);
    }
}
