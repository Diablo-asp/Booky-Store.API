using Booky_Store.API.API.Repositories.IRepositories;
using Booky_Store.API.Repositories.IRepositories;

namespace Booky_Store.API.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context, IAuthorRepository authorRepository,IBookRepository bookRepository,
            IBookAuthorRepository bookAuthorRepository,ICartRepository cartRepository,
            ICategoryRepository categoryRepository,IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository, IPublisherRepository PublisherRepository)
        {
            _context = context;
            AuthorRepository = authorRepository;
            BookRepository = bookRepository;
            BookAuthorRepository = bookAuthorRepository;
            CartRepository = cartRepository;
            CategoryRepository = categoryRepository;
            OrderRepository = orderRepository;
            OrderItemRepository = orderItemRepository;
            this.PublisherRepository = PublisherRepository;
        }

        public IAuthorRepository AuthorRepository { get; }
        public IBookRepository BookRepository { get; }
        public IBookAuthorRepository BookAuthorRepository { get; }
        public ICartRepository CartRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IOrderItemRepository OrderItemRepository { get; }
        public IPublisherRepository PublisherRepository { get; }

        public void Dispose()
        {
        }
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
