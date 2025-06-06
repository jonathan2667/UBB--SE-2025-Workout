/*namespace ServerLibraryProject.Data
{
    using Microsoft.EntityFrameworkCore;
    using ServerLibraryProject.DbRelationshipEntities;
    using ServerLibraryProject.Models;
    using Workout.Core.Models;

    public class SocialAppDbContext : DbContext
    {
        public SocialAppDbContext(DbContextOptions<SocialAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; } = default!;

        public DbSet<Comment> Comments { get; set; } = default!;

        public DbSet<UserFollower> UserFollowers { get; set; } = default!;

        public DbSet<GroupUser> GroupUsers { get; set; } = default!;

        public DbSet<Group> Groups { get; set; } = default!;

        public DbSet<Reaction> Reactions { get; set; } = default!;

        public DbSet<UserModel> Users { get; set; } = default!;


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
        }
    }
}*/
