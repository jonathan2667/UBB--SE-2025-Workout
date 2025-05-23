using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workout.Core.Migrations
{
    /// <inheritdoc />
    public partial class SeedProductImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "ID",
                keyValue: 1,
                column: "PhotoURL",
                value: "https://m.media-amazon.com/images/I/711Lq+gNUtL._AC_UF1000,1000_QL80_.jpg");

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "ID",
                keyValue: 2,
                column: "PhotoURL",
                value: "https://i5.walmartimages.com/seo/CAP-High-Density-1-inch-Thick-Exercise-Mat-with-Carry-Strap-71-x24-x1-Purple_8c5eca06-c117-4677-8d0d-71cee6065b4c.8aeda60b6c033b50c0bc2f993eab60f5.jpeg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "ID",
                keyValue: 1,
                column: "PhotoURL",
                value: "https://example.com/images/protein-powder.jpg");

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "ID",
                keyValue: 2,
                column: "PhotoURL",
                value: "https://example.com/images/yoga-mat.jpg");
        }
    }
}
