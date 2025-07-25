
using Booky_Store.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Booky_Store.API.API.Repositories.IRepositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        private readonly ApplicationDbContext _context;
        public BookRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllBooksWithDetailsAsync()
        {
            return await _context.books
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .ToListAsync();
        }
    }
}
