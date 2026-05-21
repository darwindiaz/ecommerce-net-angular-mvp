using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Ecommerce.Infrastructure.Persistence;

public class EcommerceDbContextFactory : IDesignTimeDbContextFactory<EcommerceDbContext>
{
    public EcommerceDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EcommerceDbContext>();
        optionsBuilder.UseSqlite("Data Source=ecommerce.dev.db");

        return new EcommerceDbContext(optionsBuilder.Options);
    }
}
