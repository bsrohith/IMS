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
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierService(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }
        public async Task<IEnumerable<Suppliers>> GetAllSuppliers()
        {
            return await _supplierRepository.GetAllSuppliers();
        }

        public async Task<Suppliers> GetSuppliersById(int id)
        {
            return await _supplierRepository.GetSuppliersById(id);
        }
    }
}
