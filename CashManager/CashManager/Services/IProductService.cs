using CashManager.Models;

namespace CashManager.Services
{
    public interface IProductService
    {
        public Product GetProductByReference(string reference);
    }
}
