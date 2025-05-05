using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using Workout.Core.Models;
using Workout.Core.Services.Interfaces;

namespace NeoIsisJob.Proxy
{
    public interface IClassTypeServiceProxy : IClassTypeService
    {
        [Get("/api/classtype")]
        Task<List<ClassTypeModel>> GetAllClassTypesAsync();

        [Get("/api/classtype/{classTypeId}")]
        Task<ClassTypeModel> GetClassTypeByIdAsync(int classTypeId);

        [Post("/api/classtype")]
        Task AddClassTypeAsync([Body] ClassTypeModel classTypeModel);

        [Delete("/api/classtype/{classTypeId}")]
        Task DeleteClassTypeAsync(int classTypeId);
    }
}
