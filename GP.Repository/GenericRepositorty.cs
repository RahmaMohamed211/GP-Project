using GP.Core.Entities;
using GP.Core.Repositories;
using GP.Core.Specificatios;
using GP.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GP.Repository
{
    public class GenericRepositorty<T> : IGenericRepositroy<T> where T : BaseEntity
    {
        private readonly StoreContext dbContext;

        public GenericRepositorty(StoreContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async  Task<int> AddAsync(T entity)
        {
            await dbContext.Set<T>().AddAsync(entity);
            return await dbContext.SaveChangesAsync();
        }


        public async Task<int> AddRangeAsync(IEnumerable<T> entities)
        {
            await dbContext.Set<T>().AddRangeAsync(entities);
            return await dbContext.SaveChangesAsync();
        }

        public void Delete(T entity)
        
           => dbContext.Set<T>().Remove(entity);

        public async void Update(T entity)
              => dbContext.Set<T>().Update(entity);
        #region static qui
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Shipment))
            {
                return (IEnumerable<T>)await dbContext.shipments.Include(T => T.Products).ThenInclude(T => T.Category).ToListAsync();

            }
            return await dbContext.Set<T>().ToListAsync();
        }

        public async Task<int> GetCountWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecifiaction(spec).CountAsync();
        }

      

        #endregion 
        public async Task<IEnumerable<T>> GetAllWithSpecAsyn(ISpecification<T> spec)
        {
            return await ApplySpecifiaction(spec).ToListAsync();
        }

        public async Task<T> GetByIdwithSpecAsyn(ISpecification<T> spec)
        {
            return await ApplySpecifiaction(spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecifiaction(ISpecification<T> spec)
        {
            return SpecificationEvalutor<T>.GetQuery(dbContext.Set<T>(), spec);
        }
        //public async Task<T> GetByIdAsyn(int id)
        //{
        //    return await dbContext.Set<T>().FindAsync(id);
        //}
        public async Task<T> GetByIdAsync(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        public async Task AddProductToShipmentAsync(IEnumerable<Product> products, Shipment shipment)
        {
            // تحميل الشحنة مع المنتجات المرتبطة بها من قاعدة البيانات
            var shipmentWithProducts = await dbContext.shipments
                                                    .Include(s => s.Products)
                                                    .SingleOrDefaultAsync(s => s.Id == shipment.Id);

            // إضافة المنتجات إلى الشحنة
            foreach (var product in products)
            {
                shipmentWithProducts.Products.Add(product);
            }

            // حفظ التغييرات في قاعدة البيانات
            await dbContext.SaveChangesAsync();
        }

        public Task RemoveProductFromShipmentAsync(Product product, Shipment shipment)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetProductsByShipmentIdAsync(int shipmentId)
        {
            throw new NotImplementedException();
        }
        public async Task<Product> GetByNameAsync(string Name)
        {
            // قم بتنفيذ استعلام للحصول على المنتج بناءً على اسمه
            // يعتمد ذلك على هيكل جدول قاعدة البيانات والتسميات المستخدمة
            return await dbContext.Products.FirstOrDefaultAsync(p => p.ProductName == Name);
        }
    }
}

