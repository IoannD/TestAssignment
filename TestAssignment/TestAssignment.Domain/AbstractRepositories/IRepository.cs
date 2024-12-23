using System.Linq.Expressions;

namespace TestAssignment.Domain.AbstractRepositories;

public interface IRepository<T> where T : class
{
    Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate, int pageNumber,
        int pageSize, CancellationToken cancellationToken = default);

    Task<T?> GetOneAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task CreateAsync(T entity, CancellationToken cancellationToken = default);
}