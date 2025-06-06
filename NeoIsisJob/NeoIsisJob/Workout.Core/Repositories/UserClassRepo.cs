﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Core.Data;

namespace Workout.Core.Repositories
{
    public class UserClassRepo : IUserClassRepo
    {
        private readonly WorkoutDbContext context;

        public UserClassRepo(WorkoutDbContext context)
        {
            this.context = context;
        }

        public async Task<UserClassModel?> GetUserClassModelByIdAsync(int userId, int classId, DateTime enrollmentDate)
        {
            return await context.UserClasses
                .Include(uc => uc.User)
                .Include(uc => uc.Class)
                .FirstOrDefaultAsync(uc => uc.UID == userId && uc.CID == classId && uc.Date == enrollmentDate);
        }

        public async Task<List<UserClassModel>> GetAllUserClassModelAsync()
        {
            return await context.UserClasses
                .Include(uc => uc.User)
                .Include(uc => uc.Class)
                .ToListAsync();
        }

        public async Task AddUserClassModelAsync(UserClassModel userClass)
        {
            context.UserClasses.Add(userClass);
            await context.SaveChangesAsync();
        }

        public async Task DeleteUserClassModelAsync(int userId, int classId, DateTime enrollmentDate)
        {
            var userClass = await context.UserClasses
                .FirstOrDefaultAsync(uc => uc.UID == userId && uc.CID == classId && uc.Date == enrollmentDate);
            if (userClass != null)
            {
                context.UserClasses.Remove(userClass);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<UserClassModel>> GetUserClassModelByDateAsync(DateTime date)
        {
            return await context.UserClasses
                .Include(uc => uc.User)
                .Include(uc => uc.Class)
                .Where(uc => uc.Date.Date == date.Date)
                .ToListAsync();
        }
    }
}
