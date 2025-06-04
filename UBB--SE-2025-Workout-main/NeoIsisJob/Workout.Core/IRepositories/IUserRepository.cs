using ServerLibraryProject.Models;
using Workout.Core.Models;

namespace ServerLibraryProject.Interfaces
{
    public interface IUserRepository
    {
        void Follow(int userId, int whoToFollowId);

        List<UserModel> GetAll();

        UserModel GetById(int id);

        UserModel? GetByUsername(string username);

        List<UserModel> GetUserFollowers(int id);

        List<UserModel> GetUserFollowing(int id);

        UserModel Save(UserModel entity);

        void Unfollow(int userId, int whoToUnfollowId);

        void JoinGroup(int userId, long groupId);

        void ExitGroup(int userId, long groupId);
    }
}