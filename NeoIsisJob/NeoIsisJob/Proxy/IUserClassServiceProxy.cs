using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using Workout.Core.Models;
using Workout.Core.Services.Interfaces;

namespace NeoIsisJob.Proxy
{
    public interface IUserClassServiceProxy : IUserClassService
    {
        [Get("/api/userclass")]
        Task<List<UserClassModel>> GetAllUserClassesAsync();

        [Get("/api/userclass/{userId}/{classId}/{date}")]
        Task<UserClassModel> GetUserClassByIdAsync(int userId, int classId, string date); // Format: yyyy-MM-dd

        [Post("/api/userclass")]
        Task AddUserClassAsync([Body] UserClassModel userClassModel);

        [Delete("/api/userclass/{userId}/{classId}/{date}")]
        Task DeleteUserClassAsync(int userId, int classId, string date); // Format: yyyy-MM-dd
    }
}
