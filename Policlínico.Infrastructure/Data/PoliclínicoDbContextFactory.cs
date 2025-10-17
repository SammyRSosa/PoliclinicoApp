using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Policlínico.Infrastructure.Data;

public class PoliclínicoDbContextFactory : IDesignTimeDbContextFactory<PoliclínicoDbContext>
{
    public PoliclínicoDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PoliclínicoDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=policlinico_db;Username=policlinico_user;Password=SadelRi0");

        return new PoliclínicoDbContext(optionsBuilder.Options);
    }
}
