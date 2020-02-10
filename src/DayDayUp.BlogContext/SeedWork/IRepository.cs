using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DayDayUp.BlogContext.SeedWork
{
    public interface IRepository<T>
        where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
        T Insert(T entity);
        T Update(T entity);
        T Find(Expression<Func<T, bool>> predicate);

        void Delete(T entity);
        List<T> FindAll(Expression<Func<T, bool>> predicate);
        bool Any(Expression<Func<T, bool>> predicate);
        int Count(Expression<Func<T, bool>> predicate);
    }
}