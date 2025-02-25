using InventoryManagement.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Application.Interfaces
{
    public interface ISupplierService
    {
        Task<List<Suppliers>> GetAllSuppliers();
        Task<Suppliers> GetSuppliersById(int id);
    }
}
