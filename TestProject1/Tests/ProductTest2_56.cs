using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace TestProject1.Tests
{
    public class ProductTest2_56 : BaseIntegrationTest
    {
        public ProductTest2_56(IntegrationTestWebAppFactory factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Create_ShouldCreateCPU()
        {
            // Act
            var productData = new
            {
                Name = "AMD Ryzen 7 7700X",
                Category = "CPU",
                Price = 223.99m
            };
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(productData),
                Encoding.UTF8,
                "application/json");
            var response = await httpClient.PostAsync("/api/WeatherForecast", jsonContent);

            // Assert
            var productId = await response.Content.ReadFromJsonAsync<int>();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal("/api/WeatherForecast/1", response.Headers.Location?.ToString());
            var product = Assert.Single(dbContext.Products);
            Assert.Equal(1, dbContext.Products.Count());
            Assert.Equal(product.Id, productId);
            Assert.Equal(productData.Name, product.Name);
            Assert.Equal(productData.Category, product.Category);
            Assert.Equal(productData.Price, product.Price);
        }

        [Fact]
        public async Task Create_ShouldCreateGPU()
        {
            // Act
            var productData = new
            {
                Name = "Nvidia",
                Category = "GPU",
                Price = 1_000m
            };
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(productData),
                Encoding.UTF8,
                "application/json");
            var response = await httpClient.PostAsync("/api/WeatherForecast", jsonContent);

            // Assert
            var productId = await response.Content.ReadFromJsonAsync<int>();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal("/api/WeatherForecast/1", response.Headers.Location?.ToString());
            var product = Assert.Single(dbContext.Products);
            Assert.Equal(1, dbContext.Products.Count());
            Assert.Equal(product.Id, productId);
            Assert.Equal(productData.Name, product.Name);
            Assert.Equal(productData.Category, product.Category);
            Assert.Equal(productData.Price, product.Price);
        }
    }
}

