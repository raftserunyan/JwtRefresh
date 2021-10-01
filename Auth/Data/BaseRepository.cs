using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Data
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly AppContext _context;
        public BaseRepository(AppContext db)
        {
            _context = db;
        }
        public async Task<TEntity> Add(TEntity entity)
        {
            var result = await _context.Set<TEntity>().AddAsync(entity);
            return result.Entity;
        }

        public void Delete(int id)
        {
            _context.Remove(id);
        }

        public async Task<TEntity> Get(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Attach(entity);
            }
            catch (Exception)
            {

            }
            _context.Entry(entity).State = EntityState.Modified;
            return await Task.FromResult(entity);
        }
    }
}
