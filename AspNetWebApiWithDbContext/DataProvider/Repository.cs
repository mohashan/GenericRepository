using AspNetWebApiWithDbContext.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AspNetWebApiWithDbContext.DataProvider;


public class Repository : IRepository
{
    private readonly MyDbContext _context;

    public Repository(MyDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves data based on filter, ordering, includes, and projects it into a result type.
    /// </summary>
    /// <typeparam name="TEntity">Entity type in the DbContext.</typeparam>
    /// <typeparam name="TResult">Result projection type.</typeparam>
    /// <param name="filter">Expression to filter the entities.</param>
    /// <param name="selector">Expression to project the entity to TReturn.</param>
    /// <param name="orderBy">
    ///     Function to order the resulting query. For example:
    ///     query => query.OrderBy(x => x.Property)
    /// </param>
    /// <param name="includes">
    ///     One or more expressions for related entities to include using eager loading.
    ///     For example: include => include.NavigationProperty
    /// </param>
    /// <returns>A list of projected results.</returns>
    public async Task<List<TResult>> GetDataAsync<TEntity, TResult>(
        Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, TResult>> selector,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        params Expression<Func<TEntity, object>>[] includes)
        where TEntity : BaseEntity, new()
    {
        // Start with the entity set
        IQueryable<TEntity> query = _context.Set<TEntity>();

        // Apply include expressions if any
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        // Apply filtering if specified
        if (filter != null)
        {
            query = query.Where(filter);
        }

        // Apply ordering if specified
        if (orderBy != null)
        {
            query = orderBy(query);
        }

        // Project the final results and execute asynchronously
        return await query.Select(selector).ToListAsync();
    }

}
