using ServerLibraryProject.Models;

namespace ServerLibraryProject.Interfaces
{
    public interface IUserRepository
    {
        void Follow(long userId, long whoToFollowId);
        List<User> GetAll();
        User GetById(long id);
        User? GetByUsername(string username);
        List<User> GetUserFollowers(long id);
        List<User> GetUserFollowing(long id);
        User Save(User entity);
        void Unfollow(long userId, long whoToUnfollowId);
        void JoinGroup(long userId, long groupId);

        void ExitGroup(long userId, long groupId);
    }
}