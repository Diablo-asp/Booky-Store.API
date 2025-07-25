
namespace Booky_Store.API.API.Repositories.IRepositories
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<IEnumerable<Book>> GetAllBooksWithDetailsAsync();
    }
}
