using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workout.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddNutritionalFieldsToMeal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Calories",
                table: "Meals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Carbohydrates",
                table: "Meals",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Fats",
                table: "Meals",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Proteins",
                table: "Meals",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Calories", "Carbohydrates", "Fats", "Proteins" },
                values: new object[] { 450, 35.5, 18.2, 32.0 }); // e.g., Chicken Salad

            migrationBuilder.UpdateData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Calories", "Carbohydrates", "Fats", "Proteins" },
                values: new object[] { 620, 52.0, 24.7, 40.3 }); // e.g., Grilled Cheese with Chicken

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Calories",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "Carbohydrates",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "Fats",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "Proteins",
                table: "Meals");
        }
    }
}
