using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workout.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalendarDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayNumber = table.Column<int>(type: "int", nullable: false),
                    IsCurrentDay = table.Column<bool>(type: "bit", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    GridRow = table.Column<int>(type: "int", nullable: false),
                    GridColumn = table.Column<int>(type: "int", nullable: false),
                    HasClass = table.Column<bool>(type: "bit", nullable: false),
                    HasWorkout = table.Column<bool>(type: "bit", nullable: false),
                    IsWorkoutCompleted = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarDays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassTypes",
                columns: table => new
                {
                    CTID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassTypes", x => x.CTID);
                });

            migrationBuilder.CreateTable(
                name: "MuscleGroups",
                columns: table => new
                {
                    MGID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuscleGroups", x => x.MGID);
                });

            migrationBuilder.CreateTable(
                name: "PersonalTrainers",
                columns: table => new
                {
                    PTID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WorksSince = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalTrainers", x => x.PTID);
                });

            migrationBuilder.CreateTable(
                name: "RankDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MinPoints = table.Column<int>(type: "int", nullable: false),
                    MaxPoints = table.Column<int>(type: "int", nullable: false),
                    ColorValue = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UID);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutTypes",
                columns: table => new
                {
                    WTID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutTypes", x => x.WTID);
                });

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    EID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(MAX)", nullable: false),
                    Difficulty = table.Column<int>(type: "int", nullable: false),
                    MGID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.EID);
                    table.ForeignKey(
                        name: "FK_Exercises_MuscleGroups_MGID",
                        column: x => x.MGID,
                        principalTable: "MuscleGroups",
                        principalColumn: "MGID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    CID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(MAX)", nullable: false),
                    CTID = table.Column<int>(type: "int", nullable: false),
                    PTID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.CID);
                    table.ForeignKey(
                        name: "FK_Classes_ClassTypes_CTID",
                        column: x => x.CTID,
                        principalTable: "ClassTypes",
                        principalColumn: "CTID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Classes_PersonalTrainers_PTID",
                        column: x => x.PTID,
                        principalTable: "PersonalTrainers",
                        principalColumn: "PTID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rankings",
                columns: table => new
                {
                    UID = table.Column<int>(type: "int", nullable: false),
                    MGID = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rankings", x => new { x.UID, x.MGID });
                    table.ForeignKey(
                        name: "FK_Rankings_MuscleGroups_MGID",
                        column: x => x.MGID,
                        principalTable: "MuscleGroups",
                        principalColumn: "MGID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rankings_Users_UID",
                        column: x => x.UID,
                        principalTable: "Users",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Workouts",
                columns: table => new
                {
                    WID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    WTID = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workouts", x => x.WID);
                    table.ForeignKey(
                        name: "FK_Workouts_WorkoutTypes_WTID",
                        column: x => x.WTID,
                        principalTable: "WorkoutTypes",
                        principalColumn: "WTID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClasses",
                columns: table => new
                {
                    UID = table.Column<int>(type: "int", nullable: false),
                    CID = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClasses", x => new { x.UID, x.CID, x.Date });
                    table.ForeignKey(
                        name: "FK_UserClasses_Classes_CID",
                        column: x => x.CID,
                        principalTable: "Classes",
                        principalColumn: "CID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserClasses_Users_UID",
                        column: x => x.UID,
                        principalTable: "Users",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompleteWorkouts",
                columns: table => new
                {
                    WID = table.Column<int>(type: "int", nullable: false),
                    EID = table.Column<int>(type: "int", nullable: false),
                    Sets = table.Column<int>(type: "int", nullable: false),
                    RepsPerSet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompleteWorkouts", x => new { x.WID, x.EID });
                    table.ForeignKey(
                        name: "FK_CompleteWorkouts_Exercises_EID",
                        column: x => x.EID,
                        principalTable: "Exercises",
                        principalColumn: "EID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompleteWorkouts_Workouts_WID",
                        column: x => x.WID,
                        principalTable: "Workouts",
                        principalColumn: "WID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserWorkouts",
                columns: table => new
                {
                    UID = table.Column<int>(type: "int", nullable: false),
                    WID = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Completed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWorkouts", x => new { x.UID, x.WID, x.Date });
                    table.ForeignKey(
                        name: "FK_UserWorkouts_Users_UID",
                        column: x => x.UID,
                        principalTable: "Users",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserWorkouts_Workouts_WID",
                        column: x => x.WID,
                        principalTable: "Workouts",
                        principalColumn: "WID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_CTID",
                table: "Classes",
                column: "CTID");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_PTID",
                table: "Classes",
                column: "PTID");

            migrationBuilder.CreateIndex(
                name: "IX_CompleteWorkouts_EID",
                table: "CompleteWorkouts",
                column: "EID");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_MGID",
                table: "Exercises",
                column: "MGID");

            migrationBuilder.CreateIndex(
                name: "IX_Rankings_MGID",
                table: "Rankings",
                column: "MGID");

            migrationBuilder.CreateIndex(
                name: "IX_UserClasses_CID",
                table: "UserClasses",
                column: "CID");

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkouts_WID",
                table: "UserWorkouts",
                column: "WID");

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_WTID",
                table: "Workouts",
                column: "WTID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalendarDays");

            migrationBuilder.DropTable(
                name: "CompleteWorkouts");

            migrationBuilder.DropTable(
                name: "RankDefinitions");

            migrationBuilder.DropTable(
                name: "Rankings");

            migrationBuilder.DropTable(
                name: "UserClasses");

            migrationBuilder.DropTable(
                name: "UserWorkouts");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Workouts");

            migrationBuilder.DropTable(
                name: "MuscleGroups");

            migrationBuilder.DropTable(
                name: "ClassTypes");

            migrationBuilder.DropTable(
                name: "PersonalTrainers");

            migrationBuilder.DropTable(
                name: "WorkoutTypes");
        }
    }
}
