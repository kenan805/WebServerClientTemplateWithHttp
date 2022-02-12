using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CarsData.DAL.Repositories.Abstract
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        TEntity Find(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void Update(TEntity entity);
        int Count();
    }
}
