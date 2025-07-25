using Booky_Store.API.API.Repositories.IRepositories;

namespace Booky_Store.API.Repositories.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthorRepository AuthorRepository { get; }
        IBookAuthorRepository BookAuthorRepository { get; }
        IBookRepository BookRepository { get; }
        ICartRepository CartRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IOrderItemRepository OrderItemRepository { get; }
        IOrderRepository OrderRepository { get; }
        IPublisherRepository PublisherRepository { get; }

        Task<int> CompleteAsync();
    }
}
