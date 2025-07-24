
using Booky_Store.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Booky_Store.API.API.Repositories.IRepositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private ApplicationDbContext _context;
        private DbSet<T> _db { get; set; }
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _db = _context.Set<T>();
        }

        public async Task<bool> CreateAsync(T entity)
        {
            try
            {
                await _db.AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ex: {ex}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                _db.Update(entity);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ex: {ex}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            try
            {
                _db.Remove(entity);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ex: {ex}");
                return false;
            }
        }

        public IQueryable<T> GetQueryable()
        {
            return _db.AsQueryable();
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>?
            expression = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true)
        {
            IQueryable<T> entities = _db;

            if (expression != null)
            {
                entities = entities.Where(expression);
            }

            if (includes != null)
            {
                foreach (var item in includes)
                {
                    entities = entities.Include(item);
                }
            }

            if (!tracked)
            {
                entities = entities.AsNoTracking();
            }
            return (await entities.ToListAsync());
        }

        public async Task<T?> GetOneAsync(Expression<Func<T, bool>>?
            expression = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool tracked = true)
        {
            IQueryable<T> query = _context.Set<T>();

            if (!tracked)
                query = query.AsNoTracking();

            if (include != null)
                query = include(query);

            if (expression != null)
                query = query.Where(expression);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ex: {ex}");
                return false;
            }
        }
    }
}
