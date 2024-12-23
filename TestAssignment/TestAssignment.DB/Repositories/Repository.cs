using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TestAssignment.Domain.AbstractRepositories;

namespace TestAssignment.DB.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly Context _context;

    public Repository(Context context)
    {
        _context = context;
    }

    public async Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate,
        int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context
            .Set<T>()
            .Where(predicate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<T?> GetOneAsync(Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _context.Set<T>().AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}