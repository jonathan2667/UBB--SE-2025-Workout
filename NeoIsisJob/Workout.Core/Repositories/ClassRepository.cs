using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Workout.Core.Models;
using Workout.Core.Repositories.Interfaces;
using Workout.Server.Data;

namespace Workout.Core.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly WorkoutDbContext _context;

        public ClassRepository(WorkoutDbContext context)
        {
            _context = context;
        }

        public async Task<ClassModel> GetClassModelByIdAsync(int classId)
        {
            return await _context.Classes
                .Include(c => c.ClassType)
                .Include(c => c.PersonalTrainer)
                .FirstOrDefaultAsync(c => c.CID == classId);
        }

        public async Task<List<ClassModel>> GetAllClassModelAsync()
        {
            return await _context.Classes
                .Include(c => c.ClassType)
                .Include(c => c.PersonalTrainer)
                .ToListAsync();
        }

        public async Task AddClassModelAsync(ClassModel classModel)
        {
            _context.Classes.Add(classModel);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClassModelAsync(int classId)
        {
            var classModel = await _context.Classes.FindAsync(classId);
            if (classModel != null)
            {
                _context.Classes.Remove(classModel);
                await _context.SaveChangesAsync();
            }
        }
    }
}
