namespace ServerLibraryProject.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using ServerLibraryProject.Data;
    using ServerLibraryProject.DbRelationshipEntities;
    using ServerLibraryProject.Interfaces;
    using ServerLibraryProject.Models;

    /// <summary>
    /// Repository for managing user-related operations in the database.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly SocialAppDbContext dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The database context to be used.</param>
        public UserRepository(SocialAppDbContext context)
        {
            dbContext = context;
        }

        public void JoinGroup(long userId, long groupId)
        {
            try
            {
                GroupUser groupUser = new GroupUser
                {
                    UserId = userId,
                    GroupId = groupId
                };
                dbContext.GroupUsers.Add(groupUser);
                dbContext.SaveChanges();
            }
            catch
            {
                throw new Exception("Error joining the group");
            }
        }

        public void ExitGroup(long userId, long groupId)
        {
            try
            {
                // Find the GroupUser entry that matches the user and group
                GroupUser? groupUser = dbContext.GroupUsers
                    .FirstOrDefault(gu => gu.UserId == userId && gu.GroupId == groupId);

                if (groupUser != null)
                {
                    dbContext.GroupUsers.Remove(groupUser);
                    dbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("User is not a member of the group");
                }
            }
            catch
            {
                throw new Exception("Error exiting the group");
            }
        }

        /// <summary>
        /// Deletes a user by ID from the database.
        /// </summary>
        /// <param name="id">The id of the user that has to be deleted</param>
        //public void DeleteById(long id)
        //{
        //    User? user = dbContext.Users.FirstOrDefault(u => u.Id == id);
        //    if (user != null)
        //    {
        //        dbContext.Users.Remove(user);
        //        dbContext.SaveChanges();
        //    }
        //}

        public void Follow(long userId, long whoToFollowId)
        {
            try
            {
                dbContext.UserFollowers.Add(new UserFollower(userId, whoToFollowId));
                dbContext.SaveChanges();
            }
            catch { throw new Exception("Error following user"); }

        }
        public void Unfollow(long userId, long whoToUnfollowId)
        {
            try
            {       
                    dbContext.UserFollowers.Remove(new UserFollower(userId, whoToUnfollowId));
                    dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error unfollowing user: " + ex.Message);

            }
        }
        public List<User> GetAll()
        {
            return dbContext.Users.ToList();
        }


        public User GetById(long id)
        {
            try
            {
                return dbContext.Users.First(u => u.Id == id);

            }
            catch { throw new Exception("User not found"); }
        }

        public User GetByUsername(string username)
        {
            try
            {
                return dbContext.Users.FirstOrDefault(u => u.Username == username);

            }catch
            {
                throw new Exception("Error retrieving user by username");
            }
        }

        public List<User> GetUserFollowers(long id)

        {
            List<User> userFollowers = new List<User>();
            List<UserFollower> followers = dbContext.UserFollowers
                .Where(uf => uf.UserId == id)
                .ToList();
            foreach (UserFollower userFollower in followers)
            {
                User? user = dbContext.Users.FirstOrDefault(u => u.Id == userFollower.FollowerId);
                if (user != null)
                {
                    userFollowers.Add(user);
                }
            }

            return userFollowers;
        }

        public List<User> GetUserFollowing(long id)
        {
            var userFollowers = dbContext.UserFollowers
               .Where(uf => uf.UserId == id);
            return dbContext.Users
                .Where(u => userFollowers.Any(uf => uf.FollowerId == u.Id))
                .ToList();
        }


        public User Save(User entity)
        {
            try
            {
                if (dbContext.Users.FirstOrDefault(u => u.Username.Equals(entity.Username)) != null)
                {
                    throw new Exception("User already exists");
                }

                dbContext.Users.Add(entity);
                dbContext.SaveChanges();
                return entity;
            }
            catch
            {
                throw new Exception("Error saving the user");
            }

        }

        

        //public void UpdateById(long id, string username, string email, string hashPassword, string image)
        //{
        //    User? user = dbContext.Users.FirstOrDefault(u => u.Id == id);
        //    if (user != null)
        //    {
        //        user.Username = username;
        //        user.Password = hashPassword;
        //        dbContext.SaveChanges();
        //    }
        //}
    }
}