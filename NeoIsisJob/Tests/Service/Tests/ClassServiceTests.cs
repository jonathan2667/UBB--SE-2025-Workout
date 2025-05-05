//using Moq;
//using NeoIsisJob.Models;
//using NeoIsisJob.Repositories;
//using NeoIsisJob.Repositories.Interfaces;
//using NeoIsisJob.Services;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Tests.Service.Tests
//{
//    [TestClass]
//    public class ClassServiceTests
//    {
//        private Mock<IClassRepository> _mockClassRepo;
//        private UserClassService _userClassService;
//        private ClassService _classService;

//        [TestInitialize]
//        public void Setup()
//        {
//            _mockClassRepo = new Mock<IClassRepository>();
//            _userClassService = new UserClassService();
//            _classService = new ClassService(_mockClassRepo.Object);


//        }
//        [TestMethod]
//        public void ConfirmRegistration_ShouldReturnError_WhenExceptionThrown()
//        {
//            var classId = 5;
//            var userId = 10;
//            var date = DateTime.Today.AddDays(1);

//            _mockClassRepo.Setup(repo => repo.GetClassModelById(classId))
//            .Returns(new ClassModel { Id = classId, Name = "HIIT" });

//            _mockClassRepo.Setup(repo => repo.GetClassModelById(userId))
//                .Returns(new ClassModel { Id = userId});


//            var result = _classService.ConfirmRegistration(classId, userId, date);

//            Assert.IsTrue(result.Contains("Registration failed:"));

//        }
//    }
//}
