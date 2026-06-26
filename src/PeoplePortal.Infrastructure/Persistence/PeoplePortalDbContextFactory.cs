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
            Environment.GetEnvironmentVariable("ConnectionStrings:DefaultConnection") ??
            "Server=.\\SQLEXPRESS;Database=PeoplePortalDb;Trusted_Connection=True;TrustServerCertificate=True;";

        optionsBuilder.UseSqlServer(connectionString);

        return new PeoplePortalDbContext(optionsBuilder.Options);
    }
}
