//using NeoIsisJob.Repositories.Interfaces;
//using NeoIsisJob.Services;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Tests.Repo.Mocks;
//using NeoIsisJob.Models;

//namespace Tests.Service.Tests
//{
//    [TestClass]
//    public class WorkoutTypeTests
//    {
//        private WorkoutTypeService service;
//        private IWorkoutTypeRepository repository;

//        [TestInitialize]
//        public void Setup()
//        {
//            repository = new WorkoutTypeMock();
//            service = new WorkoutTypeService(repository);
//        }

//        [TestMethod]
//        public void TestInsert()
//        {

//            service.InsertWorkoutType("Test");

//            var result = repository.GetWorkoutTypeById(service.GetAllWorkoutTypes().Count);
//            Assert.IsNotNull(result);
//            Assert.AreEqual("Test", result.Name);

//            try
//            {
//                service.InsertWorkoutType(null);
//                Assert.Fail("Expected ArgumentNullException was not thrown.");
//            }
//            catch (Exception ex)
//            {
//                Assert.Contains( "empty or null", ex.Message);
//            }
//        }
//    }
//}
