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
        private readonly WorkoutDbContext _context;

        public WorkoutTypeRepo(WorkoutDbContext context)
        {
            _context = context;
        }

        public async Task<WorkoutTypeModel> GetWorkoutTypeByIdAsync(int workoutTypeId)
        {
            var workoutType = await _context.WorkoutTypes
                .FirstOrDefaultAsync(wt => wt.WTID == workoutTypeId);
                
            return workoutType ?? new WorkoutTypeModel();
        }

        public async Task InsertWorkoutTypeAsync(string workoutTypeName)
        {
            var workoutType = new WorkoutTypeModel { Name = workoutTypeName };
            _context.WorkoutTypes.Add(workoutType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWorkoutTypeAsync(int workoutTypeId)
        {
            var workoutType = await _context.WorkoutTypes.FindAsync(workoutTypeId);
            if (workoutType != null)
            {
                _context.WorkoutTypes.Remove(workoutType);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IList<WorkoutTypeModel>> GetAllWorkoutTypesAsync()
        {
            return await _context.WorkoutTypes.ToListAsync();
        }
    }
}
