//using Microsoft.EntityFrameworkCore;
//using Workout.Core.Models;

//namespace Workout.Core.Data
//{
//    public class WorkoutDbContext : DbContext
//    {
//        public WorkoutDbContext(DbContextOptions<WorkoutDbContext> options) : base(options) { }

//        public DbSet<UserModel> Users { get; set; }
//        public DbSet<WorkoutModel> Workouts { get; set; }
//        public DbSet<WorkoutTypeModel> WorkoutTypes { get; set; }
//        public DbSet<ExercisesModel> Exercises { get; set; }
//        public DbSet<MuscleGroupModel> MuscleGroups { get; set; }
//        public DbSet<CompleteWorkoutModel> CompleteWorkouts { get; set; }
//        public DbSet<UserWorkoutModel> UserWorkouts { get; set; }
//        public DbSet<ClassModel> Classes { get; set; }
//        public DbSet<ClassTypeModel> ClassTypes { get; set; }
//        public DbSet<UserClassModel> UserClasses { get; set; }
//        public DbSet<PersonalTrainerModel> PersonalTrainers { get; set; }
//        public DbSet<RankingModel> Rankings { get; set; }
//        public DbSet<CalendarDayModel> CalendarDays { get; set; }
//        public DbSet<RankDefinition> RankDefinitions { get; set; }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            // Configure composite keys
//            modelBuilder.Entity<CompleteWorkoutModel>()
//                .HasKey(cw => new { cw.WID, cw.EID });

//            modelBuilder.Entity<UserWorkoutModel>()
//                .HasKey(uw => new { uw.UID, uw.WID, uw.Date });

//            modelBuilder.Entity<UserClassModel>()
//                .HasKey(uc => new { uc.UID, uc.CID, uc.Date });

//            modelBuilder.Entity<RankingModel>()
//                .HasKey(r => new { r.UID, r.MGID });

//            // Define relationships
//            modelBuilder.Entity<WorkoutModel>()
//                .HasOne(w => w.WorkoutType)
//                .WithMany()
//                .HasForeignKey(w => w.WTID);

//            modelBuilder.Entity<ExercisesModel>()
//                .HasOne(e => e.MuscleGroup)
//                .WithMany()
//                .HasForeignKey(e => e.MGID);

//            modelBuilder.Entity<CompleteWorkoutModel>()
//                .HasOne(cw => cw.Workout)
//                .WithMany(w => w.CompleteWorkouts)
//                .HasForeignKey(cw => cw.WID);

//            modelBuilder.Entity<CompleteWorkoutModel>()
//                .HasOne(cw => cw.Exercise)
//                .WithMany()
//                .HasForeignKey(cw => cw.EID);

//            modelBuilder.Entity<UserWorkoutModel>()
//                .HasOne(uw => uw.User)
//                .WithMany(u => u.UserWorkouts)
//                .HasForeignKey(uw => uw.UID);

//            modelBuilder.Entity<UserWorkoutModel>()
//                .HasOne(uw => uw.Workout)
//                .WithMany(w => w.UserWorkouts)
//                .HasForeignKey(uw => uw.WID)
//                .OnDelete(DeleteBehavior.Cascade);

//            modelBuilder.Entity<UserClassModel>()
//                .HasOne(uc => uc.User)
//                .WithMany(u => u.UserClasses)
//                .HasForeignKey(uc => uc.UID);

//            modelBuilder.Entity<UserClassModel>()
//                .HasOne(uc => uc.Class)
//                .WithMany(c => c.UserClasses)
//                .HasForeignKey(uc => uc.CID);

//            modelBuilder.Entity<ClassModel>()
//                .HasOne(c => c.ClassType)
//                .WithMany()
//                .HasForeignKey(c => c.CTID);

//            modelBuilder.Entity<ClassModel>()
//                .HasOne(c => c.PersonalTrainer)
//                .WithMany()
//                .HasForeignKey(c => c.PTID);

