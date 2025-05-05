using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using Workout.Core.Models;
using Workout.Core.Services.Interfaces;

namespace NeoIsisJob.Proxy
{
    public interface IUserServiceProxy : IUserService
    {
        [Post("/api/user")]
        Task<int> RegisterNewUserAsync();

        [Get("/api/user/{userId}")]
        Task<UserModel> GetUserAsync(int userId);

        [Delete("/api/user/{userId}")]
        Task<bool> RemoveUserAsync(int userId);

        [Get("/api/user")]
        Task<IList<UserModel>> GetAllUsersAsync();
    }
}
