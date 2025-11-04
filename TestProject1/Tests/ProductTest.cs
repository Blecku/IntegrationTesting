using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.UseCases;

namespace TestProject1.Tests
{
    public class ProductTest : BaseIntegrationTest
    {
        public ProductTest(IntegrationTestWebAppFactory factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Create_ShouldCreateProduct()
        {
            // Arrange
            var command = new CreateProduct.Command("AMD Ryzen 7 7700X", "CPU", 223.99m);

            // Act
            var productId = await sender.Send(command);

            // Assert
            var product = dbContext.Products.Single(p => p.Id == productId);
            Assert.Single(dbContext.Products);
            Assert.NotNull(product);
        }

        [Fact]
        public async Task Create_ShouldCreateProduct2()
        {
            // Arrange
            var command = new CreateProduct.Command("AMD Ryzen 7 7700X", "CPU", 223.99m);

            // Act
            var productId = await sender.Send(command);

            // Assert
            var product = dbContext.Products.Single(p => p.Id == productId);
            Assert.Single(dbContext.Products);
            Assert.NotNull(product);
        }
    }
}
