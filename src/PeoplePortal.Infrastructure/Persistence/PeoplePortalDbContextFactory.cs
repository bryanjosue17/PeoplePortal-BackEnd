using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PeoplePortal.Infrastructure.Persistence;

public class PeoplePortalDbContextFactory : IDesignTimeDbContextFactory<PeoplePortalDbContext>
{
    public PeoplePortalDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PeoplePortalDbContext>();
        optionsBuilder.UseSqlServer(
            "Server=.\\SQLEXPRESS;Database=PeoplePortalDb;Trusted_Connection=True;TrustServerCertificate=True;");

        return new PeoplePortalDbContext(optionsBuilder.Options);
    }
}