using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Core.Data;

namespace Workout.Core.Repositories
{
    public class ClassTypeRepository : IClassTypeRepository
    {
        private readonly WorkoutDbContext context;

        public ClassTypeRepository(WorkoutDbContext context)
        {
            this.context = context;
        }

        public async Task<ClassTypeModel?> GetClassTypeModelByIdAsync(int classTypeId)
        {
            try
            {
                return await context.ClassTypes
                    .FirstOrDefaultAsync(ct => ct.CTID == classTypeId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching class type by ID: " + ex.Message);
            }
        }

        public async Task<List<ClassTypeModel>> GetAllClassTypeModelAsync()
        {
            try
            {
                return await context.ClassTypes.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching class types: " + ex.Message);
            }
        }

        public async Task AddClassTypeModelAsync(ClassTypeModel classType)
        {
            try
            {
                context.ClassTypes.Add(classType);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding class type: " + ex.Message);
            }
        }

        public async Task DeleteClassTypeModelAsync(int classTypeId)
        {
            try
            {
                var classType = await context.ClassTypes.FindAsync(classTypeId);
                if (classType != null)
                {
                    context.ClassTypes.Remove(classType);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting class type: " + ex.Message);
            }
        }
    }
}
