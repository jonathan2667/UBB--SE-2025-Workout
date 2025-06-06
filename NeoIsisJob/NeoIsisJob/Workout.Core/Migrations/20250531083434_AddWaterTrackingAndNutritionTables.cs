using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workout.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddWaterTrackingAndNutritionTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserDailyNutrition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalCalories = table.Column<int>(type: "int", nullable: false),
                    TotalProteins = table.Column<double>(type: "float", nullable: false),
                    TotalCarbohydrates = table.Column<double>(type: "float", nullable: false),
                    TotalFats = table.Column<double>(type: "float", nullable: false),
                    MealsConsumed = table.Column<int>(type: "int", nullable: false),
                    WaterIntakeMl = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDailyNutrition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDailyNutrition_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserMealLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MealId = table.Column<int>(type: "int", nullable: false),
                    ConsumedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PortionMultiplier = table.Column<double>(type: "float", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMealLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMealLogs_Meals_MealId",
                        column: x => x.MealId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMealLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserWaterIntake",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AmountMl = table.Column<int>(type: "int", nullable: false),
                    ConsumedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWaterIntake", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWaterIntake_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDailyNutrition_UserId",
                table: "UserDailyNutrition",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMealLogs_MealId",
                table: "UserMealLogs",
                column: "MealId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMealLogs_UserId",
                table: "UserMealLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWaterIntake_UserId",
                table: "UserWaterIntake",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDailyNutrition");

            migrationBuilder.DropTable(
                name: "UserMealLogs");

            migrationBuilder.DropTable(
                name: "UserWaterIntake");
        }
    }
}
