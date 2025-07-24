using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Booky_Store.API.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToBookAuthorAndNew2CulomnInPublisher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBestSeller",
                table: "publishers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SellsCount",
                table: "publishers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "BookAuthors",
                columns: new[] { "AuthorId", "BookId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 }
                });

            migrationBuilder.UpdateData(
                table: "publishers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsBestSeller", "SellsCount" },
                values: new object[] { false, 0 });

            migrationBuilder.UpdateData(
                table: "publishers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsBestSeller", "SellsCount" },
                values: new object[] { false, 0 });

            migrationBuilder.UpdateData(
                table: "publishers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "IsBestSeller", "SellsCount" },
                values: new object[] { false, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BookAuthors",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "BookAuthors",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "BookAuthors",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DropColumn(
                name: "IsBestSeller",
                table: "publishers");

            migrationBuilder.DropColumn(
                name: "SellsCount",
                table: "publishers");
        }
    }
}
