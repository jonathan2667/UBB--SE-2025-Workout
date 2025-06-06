﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NeoIsisJob.Helpers;
using NeoIsisJob.Proxy;
using Workout.Core.Models;

namespace NeoIsisJob.ViewModels
{
    public class UserViewModel
    {
        private readonly UserServiceProxy userService;

        public ObservableCollection<UserModel> Users { get; set; }
        public UserModel? SelectedUser { get; set; } // Make SelectedUser nullable

        public UserViewModel()
        {
            this.userService = new UserServiceProxy();
            // Users = new ObservableCollection<UserModel>(this.userService.GetAllUsers());
            // SelectedUser = null; // Initialize SelectedUser to null
            _ = InitializeAsync();
        }

        public async Task InitializeAsync()
        {
            var allUsers = await userService.GetAllUsersAsync();
            // foreach (var user in allUsers)
            // {
            //    Users.Add(user);
            // }
            Users = new ObservableCollection<UserModel>(allUsers);
            SelectedUser = null; // Initialize SelectedUser to null
        }

        public async void AddUser()
        {
            int newUserId = await userService.RegisterNewUserAsync();
            Users.Add(new UserModel(newUserId));
        }

        public async void DeleteUser(int userId)
        {
            if (await userService.RemoveUserAsync(userId))
            {
                var userToRemove = Users.FirstOrDefault(user => user.ID == userId);
                if (userToRemove != null)
                {
                    Users.Remove(userToRemove);
                }
            }
        }

        public async Task<UserModel?> GetUserById(int userId)
        {
            SelectedUser = await userService.GetUserAsync(userId);
            return SelectedUser;
        }

        public async void RefreshUsers()
        {
            Users.Clear();
            foreach (var user in await userService.GetAllUsersAsync())
            {
                Users.Add(user);
            }
        }
    }
}
