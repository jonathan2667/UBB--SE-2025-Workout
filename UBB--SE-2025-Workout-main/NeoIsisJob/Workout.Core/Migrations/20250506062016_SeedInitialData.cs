using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Workout.Core.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ClassTypes",
                columns: new[] { "CTID", "Name" },
                values: new object[,]
                {
                    { 1, "dance" },
                    { 2, "fight" },
                    { 3, "stretch" }
                });

            migrationBuilder.InsertData(
                table: "MuscleGroups",
                columns: new[] { "MGID", "Name" },
                values: new object[,]
                {
                    { 1, "Chest" },
                    { 2, "Legs" },
                    { 3, "Arms" },
                    { 4, "Abs" },
                    { 5, "Back" }
                });

            migrationBuilder.InsertData(
                table: "PersonalTrainers",
                columns: new[] { "PTID", "FirstName", "LastName", "WorksSince" },
                values: new object[,]
                {
                    { 1, "Zelu", "Popa", new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Rares", "Racsan", new DateTime(2024, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Mihai", "Predescu", new DateTime(2022, 5, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                column: "UID",
                values: new object[]
                {
                    1,
                    2
                });

            migrationBuilder.InsertData(
                table: "WorkoutTypes",
                columns: new[] { "WTID", "Name" },
                values: new object[,]
                {
                    { 1, "upper" },
                    { 2, "lower" }
                });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "CID", "CTID", "Description", "Name", "PTID" },
                values: new object[,]
                {
                    { 1, 1, "danceeee", "Samba", 1 },
                    { 2, 2, "Guts", "Box", 2 },
                    { 3, 2, "fightttt", "MMA", 2 },
                    { 4, 3, "relax", "Yoga", 3 }
                });

            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "EID", "Description", "Difficulty", "MGID", "Name" },
                values: new object[,]
                {
                    { 1, "a", 8, 1, "Bench Press" },
                    { 2, "a", 8, 5, "Pull Ups" },
                    { 3, "a", 6, 1, "Cable Flys" }
                });

            migrationBuilder.InsertData(
                table: "Rankings",
                columns: new[] { "MGID", "UID", "Rank" },
                values: new object[,]
                {
                    { 1, 1, 2000 },
                    { 2, 1, 7800 },
                    { 3, 1, 6700 },
                    { 4, 1, 9600 },
                    { 5, 1, 3700 }
                });

            migrationBuilder.InsertData(
                table: "Workouts",
                columns: new[] { "WID", "Description", "Name", "WTID" },
                values: new object[,]
                {
                    { 1, string.Empty, "workout1", 1 },
                    { 2, string.Empty, "workout2", 1 },
                    { 3, string.Empty, "workout3", 1 },
                    { 4, string.Empty, "workout4", 1 },
                    { 5, string.Empty, "workout5", 2 }
                });

            migrationBuilder.InsertData(
                table: "CompleteWorkouts",
                columns: new[] { "EID", "WID", "RepsPerSet", "Sets" },
                values: new object[,]
                {
                    { 1, 1, 10, 4 },
                    { 3, 1, 12, 4 },
                    { 2, 2, 8, 5 }
                });

            migrationBuilder.InsertData(
                table: "UserWorkouts",
                columns: new[] { "Date", "UID", "WID", "Completed" },
                values: new object[,]
                {
                    { new DateTime(2025, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, true },
                    { new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, true },
                    { new DateTime(2025, 3, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, false },
                    { new DateTime(2025, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, true },
                    { new DateTime(2025, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 4, false },
                    { new DateTime(2025, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, true },
                    { new DateTime(2025, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2, false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "CID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "CID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "CID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "CID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CompleteWorkouts",
                keyColumns: new[] { "EID", "WID" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "CompleteWorkouts",
                keyColumns: new[] { "EID", "WID" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "CompleteWorkouts",
                keyColumns: new[] { "EID", "WID" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "Rankings",
                keyColumns: new[] { "MGID", "UID" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "Rankings",
                keyColumns: new[] { "MGID", "UID" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "Rankings",
                keyColumns: new[] { "MGID", "UID" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "Rankings",
                keyColumns: new[] { "MGID", "UID" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "Rankings",
                keyColumns: new[] { "MGID", "UID" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "UserWorkouts",
                keyColumns: new[] { "Date", "UID", "WID" },
                keyValues: new object[] { new DateTime(2025, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1 });

            migrationBuilder.DeleteData(
                table: "UserWorkouts",
                keyColumns: new[] { "Date", "UID", "WID" },
                keyValues: new object[] { new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1 });

            migrationBuilder.DeleteData(
                table: "UserWorkouts",
                keyColumns: new[] { "Date", "UID", "WID" },
                keyValues: new object[] { new DateTime(2025, 3, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2 });

            migrationBuilder.DeleteData(
                table: "UserWorkouts",
                keyColumns: new[] { "Date", "UID", "WID" },
                keyValues: new object[] { new DateTime(2025, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3 });

            migrationBuilder.DeleteData(
                table: "UserWorkouts",
                keyColumns: new[] { "Date", "UID", "WID" },
                keyValues: new object[] { new DateTime(2025, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 4 });

            migrationBuilder.DeleteData(
                table: "UserWorkouts",
                keyColumns: new[] { "Date", "UID", "WID" },
                keyValues: new object[] { new DateTime(2025, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1 });

            migrationBuilder.DeleteData(
                table: "UserWorkouts",
                keyColumns: new[] { "Date", "UID", "WID" },
                keyValues: new object[] { new DateTime(2025, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2 });

            migrationBuilder.DeleteData(
                table: "Workouts",
                keyColumn: "WID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ClassTypes",
                keyColumn: "CTID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ClassTypes",
                keyColumn: "CTID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ClassTypes",
                keyColumn: "CTID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "EID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "EID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "EID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "MGID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "MGID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "MGID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PersonalTrainers",
                keyColumn: "PTID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PersonalTrainers",
                keyColumn: "PTID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PersonalTrainers",
                keyColumn: "PTID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WorkoutTypes",
                keyColumn: "WTID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Workouts",
                keyColumn: "WID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Workouts",
                keyColumn: "WID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Workouts",
                keyColumn: "WID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Workouts",
                keyColumn: "WID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "MGID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "MGID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "WorkoutTypes",
                keyColumn: "WTID",
                keyValue: 1);
        }
    }
}
