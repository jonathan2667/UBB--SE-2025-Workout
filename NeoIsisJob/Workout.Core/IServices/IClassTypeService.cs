using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.IServices
{
    public interface IClassTypeService
    {
        Task<List<ClassTypeModel>> GetAllClassTypesAsync();
        Task<ClassTypeModel> GetClassTypeByIdAsync(int classTypeId);
        Task AddClassTypeAsync(ClassTypeModel classTypeModel);
        Task DeleteClassTypeAsync(int classTypeId);
    }
}
