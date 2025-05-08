using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Workout.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeedShopData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "Supplements" },
                    { 2, "Equipment" }
                });

            migrationBuilder.InsertData(
                table: "Order",
                columns: new[] { "ID", "OrderDate", "UserID" },
                values: new object[] { 1, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ID", "CategoryID", "Color", "Description", "Name", "PhotoURL", "Price", "Size", "Stock" },
                values: new object[,]
                {
                    { 1, 1, "N/A", "High-quality whey protein for muscle building.", "Protein Powder", "https://example.com/images/protein-powder.jpg", 29.99m, "2 lb", 50 },
                    { 2, 2, "Purple", "Non-slip yoga mat for all levels.", "Yoga Mat", "https://example.com/images/yoga-mat.jpg", 19.99m, "Standard", 120 }
                });

            migrationBuilder.InsertData(
                table: "CartItem",
                columns: new[] { "ID", "ProductID", "UserID" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 1 },
                    { 3, 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "OrderItem",
                columns: new[] { "ID", "OrderID", "ProductID", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 1, 1 },
                    { 2, 1, 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "WishlistItem",
                columns: new[] { "ID", "ProductID", "UserID" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 1, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CartItem",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CartItem",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CartItem",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OrderItem",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderItem",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WishlistItem",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WishlistItem",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Order",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "ID",
                keyValue: 2);
        }
    }
}