//            modelBuilder.Entity<RankingModel>()
//                .HasOne(r => r.User)
//                .WithMany(u => u.Rankings)
//                .HasForeignKey(r => r.UID);

//            modelBuilder.Entity<RankingModel>()
//                .HasOne(r => r.MuscleGroup)
//                .WithMany()
//                .HasForeignKey(r => r.MGID);

//            // Set all table names explicitly to match database
//            modelBuilder.Entity<UserModel>().ToTable("Users");
//            modelBuilder.Entity<WorkoutModel>().ToTable("Workouts");
//            modelBuilder.Entity<WorkoutTypeModel>().ToTable("WorkoutTypes");
//            modelBuilder.Entity<ExercisesModel>().ToTable("Exercises");
//            modelBuilder.Entity<MuscleGroupModel>().ToTable("MuscleGroups");
//            modelBuilder.Entity<CompleteWorkoutModel>().ToTable("CompleteWorkouts");
//            modelBuilder.Entity<UserWorkoutModel>().ToTable("UserWorkouts");
//            modelBuilder.Entity<ClassModel>().ToTable("Classes");
//            modelBuilder.Entity<ClassTypeModel>().ToTable("ClassTypes");
//            modelBuilder.Entity<UserClassModel>().ToTable("UserClasses");
//            modelBuilder.Entity<PersonalTrainerModel>().ToTable("PersonalTrainers");
//            modelBuilder.Entity<RankingModel>().ToTable("Rankings");
//            modelBuilder.Entity<RankDefinition>().ToTable("RankDefinitions");
//        }
//    }
//} 


using System;
using Microsoft.EntityFrameworkCore;
using Workout.Core.Models;

