
using Booky_Store.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Booky_Store.API.API.Repositories.IRepositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
