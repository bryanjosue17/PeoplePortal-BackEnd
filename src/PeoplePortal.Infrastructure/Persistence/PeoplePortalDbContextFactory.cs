using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PeoplePortal.Infrastructure.Persistence;

public class PeoplePortalDbContextFactory : IDesignTimeDbContextFactory<PeoplePortalDbContext>
{
    public PeoplePortalDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PeoplePortalDbContext>();
        var connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") ??
            Environment.GetEnvironmentVariable("ConnectionStrings:DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string not found. Set ConnectionStrings__DefaultConnection or ConnectionStrings:DefaultConnection.");
        }

        optionsBuilder.UseNpgsql(connectionString);

        return new PeoplePortalDbContext(optionsBuilder.Options);
    }
}