namespace Workout.Core.Data
{
    public class WorkoutDbContext : DbContext
    {
        public WorkoutDbContext(DbContextOptions<WorkoutDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<WorkoutModel> Workouts { get; set; }
        public DbSet<WorkoutTypeModel> WorkoutTypes { get; set; }
        public DbSet<ExercisesModel> Exercises { get; set; }
        public DbSet<MuscleGroupModel> MuscleGroups { get; set; }
        public DbSet<CompleteWorkoutModel> CompleteWorkouts { get; set; }
        public DbSet<UserWorkoutModel> UserWorkouts { get; set; }
        public DbSet<ClassModel> Classes { get; set; }
        public DbSet<ClassTypeModel> ClassTypes { get; set; }
        public DbSet<UserClassModel> UserClasses { get; set; }
        public DbSet<PersonalTrainerModel> PersonalTrainers { get; set; }
        public DbSet<RankingModel> Rankings { get; set; }
        public DbSet<CalendarDayModel> CalendarDays { get; set; }
        public DbSet<RankDefinition> RankDefinitions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure composite keys
            modelBuilder.Entity<CompleteWorkoutModel>()
                .HasKey(cw => new { cw.WID, cw.EID });
            modelBuilder.Entity<UserWorkoutModel>()
                .HasKey(uw => new { uw.UID, uw.WID, uw.Date });
            modelBuilder.Entity<UserClassModel>()
                .HasKey(uc => new { uc.UID, uc.CID, uc.Date });
            modelBuilder.Entity<RankingModel>()
                .HasKey(r => new { r.UID, r.MGID });

            // Define relationships
            modelBuilder.Entity<WorkoutModel>()
                .HasOne(w => w.WorkoutType)
                .WithMany()
                .HasForeignKey(w => w.WTID);

            modelBuilder.Entity<ExercisesModel>()
                .HasOne(e => e.MuscleGroup)
                .WithMany()
                .HasForeignKey(e => e.MGID);

            modelBuilder.Entity<CompleteWorkoutModel>()
                .HasOne(cw => cw.Workout)
                .WithMany(w => w.CompleteWorkouts)
                .HasForeignKey(cw => cw.WID);
            modelBuilder.Entity<CompleteWorkoutModel>()
                .HasOne(cw => cw.Exercise)
                .WithMany()
                .HasForeignKey(cw => cw.EID);

            modelBuilder.Entity<UserWorkoutModel>()
                .HasOne(uw => uw.User)
                .WithMany(u => u.UserWorkouts)
                .HasForeignKey(uw => uw.UID);
            modelBuilder.Entity<UserWorkoutModel>()
                .HasOne(uw => uw.Workout)
                .WithMany(w => w.UserWorkouts)
                .HasForeignKey(uw => uw.WID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserClassModel>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserClasses)
                .HasForeignKey(uc => uc.UID);
            modelBuilder.Entity<UserClassModel>()
                .HasOne(uc => uc.Class)
                .WithMany(c => c.UserClasses)
                .HasForeignKey(uc => uc.CID);

            modelBuilder.Entity<ClassModel>()
                .HasOne(c => c.ClassType)
                .WithMany()
                .HasForeignKey(c => c.CTID);
            modelBuilder.Entity<ClassModel>()
                .HasOne(c => c.PersonalTrainer)
                .WithMany()
                .HasForeignKey(c => c.PTID);

            modelBuilder.Entity<RankingModel>()
                .HasOne(r => r.User)
                .WithMany(u => u.Rankings)
                .HasForeignKey(r => r.UID);
            modelBuilder.Entity<RankingModel>()
                .HasOne(r => r.MuscleGroup)
                .WithMany()
                .HasForeignKey(r => r.MGID);

            // Table mappings
            modelBuilder.Entity<UserModel>().ToTable("Users");
            modelBuilder.Entity<WorkoutModel>().ToTable("Workouts");
            modelBuilder.Entity<WorkoutTypeModel>().ToTable("WorkoutTypes");
            modelBuilder.Entity<ExercisesModel>().ToTable("Exercises");
            modelBuilder.Entity<MuscleGroupModel>().ToTable("MuscleGroups");
            modelBuilder.Entity<CompleteWorkoutModel>().ToTable("CompleteWorkouts");
            modelBuilder.Entity<UserWorkoutModel>().ToTable("UserWorkouts");
            modelBuilder.Entity<ClassModel>().ToTable("Classes");
            modelBuilder.Entity<ClassTypeModel>().ToTable("ClassTypes");
            modelBuilder.Entity<UserClassModel>().ToTable("UserClasses");
            modelBuilder.Entity<PersonalTrainerModel>().ToTable("PersonalTrainers");
            modelBuilder.Entity<RankingModel>().ToTable("Rankings");
            modelBuilder.Entity<CalendarDayModel>().ToTable("CalendarDays");
            modelBuilder.Entity<RankDefinition>().ToTable("RankDefinitions");

            // Seed data
            modelBuilder.Entity<MuscleGroupModel>().HasData(
                new MuscleGroupModel { MGID = 1, Name = "Chest" },
                new MuscleGroupModel { MGID = 2, Name = "Legs" },
                new MuscleGroupModel { MGID = 3, Name = "Arms" },
                new MuscleGroupModel { MGID = 4, Name = "Abs" },
                new MuscleGroupModel { MGID = 5, Name = "Back" }
            );

            modelBuilder.Entity<ExercisesModel>().HasData(
                new ExercisesModel { EID = 1, Name = "Bench Press", Description = "a", Difficulty = 8, MGID = 1 },
                new ExercisesModel { EID = 2, Name = "Pull Ups", Description = "a", Difficulty = 8, MGID = 5 },
                new ExercisesModel { EID = 3, Name = "Cable Flys", Description = "a", Difficulty = 6, MGID = 1 }
            );

            modelBuilder.Entity<WorkoutTypeModel>().HasData(
                new WorkoutTypeModel { WTID = 1, Name = "upper" },
                new WorkoutTypeModel { WTID = 2, Name = "lower" }
            );

            modelBuilder.Entity<WorkoutModel>().HasData(
                new WorkoutModel { WID = 1, Name = "workout1", WTID = 1, Description = "" },
                new WorkoutModel { WID = 2, Name = "workout2", WTID = 1, Description = "" },
                new WorkoutModel { WID = 3, Name = "workout3", WTID = 1, Description = "" },
                new WorkoutModel { WID = 4, Name = "workout4", WTID = 1, Description = "" },
                new WorkoutModel { WID = 5, Name = "workout5", WTID = 2, Description = "" }
            );

            modelBuilder.Entity<CompleteWorkoutModel>().HasData(
                new CompleteWorkoutModel { WID = 1, EID = 1, Sets = 4, RepsPerSet = 10 },
                new CompleteWorkoutModel { WID = 1, EID = 3, Sets = 4, RepsPerSet = 12 },
                new CompleteWorkoutModel { WID = 2, EID = 2, Sets = 5, RepsPerSet = 8 }
            );

            modelBuilder.Entity<UserModel>().HasData(
                new UserModel { ID = 1 },
                new UserModel { ID = 2 }
            );

            modelBuilder.Entity<UserWorkoutModel>().HasData(
                new UserWorkoutModel { UID = 1, WID = 1, Date = new DateTime(2025, 3, 28), Completed = true },
                new UserWorkoutModel { UID = 1, WID = 2, Date = new DateTime(2025, 3, 29), Completed = false },
                new UserWorkoutModel { UID = 2, WID = 1, Date = new DateTime(2025, 3, 24), Completed = true },
                new UserWorkoutModel { UID = 2, WID = 2, Date = new DateTime(2025, 3, 25), Completed = false },
                new UserWorkoutModel { UID = 1, WID = 3, Date = new DateTime(2025, 3, 30), Completed = true },
                new UserWorkoutModel { UID = 1, WID = 1, Date = new DateTime(2025, 4, 5), Completed = true },
                new UserWorkoutModel { UID = 1, WID = 4, Date = new DateTime(2025, 4, 6), Completed = false }
            );

            modelBuilder.Entity<RankingModel>().HasData(
                new RankingModel { UID = 1, MGID = 1, Rank = 2000 },
                new RankingModel { UID = 1, MGID = 2, Rank = 7800 },
                new RankingModel { UID = 1, MGID = 3, Rank = 6700 },
                new RankingModel { UID = 1, MGID = 4, Rank = 9600 },
                new RankingModel { UID = 1, MGID = 5, Rank = 3700 }
            );

            modelBuilder.Entity<ClassTypeModel>().HasData(
                new ClassTypeModel { CTID = 1, Name = "dance" },
                new ClassTypeModel { CTID = 2, Name = "fight" },
                new ClassTypeModel { CTID = 3, Name = "stretch" }
            );

            modelBuilder.Entity<PersonalTrainerModel>().HasData(
                new PersonalTrainerModel { PTID = 1, FirstName = "Zelu", LastName = "Popa", WorksSince = new DateTime(2024, 2, 10) },
                new PersonalTrainerModel { PTID = 2, FirstName = "Rares", LastName = "Racsan", WorksSince = new DateTime(2024, 3, 11) },
                new PersonalTrainerModel { PTID = 3, FirstName = "Mihai", LastName = "Predescu", WorksSince = new DateTime(2022, 5, 11) }
            );

            modelBuilder.Entity<ClassModel>().HasData(
                new ClassModel { CID = 1, Name = "Samba", Description = "danceeee", CTID = 1, PTID = 1 },
                new ClassModel { CID = 2, Name = "Box", Description = "Guts", CTID = 2, PTID = 2 },
                new ClassModel { CID = 3, Name = "MMA", Description = "fightttt", CTID = 2, PTID = 2 },
                new ClassModel { CID = 4, Name = "Yoga", Description = "relax", CTID = 3, PTID = 3 }
            );

            modelBuilder.Entity<UserClassModel>().HasData(
            // (Optional) seed user-class associations if needed
            );
        }
    }
}

