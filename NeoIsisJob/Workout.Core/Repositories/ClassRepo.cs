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
        private readonly WorkoutDbContext _context;

        public ClassRepo(WorkoutDbContext context)
        {
            _context = context;
        }

        public async Task<ClassModel> GetClassModelByIdAsync(int classId)
        {
            var classModel = await _context.Classes
                .Include(c => c.PersonalTrainer)
                .Include(c => c.ClassType)
                .FirstOrDefaultAsync(c => c.CID == classId);
                
            return classModel ?? new ClassModel();
        }

        public async Task<List<ClassModel>> GetAllClassModelAsync()
        {
            return await _context.Classes
                .Include(c => c.PersonalTrainer)
                .Include(c => c.ClassType)
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
