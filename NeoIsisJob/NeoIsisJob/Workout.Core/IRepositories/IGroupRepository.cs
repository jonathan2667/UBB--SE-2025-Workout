using System.Collections.Generic;
using Workout.Core.Models;

namespace Workout.Core.IRepositories

{
    public interface IGroupRepository
    {
        //void DeleteGroupById(long id);

        List<Group> GetAllGroups();

        Group GetGroupById(long id);

        List<Group> GetGroupsForUser(int userId);

        List<UserModel> GetUsersFromGroup(long id);

        void SaveGroup(Group entity);

        //void UpdateGroup(long id, string name, string image, string description, long adminId);
    }
}