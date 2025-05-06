using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Core.Repositories;
using Workout.Core.IServices;

namespace Workout.Core.Services
{
    public class ClassTypeService : IClassTypeService
    {
        private readonly IClassTypeRepository classTypeRepository;

        public ClassTypeService(IClassTypeRepository classTypeRepository)
        {
            this.classTypeRepository = classTypeRepository ?? throw new ArgumentNullException(nameof(classTypeRepository));
        }

        public async Task<List<ClassTypeModel>> GetAllClassTypesAsync()
        {
            return await classTypeRepository.GetAllClassTypeModelAsync();
        }

        public async Task<ClassTypeModel> GetClassTypeByIdAsync(int classTypeId)
        {
            return await classTypeRepository.GetClassTypeModelByIdAsync(classTypeId);
        }

        public async Task AddClassTypeAsync(ClassTypeModel classTypeModel)
        {
            await classTypeRepository.AddClassTypeModelAsync(classTypeModel);
        }

        public async Task DeleteClassTypeAsync(int classTypeId)
        {
            await classTypeRepository.DeleteClassTypeModelAsync(classTypeId);
        }
    }
}
