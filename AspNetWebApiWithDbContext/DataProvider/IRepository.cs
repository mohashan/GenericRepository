using AspNetWebApiWithDbContext.Domain;
using System.Linq.Expressions;

namespace AspNetWebApiWithDbContext.DataProvider;
public interface IRepository<TEntity> where TEntity : BaseEntity, new()
{
    Task AddAsync(TEntity entity);
    void Delete(TEntity entity);
    IQueryable<TSelect> GetDataAsync<TSelect>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TSelect>> selector, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, params Expression<Func<TEntity, object>>[] includes);
    IQueryable<TSelect> GetPagedDataAsync<TSelect>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TSelect>> selector, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, int page, int pageSize, params Expression<Func<TEntity, object>>[] includes);
    Task SaveAsync();
    void Update(TEntity entity);
}