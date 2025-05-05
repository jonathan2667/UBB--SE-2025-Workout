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
        private readonly WorkoutDbContext _context;
        
        public ClassTypeRepo(WorkoutDbContext context)
        {
            _context = context;
        }

        public async Task<ClassTypeModel> GetClassTypeModelByIdAsync(int classTypeId)
        {
            var classType = await _context.ClassTypes
                .FirstOrDefaultAsync(ct => ct.CTID == classTypeId);
                
            return classType ?? new ClassTypeModel();
        }

        public async Task<List<ClassTypeModel>> GetAllClassTypeModelAsync()
        {
            return await _context.ClassTypes.ToListAsync();
        }

        public async Task AddClassTypeModelAsync(ClassTypeModel classType)
        {
            _context.ClassTypes.Add(classType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClassTypeModelAsync(int classTypeId)
        {
            var classType = await _context.ClassTypes.FindAsync(classTypeId);
            if (classType != null)
            {
                _context.ClassTypes.Remove(classType);
                await _context.SaveChangesAsync();
            }
        }
    }
}
