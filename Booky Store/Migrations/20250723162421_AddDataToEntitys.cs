using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Booky_Store.API.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToEntitys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "authors",
                columns: new[] { "Id", "Bio", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 1, "Author of Clean Code", "authors/robert_martin.jpg", "Robert C. Martin" },
                    { 2, "Author of Sapiens and Homo Deus", "authors/harari.jpg", "Yuval Noah Harari" },
                    { 3, "Author of Harry Potter series", "authors/rowling.jpg", "J.K. Rowling" }
                });

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "Id", "ImgUrl", "Name" },
                values: new object[,]
                {
                    { 1, "programming.jpg", "Programming" },
                    { 2, "science.jpg", "Science" },
                    { 3, "history.jpg", "History" },
                    { 4, "business.jpg", "Business" }
                });

            migrationBuilder.InsertData(
                table: "publishers",
                columns: new[] { "Id", "Description", "LogoUrl", "Name" },
                values: new object[,]
                {
                    { 1, "Leading publisher of tech books", "publishers/oreilly.png", "O'Reilly Media" },
                    { 2, "Publisher of programming books", "publishers/packt.png", "Packt Publishing" },
                    { 3, "Famous general publisher", "publishers/penguin.png", "Penguin Books" }
                });

            migrationBuilder.InsertData(
                table: "books",
                columns: new[] { "Id", "CategoryId", "CoverImageUrl", "Description", "ISBN", "Price", "PublishDate", "PublisherId", "Rate", "Review", "Title" },
                values: new object[,]
                {
                    { 1, 1, "books/clean_code.jpg", "A Handbook of Agile Software Craftsmanship", "9780132350884", 45.990000000000002, new DateOnly(2008, 8, 11), 1, 4.7999999999999998, 100, "Clean Code" },
                    { 2, 3, "books/sapiens.jpg", "Exploring the history of humans from early ages to modern times.", "9780062316097", 30.5, new DateOnly(2015, 2, 10), 3, 4.7000000000000002, 200, "Sapiens: A Brief History of Humankind" },
                    { 3, 4, "books/harry_potter1.jpg", "The first book of the Harry Potter series.", "9780439708180", 25.0, new DateOnly(1997, 6, 26), 3, 4.9000000000000004, 300, "Harry Potter and the Sorcerer's Stone" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "authors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "authors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "authors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "publishers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "publishers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "publishers",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
