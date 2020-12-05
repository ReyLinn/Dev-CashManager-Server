using CashManager.Data;
using CashManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashManager.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService (ApplicationDbContext context)
        {
            _context = context;
        }

        public Product GetProductByReference(string reference)
        {
            return _context.Products.FirstOrDefault(p => p.Reference == reference);
        }
    }
}
