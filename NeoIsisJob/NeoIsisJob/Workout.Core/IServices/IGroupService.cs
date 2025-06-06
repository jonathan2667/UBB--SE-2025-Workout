using Workout.Core.Models;

namespace Workout.Core.IServices
{
    public interface IGroupService
    {
        Group GetGroupById(long id);

        List<Group> GetUserGroups(int userId);

        List<UserModel> GetUsersFromGroup(long groupId);

        Group AddGroup(string name, string desc);

        // void DeleteGroup(long groupId);

        // void UpdateGroup(long id, string name, string desc, string image, long adminId);
        List<Group> GetAllGroups();
    }
}