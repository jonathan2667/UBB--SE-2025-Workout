using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Server.Data;

namespace Workout.Server.Repositories
{
    public class UserClassRepo : IUserClassRepo
    {
        private readonly WorkoutDbContext _context;

        public UserClassRepo(WorkoutDbContext context)
        {
            _context = context;
        }

        public async Task<UserClassModel?> GetUserClassModelByIdAsync(int userId, int classId, DateTime enrollmentDate)
        {
            return await _context.UserClasses
                .Include(uc => uc.User)
                .Include(uc => uc.Class)
                .FirstOrDefaultAsync(uc => uc.UID == userId && uc.CID == classId && uc.Date == enrollmentDate);
        }

        public async Task<List<UserClassModel>> GetAllUserClassModelAsync()
        {
            return await _context.UserClasses
                .Include(uc => uc.User)
                .Include(uc => uc.Class)
                .ToListAsync();
        }

        public async Task AddUserClassModelAsync(UserClassModel userClass)
        {
            _context.UserClasses.Add(userClass);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserClassModelAsync(int userId, int classId, DateTime enrollmentDate)
        {
            var userClass = await _context.UserClasses
                .FirstOrDefaultAsync(uc => uc.UID == userId && uc.CID == classId && uc.Date == enrollmentDate);
                
            if (userClass != null)
            {
                _context.UserClasses.Remove(userClass);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<UserClassModel>> GetUserClassModelByDateAsync(DateTime date)
        {
            return await _context.UserClasses
                .Include(uc => uc.User)
                .Include(uc => uc.Class)
                .Where(uc => uc.Date.Date == date.Date)
                .ToListAsync();
        }
    }
}
