using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.IRepositories
{
    public interface IClassTypeRepository
    {
        Task<ClassTypeModel?> GetClassTypeModelByIdAsync(int classTypeId);
        Task<List<ClassTypeModel>> GetAllClassTypeModelAsync();
        Task AddClassTypeModelAsync(ClassTypeModel classType);
        Task DeleteClassTypeModelAsync(int classTypeId);
    }
}
