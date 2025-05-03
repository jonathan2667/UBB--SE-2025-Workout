using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.Repositories.Interfaces
{
    internal interface IClassRepository
    {
        public ClassModel GetClassModelById(int classId);
        public List<ClassModel> GetAllClassModel();
        public void AddClassModel(ClassModel classModel);
        public void DeleteClassModel(int classId);
    }
}
