using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Core.Data;

namespace Workout.Core.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly WorkoutDbContext _context;

        public UserRepo(WorkoutDbContext context)
        {
            _context = context;
        }

        public async Task<UserModel?> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.ID == userId);
        }

        public async Task<int> InsertUserAsync()
        {
            var user = new UserModel();
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.ID;
        }

        public async Task<bool> DeleteUserByIdAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
