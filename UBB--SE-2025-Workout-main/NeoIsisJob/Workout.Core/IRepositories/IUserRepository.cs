using ServerLibraryProject.Models;
using Workout.Core.Models;

namespace ServerLibraryProject.Interfaces
{
    public interface IUserRepository
    {
        void Follow(long userId, long whoToFollowId);

        List<UserModel> GetAll();

        UserModel GetById(long id);

        UserModel? GetByUsername(string username);

        List<UserModel> GetUserFollowers(long id);

        List<UserModel> GetUserFollowing(long id);

        UserModel Save(UserModel entity);

        void Unfollow(long userId, long whoToUnfollowId);

        void JoinGroup(long userId, long groupId);

        void ExitGroup(long userId, long groupId);
    }
}