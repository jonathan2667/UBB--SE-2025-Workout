using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.Repositories.Interfaces
{
    public interface IClassRepository
    {
        Task<ClassModel> GetClassModelByIdAsync(int classId);
        Task<List<ClassModel>> GetAllClassModelAsync();
        Task AddClassModelAsync(ClassModel classModel);
        Task DeleteClassModelAsync(int classId);
    }
}
