using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Auth.Data
{
	public interface IBaseRepository<TEntity> where TEntity: class
    {
		Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);
		Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

		Task<IEnumerable<TEntity>> GetAllAsync();
		Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeExpressions);

		Task<IEnumerable<TEntity>> GetAllTrackingAsync();
		Task<IEnumerable<TEntity>> GetAllTrackingAsync(params Expression<Func<TEntity, object>>[] includeExpressions);

		Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> whereExpression);
		Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> whereExpression, params Expression<Func<TEntity, object>>[] includeExpressions);
		Task<IEnumerable<TResult>> GetWhereSelectAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> select);

		Task<TEntity> AddAsync(TEntity entity);
		Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);

		TEntity Update(TEntity entity);
		Task<TEntity> UpdateAsync(TEntity entity);
		IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entities);

		void Remove(TEntity entity);
		void RemoveRange(IEnumerable<TEntity> entities);


		Task LoadReferenceAsync(TEntity entity, params Expression<Func<TEntity, object>>[] referenceProperties);
		Task<int> CountAllAsync();
		Task<int> CountWhereAsync(Expression<Func<TEntity, bool>> predicate);

		Task<bool> ExistsWhereAsync(Expression<Func<TEntity, bool>> predicate);

		Task<bool> CommitAsync();
		//public Task<TEntity> GetAsync(int id);
		//public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);
		//public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

		//public Task<IEnumerable<TEntity>> GetAllAsync();
		//public Task<TEntity> AddAsync(TEntity entity);
		//public Task<TEntity> UpdateAsync(TEntity entity);
		//public void Delete(int id);
	}
}
