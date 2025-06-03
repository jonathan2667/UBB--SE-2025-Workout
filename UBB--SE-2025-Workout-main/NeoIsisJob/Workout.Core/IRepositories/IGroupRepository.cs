using ServerLibraryProject.Models;
using System.Collections.Generic;

namespace ServerLibraryProject.Interfaces

{
    public interface IGroupRepository
    {
        //void DeleteGroupById(long id);

        List<Group> GetAllGroups();

        Group GetGroupById(long id);

        List<Group> GetGroupsForUser(long userId);

        List<User> GetUsersFromGroup(long id);

        void SaveGroup(Group entity);

        //void UpdateGroup(long id, string name, string image, string description, long adminId);
    }
}