using MediatR;
using WebApplication1.Database;

namespace WebApplication1.UseCases
{
    public static class CreateProduct
    {
        public record Command(string Name, string Category, decimal Price) : IRequest<int>;

        internal class CommandHandler(ApplicationDbContext dbContext) : IRequestHandler<Command, int>
        {
            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = new Product {
                     Category = request.Category,
                     Name = request.Name,
                     Price = request.Price
                };
                await dbContext.Products.AddAsync(product);
                await dbContext.SaveChangesAsync(cancellationToken);
                return product.Id;
            }
        }
    }
}
