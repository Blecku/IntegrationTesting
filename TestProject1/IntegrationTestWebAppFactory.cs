using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using WebApplication1.Database;

namespace TestProject1
{
    public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {

        //private SqlConnectionStringBuilder builder;

        protected string DatabaseName { get; private set; }

        private MsSqlContainer _container;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptorType =
                    typeof(DbContextOptions<ApplicationDbContext>);

                var descriptor = services
                    .SingleOrDefault(s => s.ServiceType == descriptorType);

                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                }

                var builder = new SqlConnectionStringBuilder(_container.GetConnectionString())
                {
                    InitialCatalog = DatabaseName
                };
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(builder.ToString()));
            });
        }

        public async Task InitializeAsync()
        {
            // Pobierz współdzielony kontener
            _container = await SharedMsSqlContainer.GetContainerAsync();

            // Utwórz unikalną nazwę bazy danych dla tej klasy testów
            DatabaseName = $"TestDb_{GetType().Name}_{Guid.NewGuid():N}";

            // Utwórz nową bazę danych
            await CreateDatabaseAsync();

        }

        public new async Task DisposeAsync()
        {
            await DropDatabaseAsync();
        }

        private async Task CreateDatabaseAsync()
        {
            var cs = SharedMsSqlContainer.GetConnectionString();
            using var connection = new SqlConnection(cs);
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = $"CREATE DATABASE [{DatabaseName}]";
            await command.ExecuteNonQueryAsync();
        }

        private async Task DropDatabaseAsync()
        {
            using var connection = new SqlConnection(SharedMsSqlContainer.GetConnectionString());
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            // Najpierw rozłącz wszystkie połączenia
            command.CommandText = $@"
            ALTER DATABASE [{DatabaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
            DROP DATABASE [{DatabaseName}];";
            await command.ExecuteNonQueryAsync();
        }
    }
}
