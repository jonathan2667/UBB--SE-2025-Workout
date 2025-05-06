using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Workout.Core.Models;
using Workout.Core.Data;

namespace Workout.Core.Repositories
{
    public class ClassTypeRepo
    {
        private readonly WorkoutDbContext context;
        public ClassTypeRepo(WorkoutDbContext context)
        {
            this.context = context;
        }

        public async Task<ClassTypeModel> GetClassTypeModelByIdAsync(int classTypeId)
        {
            var classType = await context.ClassTypes
                .FirstOrDefaultAsync(ct => ct.CTID == classTypeId);
            return classType ?? new ClassTypeModel();
        }

        public async Task<List<ClassTypeModel>> GetAllClassTypeModelAsync()
        {
            return await context.ClassTypes.ToListAsync();
        }

        public async Task AddClassTypeModelAsync(ClassTypeModel classType)
        {
            context.ClassTypes.Add(classType);
            await context.SaveChangesAsync();
        }

        public async Task DeleteClassTypeModelAsync(int classTypeId)
        {
            var classType = await context.ClassTypes.FindAsync(classTypeId);
            if (classType != null)
            {
                context.ClassTypes.Remove(classType);
                await context.SaveChangesAsync();
            }
        }
    }
}
