//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using NeoIsisJob.Repositories;
//using NeoIsisJob.Models;


//namespace Tests.Repo.Mocks
//{
//    class UserClassMock : UserClassRepo
//    {
//        private readonly List<UserClassModel> userClasses;
//        public UserClassMock()
//        {

//            userClasses = new List<UserClassModel>
//            {
//                new UserClassModel(1, 1, DateTime.Now),
//                new UserClassModel(1, 2, DateTime.Now),
//                new UserClassModel(1, 3, DateTime.Now)
//            };

//        }

//        public override UserClassModel GetUserClassModelById(int UserId, int classId, DateTime date)
//        {
//            return (UserClassModel)userClasses.Where(uc => uc.UserId == UserId);
//        }

//        public override UserClassModel GetUserClassByFullID(int UserId, int classId)
//        {
//            return userClasses.FirstOrDefault(uc => uc.UserId == UserId && uc.ClassId == classId);
//        }

//        public override UserClassModel GetUserClassByID(int userClassId)
//        {
//            return new UserClassModel(1, 1, 1);
//        }

//        public override IList<UserClassModel> GetAllUserClasses()
//        {
//            return new List<UserClassModel>
//            {
//                new UserClassModel(1, 1, 1),
//                new UserClassModel(1, 2, 2),
//                new UserClassModel(1, 3, 3)
//            };
//        }
//    }
//}
