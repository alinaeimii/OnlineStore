using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Interfaces;
using System.Linq.Expressions;

namespace OnlineStore.Infrastructure.Data;
public abstract class RepositoryContext<T> : IRepository<T> where T : class
{
    private readonly DbContext dbContext;
    public RepositoryContext(DbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        dbContext.Set<T>().Add(entity);

        await SaveChangesAsync(cancellationToken);

        return entity;
    }
    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        dbContext.Set<T>().Update(entity);

        await SaveChangesAsync(cancellationToken);
    }
    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        dbContext.Set<T>().Remove(entity);

        await SaveChangesAsync(cancellationToken);
    }
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
    public async Task<T?> GetByIdAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<T>().FirstOrDefaultAsync(predicate, cancellationToken: cancellationToken);
    }
    public async Task<bool> IsExistAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<T>().AnyAsync(predicate, cancellationToken: cancellationToken);
    }

}
