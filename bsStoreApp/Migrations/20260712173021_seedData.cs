using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace bsStoreApp.Migrations
{
    /// <inheritdoc />
    public partial class seedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Price", "Title" },
                values: new object[,]
                {
                    { 1, 999m, "The Lord of the Rings" },
                    { 2, 899m, "The Hobbit" },
                    { 3, 799m, "The Silmarillion" },
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "Books", keyColumn: "Id", keyValue: 1);

            migrationBuilder.DeleteData(table: "Books", keyColumn: "Id", keyValue: 2);

            migrationBuilder.DeleteData(table: "Books", keyColumn: "Id", keyValue: 3);
        }
    }
}
