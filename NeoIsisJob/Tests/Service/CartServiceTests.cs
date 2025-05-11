using Moq;
using Workout.Core.Models;
using Workout.Core.Services;
using Xunit;
using Workout.Core.IRepositories;
using Assert = Xunit.Assert;

namespace Workout.Core.Services
{
    public class CartServiceTests
    {
        private readonly Mock<IRepository<CartItemModel>> cartRepositoryMock;
        private readonly CartService cartService;
        private readonly int customerID;

        public CartServiceTests()
        {
            cartRepositoryMock = new Mock<IRepository<CartItemModel>>();
            customerID = 1;
            cartService = new CartService(cartRepositoryMock.Object);
        }

        [Fact]
        public async Task GetCartItems_ShouldReturnAllCartItems()
        {
            UserModel user1 = new UserModel { ID = 1 };
            UserModel user2 = new UserModel { ID = 2 };
            CategoryModel category = new CategoryModel { ID = 10, Name = "Category" };
            ProductModel product1 = new ProductModel
            {
                ID = 100,
                CategoryID = 10,
                Name = "Test Product1",
                PhotoURL = "http://example.com/image.jpg"
            };
            ProductModel product2 = new ProductModel
            {
                ID = 101,
                CategoryID = 10,
                Name = "Test Product2",
                PhotoURL = "http://example.com/image.jpg"
            };

            var items = new List<CartItemModel>
            {
                new CartItemModel { ID = 1, UserID = 1, ProductID = 100 },
                new CartItemModel { ID = 2, UserID = 1, ProductID = 101 },
            };

            cartRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(items);

            var result = (List<CartItemModel>)await cartService.GetAllAsync();

            Assert.Equal(2, result.Count);
            cartRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetCartItemById_ShouldReturnCorrectItem()
        {
            int itemId = 1;
            UserModel user1 = new UserModel { ID = 1 };
            CategoryModel category = new CategoryModel { ID = 10, Name = "Category" };
            ProductModel product = new ProductModel
            {
                ID = 100,
                CategoryID = 10,
                Name = "Test Product",
                PhotoURL = "http://example.com/image.jpg"
            };

            CartItemModel cartItem = new CartItemModel
            {
                ID = 1,
                UserID = 1,
                ProductID = 100,
                Product = product // Populate the Product property
            };

            cartRepositoryMock.Setup(repo => repo.GetByIdAsync(itemId)).ReturnsAsync(cartItem);

            var result = await cartService.GetByIdAsync(itemId);

            Assert.Equal(itemId, result.ID);
            Assert.NotNull(result);
            Assert.Equal("Test Product", result.Product.Name);

            cartRepositoryMock.Verify(repo => repo.GetByIdAsync(itemId), Times.Once);
        }

        [Fact]
        public async Task RemoveCartItem_ShouldCallDeleteAsync()
        {
            int itemId = 1;

            await cartService.DeleteAsync(itemId);

            cartRepositoryMock.Verify(repo => repo.DeleteAsync(itemId), Times.Once);
        }

        [Fact]
        public async Task AddToCart_ShouldCallCreateAsyncWithCorrectParams()
        {
            UserModel user1 = new UserModel { ID = 1 };
            CategoryModel category = new CategoryModel { ID = 10, Name = "Category" };
            ProductModel product2 = new ProductModel
            {
                ID = 101,
                CategoryID = 10,
                Name = "Test Product2",
                PhotoURL = "http://example.com/image.jpg"
            };

            CartItemModel item = new CartItemModel { ID = 1, UserID = 1, ProductID = 100 };

            await cartService.CreateAsync(item);

            cartRepositoryMock.Verify(repo => repo.CreateAsync(item), Times.Once);
        }

        [Fact]
        public async Task ResetCart_ShouldDeleteAllItems()
        {
            UserModel user1 = new UserModel { ID = 1 };
            UserModel user2 = new UserModel { ID = 2 };
            CategoryModel category = new CategoryModel { ID = 10, Name = "Category" };
            ProductModel product1 = new ProductModel
            {
                ID = 100,
                CategoryID = 10,
                Name = "Test Product1",
                PhotoURL = "http://example.com/image.jpg"
            };
            ProductModel product2 = new ProductModel
            {
                ID = 101,
                CategoryID = 10,
                Name = "Test Product2",
                PhotoURL = "http://example.com/image.jpg"
            };

            var items = new List<CartItemModel>
            {
                new CartItemModel { ID = 1, UserID = 1, ProductID = 100 },
                new CartItemModel { ID = 2, UserID = 1, ProductID = 101 },
            };

            cartRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(items);

            await cartService.ResetCart();

            cartRepositoryMock.Verify(repo => repo.DeleteAsync(1), Times.Once);
            cartRepositoryMock.Verify(repo => repo.DeleteAsync(2), Times.Once);
        }
    }
}
