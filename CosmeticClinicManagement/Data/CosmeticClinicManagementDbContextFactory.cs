using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CosmeticClinicManagement.Data;

public class CosmeticClinicManagementDbContextFactory : IDesignTimeDbContextFactory<CosmeticClinicManagementDbContext>
{
    public CosmeticClinicManagementDbContext CreateDbContext(string[] args)
    {
        CosmeticClinicManagementEfCoreEntityExtensionMappings.Configure();
        
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<CosmeticClinicManagementDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new CosmeticClinicManagementDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
