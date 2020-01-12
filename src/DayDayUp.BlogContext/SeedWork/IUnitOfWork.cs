using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DayDayUp.BlogContext.SeedWork
{
    public interface IUnitOfWork : IDisposable
    {
        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}