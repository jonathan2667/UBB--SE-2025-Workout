using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.Repositories.Interfaces
{
    internal interface IClassTypeRepository
    {
        ClassTypeModel GetClassTypeModelById(int classTypeId);
        List<ClassTypeModel> GetAllClassTypeModel();
        void AddClassTypeModel(ClassTypeModel classType);
        void DeleteClassTypeModel(int classTypeId);
    }
}
