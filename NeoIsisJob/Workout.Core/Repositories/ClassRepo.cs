using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Workout.Core.Models;
using Workout.Core.Data;

namespace Workout.Core.Repositories
{
    public class ClassRepo
    {
        private readonly WorkoutDbContext context;

        public ClassRepo(WorkoutDbContext context)
        {
            this.context = context;
        }

        public async Task<ClassModel> GetClassModelByIdAsync(int classId)
        {
            var classModel = await context.Classes
                .Include(c => c.PersonalTrainer)
                .Include(c => c.ClassType)
                .FirstOrDefaultAsync(c => c.CID == classId);
            return classModel ?? new ClassModel();
        }

        public async Task<List<ClassModel>> GetAllClassModelAsync()
        {
            return await context.Classes
                .Include(c => c.PersonalTrainer)
                .Include(c => c.ClassType)
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
