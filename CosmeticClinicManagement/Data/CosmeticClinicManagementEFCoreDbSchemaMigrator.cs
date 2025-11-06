using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace CosmeticClinicManagement.Data;

public class CosmeticClinicManagementEFCoreDbSchemaMigrator : ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public CosmeticClinicManagementEFCoreDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the CosmeticClinicManagementDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<CosmeticClinicManagementDbContext>()
            .Database
            .MigrateAsync();
    }
}
