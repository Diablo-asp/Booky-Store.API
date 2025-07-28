
namespace Booky_Store.API.API.Repositories.IRepositories
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        Task<bool> CreateRangeAsync(List<OrderItem> entities);
    }
}
