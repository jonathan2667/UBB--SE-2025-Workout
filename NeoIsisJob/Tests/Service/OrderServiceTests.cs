using Moq;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Core.Services;
using Xunit;

namespace Workout.Tests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IRepository<OrderModel>> mockOrderRepo;
        private readonly Mock<IRepository<CartItemModel>> mockCartRepo;
        private readonly OrderService orderService;

        public OrderServiceTests()
        {
            mockOrderRepo = new Mock<IRepository<OrderModel>>();
            mockCartRepo = new Mock<IRepository<CartItemModel>>();
            orderService = new OrderService(mockOrderRepo.Object, mockCartRepo.Object);
        }

        [Fact]
        public async Task CreateAsync_Should_Return_Created_Order()
        {
            var order = new OrderModel { ID = 1 };
            mockOrderRepo.Setup(r => r.CreateAsync(order)).ReturnsAsync(order);

            var result = await orderService.CreateAsync(order);

            Xunit.Assert.Equal(order, result);
            mockOrderRepo.Verify(r => r.CreateAsync(order), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Should_Call_Repo_And_Return_True()
        {
            mockOrderRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            var result = await orderService.DeleteAsync(1);

            Xunit.Assert.True(result);
            mockOrderRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_Orders()
        {
            var orders = new List<OrderModel> { new OrderModel { ID = 1 } };
            mockOrderRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);

            var result = await orderService.GetAllAsync();

            Xunit.Assert.Single(result);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Correct_Order()
        {
            var order = new OrderModel { ID = 1 };
            mockOrderRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(order);

            var result = await orderService.GetByIdAsync(1);

            Xunit.Assert.Equal(order, result);
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Updated_Order()
        {
            var order = new OrderModel { ID = 1 };
            mockOrderRepo.Setup(r => r.UpdateAsync(order)).ReturnsAsync(order);

            var result = await orderService.UpdateAsync(order);

            Xunit.Assert.Equal(order, result);
        }

        [Fact]
        public async Task CreateOrderFromCart_Should_Create_Order_And_Clear_Cart()
        {
            // Arrange
            var cartItems = new List<CartItemModel>
        {
            new CartItemModel
            {
                ID = 1,
                UserID = 101,
                Product = new ProductModel { ID = 10 }
            },
            new CartItemModel
            {
                ID = 2,
                UserID = 101,
                Product = new ProductModel { ID = 20 }
            }
        };

            mockCartRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(cartItems);
            mockCartRepo.Setup(r => r.DeleteAsync(It.IsAny<int>())).ReturnsAsync(true);
            mockOrderRepo.Setup(r => r.CreateAsync(It.IsAny<OrderModel>())).ReturnsAsync(new OrderModel());

            // Act
            await orderService.CreateOrderFromCart();

            // Assert
            mockCartRepo.Verify(r => r.GetAllAsync(), Times.Once);
            mockCartRepo.Verify(r => r.DeleteAsync(1), Times.Once);
            mockCartRepo.Verify(r => r.DeleteAsync(2), Times.Once);
            mockOrderRepo.Verify(r => r.CreateAsync(It.Is<OrderModel>(o =>
                o.OrderItems.Count == 2 &&
                o.OrderItems.Any(i => i.ProductID == 10) &&
                o.OrderItems.Any(i => i.ProductID == 20) &&
                o.UserID == 101)), Times.Once);
        }
    }
}
