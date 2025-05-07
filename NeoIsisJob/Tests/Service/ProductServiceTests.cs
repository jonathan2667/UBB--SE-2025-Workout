using Moq;
using Workout.Core.IServices;
using Workout.Core.IRepositories;
using Workout.Core.Models;
using Workout.Core.Services;
using Workout.Core.Utils.Filters;
using Xunit;

namespace Workout.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IRepository<ProductModel>> mockRepo;
        private readonly IService<ProductModel> productService;

        public ProductServiceTests()
        {
            mockRepo = new Mock<IRepository<ProductModel>>();
            productService = new ProductService(mockRepo.Object);
        }

        [Fact]
        public async Task CreateAsync_Should_Call_Repo_And_Return_Product()
        {
            // Arrange
            var newProduct = new ProductModel { ID = 1, Name = "Test Product" };
            mockRepo.Setup(r => r.CreateAsync(newProduct)).ReturnsAsync(newProduct);

            // Act
            var result = await productService.CreateAsync(newProduct);

            // Assert
            Xunit.Assert.Equal(newProduct, result);
            mockRepo.Verify(r => r.CreateAsync(newProduct), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Should_Return_True()
        {
            // Arrange
            mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await productService.DeleteAsync(1);

            // Assert
            Xunit.Assert.True(result);
            mockRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_Product_List()
        {
            // Arrange
            var products = new List<ProductModel>
        {
            new ProductModel { ID = 1, Name = "A" },
            new ProductModel { ID = 2, Name = "B" }
        };
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await productService.GetAllAsync();

            // Assert
            Xunit.Assert.Equal(2, ((List<ProductModel>)result).Count);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Product()
        {
            // Arrange
            var product = new ProductModel { ID = 1, Name = "Item" };
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await productService.GetByIdAsync(1);

            // Assert
            Xunit.Assert.Equal(product, result);
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Updated_Product()
        {
            // Arrange
            var updatedProduct = new ProductModel { ID = 1, Name = "Updated" };
            mockRepo.Setup(r => r.UpdateAsync(updatedProduct)).ReturnsAsync(updatedProduct);

            // Act
            var result = await productService.UpdateAsync(updatedProduct);

            // Assert
            Xunit.Assert.Equal("Updated", result.Name);
        }

        [Fact]
        public async Task GetFilteredAsync_Should_Return_Filtered_Products()
        {
            // Arrange
            var filter = new Mock<IFilter>().Object;
            var filtered = new List<ProductModel>
        {
            new ProductModel { ID = 3, Name = "Filtered" }
        };

            mockRepo.Setup(r => r.GetAllFilteredAsync(filter)).ReturnsAsync(filtered);

            // Act
            var result = await productService.GetFilteredAsync(filter);

            // Assert
            Xunit.Assert.Single(result);
        }
    }
}
