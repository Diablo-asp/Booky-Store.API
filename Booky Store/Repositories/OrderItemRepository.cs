
using Booky_Store.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Booky_Store.API.API.Repositories.IRepositories
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> CreateRangeAsync(List<OrderItem> entities)
        {
            try
            {
                _context.orderItems.AddRange(entities);
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
