using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DayDayUp.BlogContext.SeedWork;

namespace DayDayUp.BlogContext.Repositories
{
    public class RepositoryBase<T> : IRepository<T>
        where T : class, IAggregateRoot
    {
        public RepositoryBase(BlogDbContext dbContext)
        {
            DbContext = dbContext;
        }

        protected BlogDbContext DbContext { get; set; }

        public IUnitOfWork UnitOfWork => DbContext;

        public T Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            return DbContext.Add(entity).Entity;
        }

        public T Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            DbContext.Attach(entity);
            return DbContext.Update(entity).Entity;
        }

        public T Find(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>().FirstOrDefault(predicate);
        }

        public List<T> FindAll(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>().Where(predicate).ToList();
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>().Any(predicate);
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>().Count(predicate);
        }

        public void Delete(T entity)
        {
            DbContext.Remove(entity);
        }
    }

}