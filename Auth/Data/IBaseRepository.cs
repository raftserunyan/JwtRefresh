using Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Data
{
    public interface IBaseRepository<TEntity> where TEntity: class
    {
        public Task<TEntity> Get(int id);
        public Task<IEnumerable<TEntity>> GetAll();
        public Task<TEntity> Add(TEntity entity);
        public Task<TEntity> Update(TEntity entity);
        public void Delete(int id);
    }
}
