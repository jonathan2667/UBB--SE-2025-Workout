using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Core.Data;

namespace Workout.Core.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly WorkoutDbContext context;

        public ClassRepository(WorkoutDbContext context)
        {
            this.context = context;
        }

        public async Task<ClassModel> GetClassModelByIdAsync(int classId)
        {
            return await context.Classes
                .Include(c => c.ClassType)
                .Include(c => c.PersonalTrainer)
                .FirstOrDefaultAsync(c => c.CID == classId);
        }

        public async Task<List<ClassModel>> GetAllClassModelAsync()
        {
            return await context.Classes
                .Include(c => c.ClassType)
                .Include(c => c.PersonalTrainer)
                .ToListAsync();
        }

        public async Task AddClassModelAsync(ClassModel classModel)
        {
            context.Classes.Add(classModel);
            await context.SaveChangesAsync();
        }

        public async Task DeleteClassModelAsync(int classId)
        {
            var classModel = await context.Classes.FindAsync(classId);
            if (classModel != null)
            {
                context.Classes.Remove(classModel);
                await context.SaveChangesAsync();
            }
        }
    }
}
