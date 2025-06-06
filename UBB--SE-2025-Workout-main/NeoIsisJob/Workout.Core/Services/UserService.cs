using Workout.Core.IRepositories;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;

        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
        }

        public async Task<int> RegisterNewUserAsync()
        {
            // No parameters to validate here; delegate to repository
            return await _userRepo
                         .InsertUserAsync()
                         .ConfigureAwait(false);
        }

        public async Task<int> AddUserAsync(string username, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty", nameof(username));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty", nameof(password));

            return await _userRepo.InsertUserAsync(username, email, password);
        }

        public async Task<UserModel> GetUserAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId), "userId must be positive.");
            }

            return await _userRepo
                         .GetUserByIdAsync(userId)
                         .ConfigureAwait(false);
        }

        public async Task<bool> RemoveUserAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId), "userId must be positive.");
            }

            return await _userRepo
                         .DeleteUserByIdAsync(userId)
                         .ConfigureAwait(false);
        }

        public async Task<IList<UserModel>> GetAllUsersAsync()
        {
            var users = await _userRepo.GetAllUsersAsync();
            return users;
        }

        public async Task<long> LoginAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty", nameof(username));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty", nameof(password));

            var user = await _userRepo.GetUserByUsernameAsync(username);

            if (user == null)
                return -2; // User not found

            return user.Password == password ? user.ID : -1; // -1 for wrong password
        }

        public UserModel GetUserByUsername(string username)
        {
            return this._userRepo.GetUserByUsernameAsync(username).Result;
        }

        public void JoinGroup(int userId, long groupId)
        {
            this._userRepo.JoinGroup(userId, groupId);
        }

        public void ExitGroup(int userId, long groupId)
        {
            this._userRepo.ExitGroup(userId, groupId);
        }

        public List<UserModel> GetUserFollowing(int id)
        {
            return this._userRepo.GetUserFollowing(id);
        }

        public void FollowUserById(int userId, int whoToFollowId)
        {
            if (this._userRepo.GetUserByIdAsync((int)userId) == null)
            {
                throw new Exception("User does not exist");
            }

            if (this._userRepo.GetUserByIdAsync((int)whoToFollowId) == null)
            {
                throw new Exception("User to follow does not exist");
            }

            this._userRepo.Follow(userId, whoToFollowId);
        }

        public void UnfollowUserById(int userId, int whoToUnfollowId)
        {
            if (this._userRepo.GetUserByIdAsync((int)userId) == null)
            {
                throw new Exception("User does not exist");
            }

            if (this._userRepo.GetUserByIdAsync((int)whoToUnfollowId) == null)
            {
                throw new Exception("User to unfollow does not exist");
            }

            this._userRepo.Unfollow(userId, whoToUnfollowId);
        }
    }
}
