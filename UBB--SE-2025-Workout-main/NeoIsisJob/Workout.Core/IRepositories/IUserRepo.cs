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

        List<UserModel> GetUserFollowers(long id);

        List<UserModel> GetUserFollowing(long id);

        UserModel Save(UserModel entity);

        void Unfollow(long userId, long whoToUnfollowId);

        void JoinGroup(long userId, long groupId);

        void ExitGroup(long userId, long groupId);

        public void Follow(long userId, long whoToFollowId);
    }
}
