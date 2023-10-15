using Microsoft.EntityFrameworkCore;

namespace FruitAPI.DataAccess.Context;

public class DbContextFactory : IDbContextFactory
{
    private readonly IConfiguration _configuration;

    public DbContextFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DataContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("StudentPortalAPIContext"));
        return new DataContext(optionsBuilder.Options);
    }

}

