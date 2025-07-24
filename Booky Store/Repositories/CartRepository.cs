
using Booky_Store.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Booky_Store.API.API.Repositories.IRepositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext _context;
        public CartRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
