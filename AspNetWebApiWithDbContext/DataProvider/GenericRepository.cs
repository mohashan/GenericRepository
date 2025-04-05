using AspNetWebApiWithDbContext.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace AspNetWebApiWithDbContext.DataProvider;


public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity, new()
{
    protected readonly MyDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(MyDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    /// <summary>
    /// Retrieves data based on a filter, ordering, includes, and projects each entity to a result type.
    /// </summary>
    public IQueryable<TSelect> GetDataQueryable<TSelect>(
        Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, TSelect>> selector,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        // Apply eager loading for each include expression.
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        // Filter the data if a filter is provided.
        if (filter != null)
        {
            query = query.Where(filter);
        }

        // Order the results if an ordering function is provided.
        if (orderBy != null)
        {
            query = orderBy(query);
        }

        // Project the data and execute the query asynchronously.
        return query.Select(selector);
    }

    /// <summary>
    /// Retrieves paginated data based on a filter, ordering, includes and projects it to a result type.
    /// </summary>
    public IQueryable<TSelect> GetPagedDataQueryable<TSelect>(
        Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, TSelect>> selector,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
        int page,
        int pageSize,
        params Expression<Func<TEntity, object>>[] includes)
    {
        if (page < 1)
            throw new ArgumentException("Page must be 1 or greater.", nameof(page));
        if (pageSize < 1)
            throw new ArgumentException("PageSize must be 1 or greater.", nameof(pageSize));

        IQueryable<TEntity> query = _dbSet;

        // Apply eager loading for navigation properties.
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        // Apply filtering.
        if (filter != null)
        {
            query = query.Where(filter);
        }

        // Apply ordering.
        if (orderBy != null)
        {
            query = orderBy(query);
        }
        else
        {
            query = query.OrderBy(c=>c.InsertDate);

        }

        // Perform pagination.
        query = query.Skip((page - 1) * pageSize).Take(pageSize);

        // Project the data and return the results.
        return query.Select(selector);
    }

    /// <summary>
    /// Adds a new entity to the context.
    /// </summary>
    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    /// <summary>
    /// Updates an existing entity in the context.
    /// </summary>
    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    /// <summary>
    /// Deletes an entity from the context.
    /// </summary>
    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    /// <summary>
    /// Commits all changes to the database.
    /// </summary>
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public IQueryable<TEntity> GetAll()
    {
        return _dbSet;
    }
}
