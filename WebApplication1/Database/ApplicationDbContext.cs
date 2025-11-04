using Microsoft.EntityFrameworkCore;
using System.Net;

namespace WebApplication1.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
