using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Server.Repositories;
using Workout.Core.IServices;

namespace Workout.Server.Services
{
    public class ClassTypeService: IClassTypeService
    {
        private readonly IClassTypeRepository classTypeRepository;

        public ClassTypeService()
        {
            this.classTypeRepository = new ClassTypeRepository();
        }

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
            //if (classTypeId <= 0)
            //    throw new ArgumentException("ClassType ID must be greater than zero.", nameof(classTypeId));

            return await classTypeRepository.GetClassTypeModelByIdAsync(classTypeId);
        }

        public async Task AddClassTypeAsync(ClassTypeModel classTypeModel)
        {
            //if (classTypeModel == null)
            //    throw new ArgumentNullException(nameof(classTypeModel), "ClassType model cannot be null.");

            //if (string.IsNullOrWhiteSpace(classTypeModel.Name))
            //    throw new ArgumentException("ClassType name cannot be empty.", nameof(classTypeModel.Name));

            await classTypeRepository.AddClassTypeModelAsync(classTypeModel);
        }

        public async Task DeleteClassTypeAsync(int classTypeId)
        {
            //if (classTypeId <= 0)
            //    throw new ArgumentException("ClassType ID must be greater than zero.", nameof(classTypeId));

            await classTypeRepository.DeleteClassTypeModelAsync(classTypeId);
        }
    }
}
