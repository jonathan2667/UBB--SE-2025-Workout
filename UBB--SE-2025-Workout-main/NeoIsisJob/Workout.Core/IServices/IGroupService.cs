using ServerLibraryProject.Models;

namespace ServerLibraryProject.Interfaces
{
    public interface IGroupService
    {
        Group GetGroupById(long id);
        List<Group> GetUserGroups(long userId);
        List<User> GetUsersFromGroup(long groupId);
        Group AddGroup(string name, string desc);
        //void DeleteGroup(long groupId);
        //void UpdateGroup(long id, string name, string desc, string image, long adminId);
        List<Group> GetAllGroups();
    }
} 