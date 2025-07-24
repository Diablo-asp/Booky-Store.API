
namespace Booky_Store.API.API.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<bool> CreateAsync(T entity);

        Task<bool> UpdateAsync(T entity);

        Task<bool> DeleteAsync(T entity);

        IQueryable<T> GetQueryable();


        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>?
               expression = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true);


        Task<T?> GetOneAsync(Expression<Func<T, bool>>?
               expression = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool tracked = true);

        Task<bool> CommitAsync();
    }
}
