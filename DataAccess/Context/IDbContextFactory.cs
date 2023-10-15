namespace FruitAPI.DataAccess.Context;

public interface IDbContextFactory
{
    DataContext CreateDbContext();
}

