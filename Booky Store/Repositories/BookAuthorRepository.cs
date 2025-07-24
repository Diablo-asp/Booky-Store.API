
using Booky_Store.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Booky_Store.API.API.Repositories.IRepositories
{
    public class BookAuthorRepository : Repository<BookAuthor>, IBookAuthorRepository
    {
        private readonly ApplicationDbContext _context;
        public BookAuthorRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
