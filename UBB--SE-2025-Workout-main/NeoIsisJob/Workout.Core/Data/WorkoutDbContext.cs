using Microsoft.EntityFrameworkCore;
using ServerLibraryProject.DbRelationshipEntities;
using ServerLibraryProject.Models;
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

        public DbSet<CategoryModel> Categories { get; set; }

        public DbSet<ProductModel> Products { get; set; }

        public DbSet<CartItemModel> CartItems { get; set; }

        public DbSet<WishlistItemModel> WishlistItems { get; set; }

        public DbSet<UserFavoriteMealModel> UserFavoriteMeals { get; set; }

        public DbSet<OrderModel> Orders { get; set; }

        public DbSet<OrderItemModel> OrderItems { get; set; }
        public DbSet<MealModel> Meals { get; set; }
        public DbSet<IngredientModel> Ingredients { get; set; }

        // Meal Statistics & Water Tracking DbSets
        public DbSet<UserDailyNutritionModel> UserDailyNutrition { get; set; }
        public DbSet<UserWaterIntakeModel> UserWaterIntake { get; set; }
        public DbSet<UserMealLogModel> UserMealLogs { get; set; }



        public DbSet<Post> Posts { get; set; } = default!;

        public DbSet<Comment> Comments { get; set; } = default!;

        public DbSet<UserFollower> UserFollowers { get; set; } = default!;

        public DbSet<GroupUser> GroupUsers { get; set; } = default!;

        public DbSet<Group> Groups { get; set; } = default!;

        public DbSet<Reaction> Reactions { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<GroupUser>()
               .HasKey(groupUser => new { groupUser.UserId, groupUser.GroupId });
            modelBuilder.Entity<GroupUser>(entity =>
            {
                entity.HasOne<UserModel>()
                    .WithMany()
                    .HasForeignKey(groupUser => groupUser.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne<Group>()
                    .WithMany()
                    .HasForeignKey(groupUser => groupUser.GroupId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<UserFollower>()
                .HasKey(userFollower => new { userFollower.UserId, userFollower.FollowerId });
            modelBuilder.Entity<UserFollower>(entity =>
            {
                entity.HasOne<UserModel>()
                    .WithMany()
                    .HasForeignKey(userFollower => userFollower.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne<UserModel>()
                    .WithMany()
                    .HasForeignKey(userFollower => userFollower.FollowerId)
                    .OnDelete(DeleteBehavior.NoAction);
            });


            modelBuilder.Entity<UserModel>(entity =>
            {
                //entity.ToTable("Users", tableBuilder =>
                //{ 
                //    tableBuilder.HasCheckConstraint("CK_User_Height_Positive", "[height] > 0");
                //    tableBuilder.HasCheckConstraint("CK_User_Weight_Positive", "[weight] > 0");
                //    tableBuilder.HasCheckConstraint("CK_User_Goal_Valid", "[goal] IN ('lose weight', 'mentain', 'gain muscles')");
                //});

                entity.HasIndex(user => user.Username).IsUnique();
            });


            modelBuilder.Entity<Reaction>()
                .HasKey(reaction => new { reaction.UserId, reaction.PostId });
            modelBuilder.Entity<Reaction>()
                .Property(reaction => reaction.Type)
                .HasConversion<int>();
            modelBuilder.Entity<Reaction>(entity =>
            {
                entity.HasOne<UserModel>()
                    .WithMany()
                    .HasForeignKey(reaction => reaction.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne<Post>()
                    .WithMany()
                    .HasForeignKey(reaction => reaction.PostId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Post>()
                .Property(post => post.Visibility)
                .HasConversion<int>();
            modelBuilder.Entity<Post>()
                .Property(post => post.Tag)
                .HasConversion<int>();
            modelBuilder.Entity<Post>()
                .Property(post => post.CreatedDate)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Post>()
                .Property(post => post.GroupId)
                .IsRequired(false); // GroupId is optional, so it can be null
            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasOne<UserModel>()
                    .WithMany()
                    .HasForeignKey(post => post.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne<Group>()
                    .WithMany()
                    .HasForeignKey(post => post.GroupId)
                    .OnDelete(DeleteBehavior.NoAction);
            });


            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasOne<Post>()
                    .WithMany()
                    .HasForeignKey(comment => comment.PostId)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne<UserModel>()
                    .WithMany()
                    .HasForeignKey(comment => comment.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<Comment>()
                .Property(comment => comment.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

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

            // Define shop relationships
            modelBuilder.Entity<ProductModel>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItemModel>()
                .HasOne(ci => ci.User)
                .WithMany(u => u.CartItems)
                .HasForeignKey(ci => ci.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItemModel>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WishlistItemModel>()
                .HasOne(w => w.User)
                .WithMany(u => u.WishlistItems)
                .HasForeignKey(w => w.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WishlistItemModel>()
                .HasOne(w => w.Product)
                .WithMany(p => p.WishlistItems)
                .HasForeignKey(w => w.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserFavoriteMealModel>()
                .HasOne(fm => fm.User)
                .WithMany()
                .HasForeignKey(fm => fm.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserFavoriteMealModel>()
                .HasOne(fm => fm.Meal)
                .WithMany()
                .HasForeignKey(fm => fm.MealID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderModel>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItemModel>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItemModel>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            // Define meal and ingredient relationships
            modelBuilder.Entity<MealModel>()
            .HasMany(m => m.Ingredients)
            .WithMany(i => i.Meals)
            .UsingEntity(j => j.ToTable("MealIngredients"));

            // Define meal statistics and water tracking relationships
            modelBuilder.Entity<UserDailyNutritionModel>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserWaterIntakeModel>()
                .HasOne(w => w.User)
                .WithMany()
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserMealLogModel>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserMealLogModel>()
                .HasOne(m => m.Meal)
                .WithMany()
                .HasForeignKey(m => m.MealId)
                .OnDelete(DeleteBehavior.Cascade);

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

            modelBuilder.Entity<CategoryModel>().ToTable("Category");
            modelBuilder.Entity<ProductModel>().ToTable("Product");
            modelBuilder.Entity<CartItemModel>().ToTable("CartItem");
            modelBuilder.Entity<WishlistItemModel>().ToTable("WishlistItem");
            modelBuilder.Entity<OrderModel>().ToTable("Order");
            modelBuilder.Entity<OrderItemModel>().ToTable("OrderItem");

            // Meal statistics and water tracking table mappings
            modelBuilder.Entity<UserDailyNutritionModel>().ToTable("UserDailyNutrition");
            modelBuilder.Entity<UserWaterIntakeModel>().ToTable("UserWaterIntake");
            modelBuilder.Entity<UserMealLogModel>().ToTable("UserMealLogs");

            // Seed data
            modelBuilder.Entity<MuscleGroupModel>().HasData(
                new MuscleGroupModel { MGID = 1, Name = "Chest" },
                new MuscleGroupModel { MGID = 2, Name = "Legs" },
                new MuscleGroupModel { MGID = 3, Name = "Arms" },
                new MuscleGroupModel { MGID = 4, Name = "Abs" },
                new MuscleGroupModel { MGID = 5, Name = "Back" });

            modelBuilder.Entity<ExercisesModel>().HasData(
                new ExercisesModel { EID = 1, Name = "Bench Press", Description = "a", Difficulty = 8, MGID = 1 },
                new ExercisesModel { EID = 2, Name = "Pull Ups", Description = "a", Difficulty = 8, MGID = 5 },
                new ExercisesModel { EID = 3, Name = "Cable Flys", Description = "a", Difficulty = 6, MGID = 1 });

            modelBuilder.Entity<WorkoutTypeModel>().HasData(
                new WorkoutTypeModel { WTID = 1, Name = "upper" },
                new WorkoutTypeModel { WTID = 2, Name = "lower" });

            modelBuilder.Entity<WorkoutModel>().HasData(
                new WorkoutModel { WID = 1, Name = "workout1", WTID = 1, Description = string.Empty },
                new WorkoutModel { WID = 2, Name = "workout2", WTID = 1, Description = string.Empty },
                new WorkoutModel { WID = 3, Name = "workout3", WTID = 1, Description = string.Empty },
                new WorkoutModel { WID = 4, Name = "workout4", WTID = 1, Description = string.Empty },
                new WorkoutModel { WID = 5, Name = "workout5", WTID = 2, Description = string.Empty });

            modelBuilder.Entity<CompleteWorkoutModel>().HasData(
                new CompleteWorkoutModel { WID = 1, EID = 1, Sets = 4, RepsPerSet = 10 },
                new CompleteWorkoutModel { WID = 1, EID = 3, Sets = 4, RepsPerSet = 12 },
                new CompleteWorkoutModel { WID = 2, EID = 2, Sets = 5, RepsPerSet = 8 });

            modelBuilder.Entity<UserModel>().HasData(
                new UserModel { 
                    ID = 1,
                    Username = "user1",
                    Email = "user1@example.com",
                    Password = "password1"
                },
                new UserModel { 
                    ID = 2,
                    Username = "user2",
                    Email = "user2@example.com",
                    Password = "password2"
                });

            modelBuilder.Entity<UserWorkoutModel>().HasData(
                new UserWorkoutModel { UID = 1, WID = 1, Date = new DateTime(2025, 3, 28), Completed = true },
                new UserWorkoutModel { UID = 1, WID = 2, Date = new DateTime(2025, 3, 29), Completed = false },
                new UserWorkoutModel { UID = 2, WID = 1, Date = new DateTime(2025, 3, 24), Completed = true },
                new UserWorkoutModel { UID = 2, WID = 2, Date = new DateTime(2025, 3, 25), Completed = false },
                new UserWorkoutModel { UID = 1, WID = 3, Date = new DateTime(2025, 3, 30), Completed = true },
                new UserWorkoutModel { UID = 1, WID = 1, Date = new DateTime(2025, 4, 5), Completed = true },
                new UserWorkoutModel { UID = 1, WID = 4, Date = new DateTime(2025, 4, 6), Completed = false });

            modelBuilder.Entity<RankingModel>().HasData(
                new RankingModel { UID = 1, MGID = 1, Rank = 2000 },
                new RankingModel { UID = 1, MGID = 2, Rank = 7800 },
                new RankingModel { UID = 1, MGID = 3, Rank = 6700 },
                new RankingModel { UID = 1, MGID = 4, Rank = 9600 },
                new RankingModel { UID = 1, MGID = 5, Rank = 3700 });

            modelBuilder.Entity<ClassTypeModel>().HasData(
                new ClassTypeModel { CTID = 1, Name = "dance" },
                new ClassTypeModel { CTID = 2, Name = "fight" },
                new ClassTypeModel { CTID = 3, Name = "stretch" });

            modelBuilder.Entity<PersonalTrainerModel>().HasData(
                new PersonalTrainerModel { PTID = 1, FirstName = "Zelu", LastName = "Popa", WorksSince = new DateTime(2024, 2, 10) },
                new PersonalTrainerModel { PTID = 2, FirstName = "Rares", LastName = "Racsan", WorksSince = new DateTime(2024, 3, 11) });

            modelBuilder.Entity<ClassModel>().HasData(
                new ClassModel { CID = 1, Name = "Samba", Description = "danceeee", CTID = 1, PTID = 1 },
                new ClassModel { CID = 2, Name = "Box", Description = "Guts", CTID = 2, PTID = 2 },
                new ClassModel { CID = 3, Name = "MMA", Description = "fightttt", CTID = 2, PTID = 2 },
                new ClassModel { CID = 4, Name = "Yoga", Description = "relax", CTID = 3, PTID = 3 });

            modelBuilder.Entity<UserClassModel>().HasData(new UserClassModel[] { });
            // (Optional) seed user-class associations if needed

            // Shop seed data
            modelBuilder.Entity<CategoryModel>().HasData(
                new CategoryModel { ID = 1, Name = "Supplements" },
                new CategoryModel { ID = 2, Name = "Equipment" });

            modelBuilder.Entity<ProductModel>().HasData(
                new ProductModel
                {
                    ID = 1,
                    Name = "Protein Powder",
                    Price = 29.99m,
                    Stock = 50,
                    CategoryID = 1,
                    Size = "2 lb",
                    Color = "N/A",
                    Description = "High-quality whey protein for muscle building.",
                    PhotoURL = "https://m.media-amazon.com/images/I/711Lq+gNUtL._AC_UF1000,1000_QL80_.jpg",
                },
                new ProductModel
                {
                    ID = 2,
                    Name = "Yoga Mat",
                    Price = 19.99m,
                    Stock = 120,
                    CategoryID = 2,
                    Size = "Standard",
                    Color = "Purple",
                    Description = "Non-slip yoga mat for all levels.",
                    PhotoURL = "https://i5.walmartimages.com/seo/CAP-High-Density-1-inch-Thick-Exercise-Mat-with-Carry-Strap-71-x24-x1-Purple_8c5eca06-c117-4677-8d0d-71cee6065b4c.8aeda60b6c033b50c0bc2f993eab60f5.jpeg",
                });

            modelBuilder.Entity<CartItemModel>().HasData(
                new CartItemModel { ID = 1, UserID = 1, ProductID = 1 },
                new CartItemModel { ID = 2, UserID = 1, ProductID = 2 },
                new CartItemModel { ID = 3, UserID = 2, ProductID = 2 });

            modelBuilder.Entity<WishlistItemModel>().HasData(
                new WishlistItemModel { ID = 1, UserID = 1, ProductID = 1 },
                new WishlistItemModel { ID = 2, UserID = 2, ProductID = 1 });

            modelBuilder.Entity<UserFavoriteMealModel>().HasData(
                new UserFavoriteMealModel { ID = 1, UserID = 1, MealID = 1 },
                new UserFavoriteMealModel { ID = 2, UserID = 1, MealID = 2 }
            );

            modelBuilder.Entity<OrderModel>().HasData(
                new OrderModel
                {
                    ID = 1,
                    UserID = 1,
                    OrderDate = new DateTime(2025, 5, 1)
                });

            modelBuilder.Entity<OrderItemModel>().HasData(
                new OrderItemModel { ID = 1, OrderID = 1, ProductID = 1, Quantity = 1 },
                new OrderItemModel { ID = 2, OrderID = 1, ProductID = 2, Quantity = 1 });

            modelBuilder.Entity<IngredientModel>().HasData(
       new IngredientModel { Id = 1, Name = "Lettuce" },
       new IngredientModel { Id = 2, Name = "Tomato" },
       new IngredientModel { Id = 3, Name = "Chicken" },
       new IngredientModel { Id = 4, Name = "Cheese" },
       new IngredientModel { Id = 5, Name = "Croutons" }
   );

            // Seed Meals
            modelBuilder.Entity<MealModel>().HasData(
                new MealModel
                {
                    Id = 1,
                    Name = "Chicken Salad",
                    Type = "Salad",
                    ImageUrl = "/images/chickensalad.jpg",
                    CookingLevel = "Easy",
                    CookingTimeMins = 15,
                    Directions = "Mix all ingredients and serve cold."
                },
                new MealModel
                {
                    Id = 2,
                    Name = "Veggie Delight",
                    Type = "Vegetarian",
                    ImageUrl = "/images/veggiedelight.jpg",
                    CookingLevel = "Easy",
                    CookingTimeMins = 10,
                    Directions = "Toss vegetables and enjoy fresh."
                }
            );

            // Seed many-to-many Meal-Ingredient links
            modelBuilder.Entity<MealModel>()
                .HasMany(m => m.Ingredients)
                .WithMany(i => i.Meals)
                .UsingEntity(j => j.HasData(
                    // Chicken Salad (MealId: 1)
                    new { IngredientsId = 1, MealsId = 1 },
                    new { IngredientsId = 2, MealsId = 1 },
                    new { IngredientsId = 3, MealsId = 1 },
                    new { IngredientsId = 5, MealsId = 1 },

                    // Veggie Delight (MealId: 2)
                    new { IngredientsId = 1, MealsId = 2 },
                    new { IngredientsId = 2, MealsId = 2 },
                    new { IngredientsId = 4, MealsId = 2 }
                ));

        }
    }
}