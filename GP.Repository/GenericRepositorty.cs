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
        public async  Task AddAsync(T entity)
          => await dbContext.Set<T>().AddAsync(entity);

        public void Delete(T entity)
        
           => dbContext.Set<T>().Remove(entity);

        public async void Update(T entity)
              => dbContext.Set<T>().Update(entity);
        #region static qui
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Trip))
            {
                return (IEnumerable<T>)await dbContext.Trips.Include(T => T.FromCity).Include(T => T.ToCity).ToListAsync();

            }
            return await dbContext.Set<T>().ToListAsync();
        }



        public async Task<T> GetByIdAsyn(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
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

        public Task<T> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
