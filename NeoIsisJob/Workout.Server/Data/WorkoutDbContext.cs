//using Microsoft.EntityFrameworkCore;
//using Workout.Core.Models;

//namespace Workout.Server.Data
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