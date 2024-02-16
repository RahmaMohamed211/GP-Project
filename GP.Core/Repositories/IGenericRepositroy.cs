using GP.Core.Entities;
using GP.Core.Specificatios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.Core.Repositories
{
    public interface IGenericRepositroy<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(int  id);

        Task<int> AddRangeAsync(IEnumerable<T> entities);
        Task<int> AddAsync(T entity);

        void Update(T entity);

        void Delete(T entity);


        Task<IEnumerable<T>> GetAllWithSpecAsyn(ISpecification<T> spec);

        Task<T> GetByIdwithSpecAsyn(ISpecification<T> spec);

        Task<int> GetCountWithSpecAsync(ISpecification<T> spec);

        // إضافة العمليات الخاصة بالعلاقة بين المنتجات والشحنات هنا
        Task AddProductToShipmentAsync(IEnumerable<Product> products, Shipment shipment);
        Task RemoveProductFromShipmentAsync(Product product, Shipment shipment);
        Task<IEnumerable<Product>> GetProductsByShipmentIdAsync(int shipmentId);
        Task<int> SaveChangesAsync();
        Task<Product> GetByNameAsync(string Name);
    }
}
