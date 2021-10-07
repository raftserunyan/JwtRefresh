using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Auth.Data
{
	public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
		protected readonly AppDbContext _context;

		public BaseRepository(AppDbContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			_context = context;
		}

		#region Get methods

		//Protected methods
		protected IQueryable<TEntity> GetAll()
		{
			return _context.Set<TEntity>().AsNoTracking();
		}
		protected IQueryable<TEntity> GetAllInclude(params Expression<Func<TEntity, object>>[] includeExpressions)
		{
			var query = GetAll();

			foreach (var include in includeExpressions)
			{
				query = query.Include(include);
			}

			return query;
		}
		protected IQueryable<TEntity> GetAllTracking()
		{
			return _context.Set<TEntity>();
		}
		protected IQueryable<TEntity> GetAllTrackingInclude(params Expression<Func<TEntity, object>>[] includeExpressions)
		{
			var query = _context.Set<TEntity>();

			foreach (var include in includeExpressions)
			{
				query.Include(include);
			}

			return query;
		}

		protected IQueryable<TResult> GetQueryableWhereSelect<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> select, params Expression<Func<TEntity, object>>[] includeProperties)
		{
			var query = GetAll().Where(predicate);

			foreach (var property in includeProperties)
			{
				query.Include(property);
			}

			return query.Select(select);
		}
		protected IQueryable<TResult> GetQueryableSelectAsync<TResult>(Expression<Func<TEntity, TResult>> select, params Expression<Func<TEntity, object>>[] includeProperties)
		{
			var query = GetAll();

			if (includeProperties?.Any() == true)
			{
				foreach (var property in includeProperties)
				{
					query.Include(property);
				}
			}

			return query.Select(select);
		}
		protected IQueryable<TResult> GetQueryableProjection<TResult>(Expression<Func<TEntity, TResult>> projection)
		{
			return GetAll().Select(projection);
		}

		//Public methods
		public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
		}
		public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
		{
			return await GetAllInclude(includeProperties).FirstOrDefaultAsync(predicate);
		}

		public async Task<IEnumerable<TEntity>> GetAllAsync()
		{
			return await GetAll().ToListAsync();
		}
		public async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeExpressions)
		{
			var query = GetAll();

			foreach (var include in includeExpressions)
			{
				query = query.Include(include);
			}

			return await query.ToListAsync();
		}

		public async Task<IEnumerable<TEntity>> GetAllTrackingAsync()
		{
			return await _context.Set<TEntity>().AsQueryable().ToListAsync();
		}
		public async Task<IEnumerable<TEntity>> GetAllTrackingAsync(params Expression<Func<TEntity, object>>[] includeExpressions)
		{
			var query = _context.Set<TEntity>().AsQueryable();

			foreach (var include in includeExpressions)
			{
				query = query.Include(include);
			}

			return await query.ToListAsync();
		}

		public async Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> whereExpression)
		{
			return await GetAll().Where(whereExpression).ToListAsync();
		}
		public async Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> whereExpression, params Expression<Func<TEntity, object>>[] includeExpressions)
		{
			return await GetAllInclude(includeExpressions).Where(whereExpression).ToListAsync();
		}
		public async Task<IEnumerable<TResult>> GetWhereSelectAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> select)
		{
			return await GetAll().Where(predicate).Select(select).ToListAsync();
		}


		#endregion

		#region Set methods
		public async virtual Task<TEntity> AddAsync(TEntity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			TryAttachEntity(entity);

			var result = await _context.Set<TEntity>().AddAsync(entity);
			return result.Entity;
		}
		public async virtual Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
		{
			if (entities == null)
			{
				throw new ArgumentNullException(nameof(entities));
			}

			foreach (var entity in entities)
			{
				TryAttachEntity(entity);
			}

			await _context.Set<TEntity>().AddRangeAsync(entities);

			return entities;
		}

		public virtual TEntity Update(TEntity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			//Attaches the given entity to the context underlying the set. 
			//That is, the entity is placed into the context in the Unchanged state, just as if it had been read from the database.
			//Note that entities that are already in the context in some other state will have their state set to Unchanged. 
			//Attach is a no-op if the entity is already in the context in the Unchanged state. 
			//See https://msdn.microsoft.com/en-us/library/mt136633%28v=vs.113%29.aspx
			TryAttachEntity(entity);

			_context.Entry(entity).State = EntityState.Modified;

			return entity;
		}
		public virtual async Task<TEntity> UpdateAsync(TEntity entity)
		{
			TryAttachEntity(entity);

			_context.Entry(entity).State = EntityState.Modified;

			return await Task.FromResult(entity);
		}
		public virtual IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entities)
		{
			if (entities == null)
			{
				throw new ArgumentNullException(nameof(entities));
			}

			foreach (var entity in entities)
			{
				TryAttachEntity(entity);

				_context.Entry(entity).State = EntityState.Modified;
			}

			return entities;
		}

		public virtual void Remove(TEntity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			TryAttachEntity(entity);

			_context.Set<TEntity>().Remove(entity);
		}
		public virtual void RemoveRange(IEnumerable<TEntity> entities)
		{
			if (entities == null)
			{
				throw new ArgumentNullException(nameof(entities));
			}

			foreach (var entity in entities)
			{
				TryAttachEntity(entity);
			}

			_context.Set<TEntity>().RemoveRange(entities);
		}
		#endregion

		public async Task LoadReferenceAsync(TEntity entity, params Expression<Func<TEntity, object>>[] referenceProperties)
		{
			foreach (var reference in referenceProperties)
			{
				await _context.Entry(entity).Reference(reference).LoadAsync();
			}
		}
		public Task<int> CountAllAsync()
		{
			return GetAll().CountAsync();
		}
		public Task<int> CountWhereAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return GetAll().CountAsync(predicate);
		}

		public async Task<bool> ExistsWhereAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return await GetAll().AnyAsync(predicate);
		}

		public async Task<bool> CommitAsync()
		{
			return await _context.SaveChangesAsync() > 0;
		}

		private void TryAttachEntity(TEntity entity)
		{
			try
			{
				_context.Set<TEntity>().Attach(entity);
			}
			catch (Exception)
			{
				// Even with .AsNoTracking, entities are attached to the context
				// If entity could not be attached to context, ignore the exception and continue with following steps
			}
		}

		//private readonly AppDbContext _context;

		//public BaseRepository(AppDbContext db)
		//{
		//    _context = db;
		//}

		//public async Task<TEntity> AddAsync(TEntity entity)
		//{
		//    var result = await _context.Set<TEntity>().AddAsync(entity);
		//    return result.Entity;
		//}
		//public void Delete(int id)
		//{
		//    _context.Remove(id);
		//}

		//public async Task<TEntity> GetAsync(int id)
		//{
		//    return await _context.Set<TEntity>().FindAsync(id);
		//}
		//public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
		//{
		//    return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
		//}
		//public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
		//{
		//    var query = _context.Set<TEntity>().AsQueryable();

		//    foreach (var include in includeProperties)
		//    {
		//        query = query.Include(include);
		//    }

		//    return await query.FirstOrDefaultAsync(predicate);
		//}

		//public async Task<IEnumerable<TEntity>> GetAllAsync()
		//{
		//    return await _context.Set<TEntity>().ToListAsync();
		//}
		//public async Task<TEntity> UpdateAsync(TEntity entity)
		//{
		//    try
		//    {
		//        _context.Set<TEntity>().Attach(entity);
		//    }
		//    catch (Exception)
		//    {

		//    }
		//    _context.Entry(entity).State = EntityState.Modified;
		//    return await Task.FromResult(entity);
		//}
	}
}
