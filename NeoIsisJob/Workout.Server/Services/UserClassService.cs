using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.Models;
using Workout.Server.Repositories;
using Workout.Core.IServices;
using Workout.Core.IRepositories;

namespace Workout.Server.Services
{
    public class UserClassService : IUserClassService 
    {
        private readonly IUserClassRepo userClassRepository;

        public UserClassService()
        {
            this.userClassRepository = new UserClassRepo();
        }

        public async Task<List<UserClassModel>> GetAllUserClassesAsync()
        {
            return await userClassRepository.GetAllUserClassModelAsync();
        }

        public async Task<UserClassModel> GetUserClassByIdAsync(int userId, int classId, DateTime date)
        {
            return await userClassRepository.GetUserClassModelByIdAsync(userId, classId, date);
        }

        public async Task AddUserClassAsync(UserClassModel userClassModel)
        {
            //if (userClassModel == null)
            //    throw new ArgumentNullException(nameof(userClassModel));

            await userClassRepository.AddUserClassModelAsync(userClassModel);
        }

        public async Task DeleteUserClassAsync(int userId, int classId, DateTime date)
        {
            await userClassRepository.DeleteUserClassModelAsync(userId, classId, date);
        }

        // In case you need an update method, you can add it here asynchronously.
    }
}
