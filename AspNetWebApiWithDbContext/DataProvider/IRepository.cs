using AspNetWebApiWithDbContext.Domain;
using System.Linq.Expressions;

namespace AspNetWebApiWithDbContext.DataProvider;
public interface IRepository
{
    IQueryable<TResult> GetDataAsync<TEntity, TResult>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TResult>> selector, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes) where TEntity : BaseEntity, new();
}