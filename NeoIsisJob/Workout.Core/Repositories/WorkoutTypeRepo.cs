using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Core.Data;

namespace Workout.Core.Repositories
{
    public class WorkoutTypeRepo : IWorkoutTypeRepository
    {
        private readonly WorkoutDbContext context;

        public WorkoutTypeRepo(WorkoutDbContext context)
        {
            this.context = context;
        }

        public async Task<WorkoutTypeModel> GetWorkoutTypeByIdAsync(int workoutTypeId)
        {
            var workoutType = await context.WorkoutTypes
                .FirstOrDefaultAsync(wt => wt.WTID == workoutTypeId);

            return workoutType ?? new WorkoutTypeModel();
        }

        public async Task InsertWorkoutTypeAsync(string workoutTypeName)
        {
            var workoutType = new WorkoutTypeModel { Name = workoutTypeName };
            context.WorkoutTypes.Add(workoutType);
            await context.SaveChangesAsync();
        }

        public async Task DeleteWorkoutTypeAsync(int workoutTypeId)
        {
            var workoutType = await context.WorkoutTypes.FindAsync(workoutTypeId);
            if (workoutType != null)
            {
                context.WorkoutTypes.Remove(workoutType);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IList<WorkoutTypeModel>> GetAllWorkoutTypesAsync()
        {
            return await context.WorkoutTypes.ToListAsync();
        }
    }
}
