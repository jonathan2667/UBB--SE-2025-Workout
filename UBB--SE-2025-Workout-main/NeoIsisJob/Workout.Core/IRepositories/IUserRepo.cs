using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServerLibraryProject.DbRelationshipEntities;
using Workout.Core.Models;

namespace Workout.Core.IRepositories
{
    public interface IUserRepo
    {
        Task<UserModel?> GetUserByIdAsync(int userId);

        Task<UserModel?> GetUserByUsernameAsync(string username);

        Task<int> InsertUserAsync();

        Task<int> InsertUserAsync(string username, string email, string password);

        Task<bool> DeleteUserByIdAsync(int userId);

        Task<List<UserModel>> GetAllUsersAsync();

        List<UserModel> GetUserFollowers(int id);

        List<UserModel> GetUserFollowing(int id);

        UserModel Save(UserModel entity);

        void Unfollow(int userId, int whoToUnfollowId);

        void JoinGroup(int userId, long groupId);

        void ExitGroup(int userId, long groupId);

        public void Follow(int userId, int whoToFollowId);
    }
}
