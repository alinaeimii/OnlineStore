using OnlineStore.Domain.Interfaces;
using OnlineStore.Infrastructure.Data;

namespace OnlineStore.Infrastructure.Repositories;
public class Repository<T> : RepositoryContext<T>, IRepository<T> where T : class
{
    public Repository(AppDbContext context) : base(context)
    {
    }
}

