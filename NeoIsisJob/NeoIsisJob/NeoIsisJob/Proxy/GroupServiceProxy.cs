
namespace DesktopProject.Proxies
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Json;
    using Workout.Core.IServices;
    using Workout.Core.Models;

    public class GroupServiceProxy : IGroupService
    {
        private readonly HttpClient httpClient;

        public GroupServiceProxy()
        {
            this.httpClient = new HttpClient();

            this.httpClient.BaseAddress = new Uri("http://localhost:5261/api/groups/");
        }

        public Group GetGroupById(long id)
        {

            var response = this.httpClient.GetAsync($"{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<Group>().Result;
            }


            throw new Exception($"Failed to get group: {response.StatusCode}");
        }

        public List<Group> GetUserGroups(int userId)
        {
            var response = this.httpClient.GetAsync($"{userId}/groups").Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<List<Group>>().Result;
            }
            throw new Exception($"Failed to get groups: {response.StatusCode}");
        }

        public List<UserModel> GetUsersFromGroup(long groupId)
        {

            var response = this.httpClient.GetAsync($"{groupId}/users").Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<List<UserModel>>().Result;
            }

            throw new Exception($"Failed to get users from group: {response.StatusCode}");
        }

        public Group AddGroup(string name, string desc)
        {
            var group = new Group
            {
                Name = name,
                Description = desc,
            };

            var response = this.httpClient.PostAsJsonAsync(string.Empty, group).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<Group>().Result;
            }
            throw new Exception($"Failed to add group: {response.StatusCode}");
        }

        //public void DeleteGroup(long groupId)
        //{
        //    var response = this.httpClient.DeleteAsync($"{groupId}").Result;
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        throw new Exception($"Failed to delete group: {response.StatusCode}");
        //    }
        //}

        //public void UpdateGroup(long id, string name, string desc, string image, long adminId)
        //{
        //    var group = new Group
        //    {
        //        Name = name,
        //        Description = desc,
        //        Image = image,
        //        AdminId = adminId,
        //    };

        //    var response = this.httpClient.PutAsJsonAsync($"{id}", group).Result;
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        throw new Exception($"Failed to update group: {response.StatusCode}");
        //    }
        //}

        public List<Group> GetAllGroups()
        {
            var response = this.httpClient.GetAsync(string.Empty).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<List<Group>>().Result;
            }


            throw new Exception($"Failed to get all groups: {response.StatusCode}");
        }
    }
}
