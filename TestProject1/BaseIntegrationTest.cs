using MediatR;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Database;

namespace TestProject1
{
    public abstract class BaseIntegrationTest
    : IClassFixture<IntegrationTestWebAppFactory>,
      IDisposable
    {
        private readonly IServiceScope _scope;
        protected readonly ISender sender;
        protected readonly ApplicationDbContext dbContext;

        protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
        {
            _scope = factory.Services.CreateScope();
            sender = _scope.ServiceProvider.GetRequiredService<ISender>();
            dbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Database.EnsureDeleted();
            dbContext.Database.Migrate();
        }

        public void Dispose()
        {
            _scope?.Dispose();
            dbContext?.Dispose();
        }
    }
}
