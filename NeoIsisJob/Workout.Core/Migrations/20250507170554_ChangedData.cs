using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workout.Core.Migrations
{
    /// <inheritdoc />
    public partial class ChangedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PersonalTrainers",
                keyColumn: "PTID",
                keyValue: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PersonalTrainers",
                columns: new[] { "PTID", "FirstName", "LastName", "WorksSince" },
                values: new object[] { 3, "Mihai", "Predescu", new DateTime(2022, 5, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
