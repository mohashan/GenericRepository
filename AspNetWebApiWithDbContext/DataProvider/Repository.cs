using AspNetWebApiWithDbContext.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AspNetWebApiWithDbContext.DataProvider;


public class Repository : IRepository
{
    protected readonly MyDbContext _context;

    public Repository(MyDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves data based on filter, ordering, and includes, projecting each entity to a desired result type.
    /// </summary>
    public IQueryable<TResult> GetDataAsync<TEntity, TResult>(
        Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, TResult>> selector,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        params Expression<Func<TEntity, object>>[] includes)
        where TEntity : BaseEntity, new()
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();

        // Apply eager loading for navigation properties if any
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        // Apply filtering if provided
        if (filter != null)
        {
            query = query.Where(filter);
        }

        // Apply ordering if provided
        if (orderBy != null)
        {
            query = orderBy(query);
        }

        // Return the projected results asynchronously
        return query.Select(selector);
    }

    /// <summary>
    /// Retrieves paginated data based on filter, ordering, includes, and projects it to a result type.
    /// </summary>
    public IQueryable<TResult> GetPagedDataAsync<TEntity, TResult>(
        Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, TResult>> selector,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
        int page,
        int pageSize,
        params Expression<Func<TEntity, object>>[] includes)
        where TEntity : BaseEntity, new()
    {
        if (page < 1)
            throw new ArgumentException("Page must be 1 or greater.", nameof(page));
        if (pageSize < 1)
            throw new ArgumentException("PageSize must be 1 or greater.", nameof(pageSize));

        IQueryable<TEntity> query = _context.Set<TEntity>();

        // Apply eager loading for each include expression
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        // Filter the data if a filter is provided
        if (filter != null)
        {
            query = query.Where(filter);
        }

        // Order the query results
        if (orderBy != null)
        {
            query = orderBy(query);
        }

        // Apply pagination: Skip (page - 1) * pageSize and take "pageSize" results
        query = query.Skip((page - 1) * pageSize).Take(pageSize);

        // Project the data and return asynchronously
        return query.Select(selector);
    }

    /// <summary>
    /// Adds a new entity to the context.
    /// </summary>
    public async Task AddAsync<TEntity>(TEntity entity) where TEntity : BaseEntity, new()
    {
        await _context.Set<TEntity>().AddAsync(entity);
    }

    /// <summary>
    /// Marks an entity as modified so that changes are saved.
    /// </summary>
    public void Update<TEntity>(TEntity entity) where TEntity : BaseEntity, new()
    {
        _context.Set<TEntity>().Update(entity);
    }

    /// <summary>
    /// Removes an entity from the context.
    /// </summary>
    public void Delete<TEntity>(TEntity entity) where TEntity : BaseEntity, new()
    {
        _context.Set<TEntity>().Remove(entity);
    }

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
