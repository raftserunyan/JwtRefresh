using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Auth.Data
{
	public interface IBaseRepository<TEntity> where TEntity: class
    {
        public Task<TEntity> GetAsync(int id);
        public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);
        public Task<IEnumerable<TEntity>> GetAllAsync();
        public Task<TEntity> AddAsync(TEntity entity);
        public Task<TEntity> UpdateAsync(TEntity entity);
        public void Delete(int id);
    }
}
