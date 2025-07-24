
namespace Booky_Store.API.Models
{
    [PrimaryKey(nameof(BookId), nameof(AuthorId))]
    public class BookAuthor
    {
        public Book Book { get; set; } = null!;
        public int BookId { get; set; }
        public Author Author { get; set; } = null!;
        public int AuthorId { get; set; }

    }
}
