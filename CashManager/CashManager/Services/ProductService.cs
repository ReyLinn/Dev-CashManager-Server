using CashManager.Data;
using CashManager.Models;
using System.Linq;

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
