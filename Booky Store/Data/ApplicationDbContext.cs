
using Microsoft.EntityFrameworkCore;

namespace Booky_Store.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> books { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<Author> authors { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderItem> orderItems { get; set; }
        public DbSet<Publisher> publishers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer("Data Source=.;DataBase=Booky_Store;Integrated Security=True;" +
                    "Trust Server Certificate=True");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BookAuthor>()
            .HasKey(ba => new { ba.BookId, ba.AuthorId });

            builder.Entity<BookAuthor>()
                .HasOne(ba => ba.Book)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(ba => ba.BookId);

            builder.Entity<BookAuthor>()
                .HasOne(ba => ba.Author)
                .WithMany()
                .HasForeignKey(ba => ba.AuthorId);


            // Seed Categories
            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Programming", ImgUrl = "programming.jpg" },
                new Category { Id = 2, Name = "Science", ImgUrl = "science.jpg" },
                new Category { Id = 3, Name = "History", ImgUrl = "history.jpg" },
                new Category { Id = 4, Name = "Business", ImgUrl = "business.jpg" }
            );

            // Seed Publishers
            builder.Entity<Publisher>().HasData(
                new Publisher { Id = 1, Name = "O'Reilly Media", Description = "Leading publisher of tech books", LogoUrl = "publishers/oreilly.png" },
                new Publisher { Id = 2, Name = "Packt Publishing", Description = "Publisher of programming books", LogoUrl = "publishers/packt.png" },
                new Publisher { Id = 3, Name = "Penguin Books", Description = "Famous general publisher", LogoUrl = "publishers/penguin.png" }
            );

            // Seed Authors
            builder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "Robert C. Martin", Bio = "Author of Clean Code", ImageUrl = "authors/robert_martin.jpg" },
                new Author { Id = 2, Name = "Yuval Noah Harari", Bio = "Author of Sapiens and Homo Deus", ImageUrl = "authors/harari.jpg" },
                new Author { Id = 3, Name = "J.K. Rowling", Bio = "Author of Harry Potter series", ImageUrl = "authors/rowling.jpg" }
            );

            // seed Books
            builder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Title = "Clean Code",
                    Description = "A Handbook of Agile Software Craftsmanship",
                    Price = 45.99,
                    Review = 100,
                    Rate = 4.8,
                    ISBN = "9780132350884",
                    PublishDate = new DateOnly(2008, 8, 11),
                    CoverImageUrl = "books/clean_code.jpg",
                    CategoryId = 1,  // Programming
                    PublisherId = 1  // O'Reilly Media
                },
                new Book
                {
                    Id = 2,
                    Title = "Sapiens: A Brief History of Humankind",
                    Description = "Exploring the history of humans from early ages to modern times.",
                    Price = 30.50,
                    Review = 200,
                    Rate = 4.7,
                    ISBN = "9780062316097",
                    PublishDate = new DateOnly(2015, 2, 10),
                    CoverImageUrl = "books/sapiens.jpg",
                    CategoryId = 3,  // History
                    PublisherId = 3  // Penguin Books
                },
                new Book
                {
                    Id = 3,
                    Title = "Harry Potter and the Sorcerer's Stone",
                    Description = "The first book of the Harry Potter series.",
                    Price = 25.00,
                    Review = 300,
                    Rate = 4.9,
                    ISBN = "9780439708180",
                    PublishDate = new DateOnly(1997, 6, 26),
                    CoverImageUrl = "books/harry_potter1.jpg",
                    CategoryId = 4,  // Novels
                    PublisherId = 3  // Penguin Books
                }
            );

            // ========== BookAuthor ==========
            builder.Entity<BookAuthor>().HasData(
                new BookAuthor { BookId = 1, AuthorId = 1 }, // Clean Code -> Robert C. Martin
                new BookAuthor { BookId = 2, AuthorId = 2 }, // Sapiens -> Yuval Noah Harari
                new BookAuthor { BookId = 3, AuthorId = 3 }  // Harry Potter -> J.K. Rowling
            );
        }
    }
}
