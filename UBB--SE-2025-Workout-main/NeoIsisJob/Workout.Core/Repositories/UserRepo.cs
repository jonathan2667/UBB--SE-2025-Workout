using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Core.Data;
using ServerLibraryProject.DbRelationshipEntities;

namespace Workout.Core.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly WorkoutDbContext context;

        public UserRepo(WorkoutDbContext context)
        {
            this.context = context;
        }

        public async Task<UserModel?> GetUserByIdAsync(int userId)
        {
            return await context.Users
                .FirstOrDefaultAsync(u => u.ID == userId);
        }
        
        public async Task<UserModel?> GetUserByUsernameAsync(string username)
        {
            return await context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<int> InsertUserAsync()
        {
            var user = new UserModel();
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user.ID;
        }

        public async Task<int> InsertUserAsync(string username, string email, string password)
        {
            var user = new UserModel
            {
                Username = username,
                Email = email ?? "",
                Password = password
            };
            
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user.ID;
        }

        public async Task<bool> DeleteUserByIdAsync(int userId)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            context.Users.Remove(user);
            int rowsAffected = await context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            return await context.Users.ToListAsync();
        }

        public List<UserModel> GetUserFollowers(int id)

        {
            List<UserModel> userFollowers = new List<UserModel>();
            List<UserFollower> followers = context.UserFollowers
                .Where(uf => uf.UserId == id)
                .ToList();
            foreach (UserFollower userFollower in followers)
            {
                UserModel? user = context.Users.FirstOrDefault(u => u.ID == userFollower.FollowerId);
                if (user != null)
                {
                    userFollowers.Add(user);
                }
            }

            return userFollowers;
        }

        public List<UserModel> GetUserFollowing(int id)
        {
            var userFollowers = context.UserFollowers
               .Where(uf => uf.UserId == id);
            return context.Users
                .Where(u => userFollowers.Any(uf => uf.FollowerId == u.ID))
                .ToList();
        }

        public UserModel Save(UserModel entity)
        {
            try
            {
                if (context.Users.FirstOrDefault(u => u.Username.Equals(entity.Username)) != null)
                {
                    throw new Exception("User already exists");
                }

                context.Users.Add(entity);
                context.SaveChanges();
                return entity;
            }
            catch
            {
                throw new Exception("Error saving the user");
            }

        }

        public void JoinGroup(int userId, long groupId)
        {
            try
            {
                GroupUser groupUser = new GroupUser
                {
                    UserId = userId,
                    GroupId = groupId
                };
                context.GroupUsers.Add(groupUser);
                context.SaveChanges();
            }
            catch
            {
                throw new Exception("Error joining the group");
            }
        }

        public void ExitGroup(int userId, long groupId)
        {
            try
            {
                // Find the GroupUser entry that matches the user and group
                GroupUser? groupUser = context.GroupUsers
                    .FirstOrDefault(gu => gu.UserId == userId && gu.GroupId == groupId);

                if (groupUser != null)
                {
                    context.GroupUsers.Remove(groupUser);
                    context.SaveChanges();
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

        public void Unfollow(int userId, int whoToUnfollowId)
        {
            try
            {
                context.UserFollowers.Remove(new UserFollower(userId, whoToUnfollowId));
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error unfollowing user: " + ex.Message);
            }
        }

        public void Follow(int userId, int whoToFollowId)
        {
            try
            {
                context.UserFollowers.Add(new UserFollower(userId, whoToFollowId));
                context.SaveChanges();
            }
            catch
            {
                throw new Exception("Error following user"); 
            }
        }
    }
}
