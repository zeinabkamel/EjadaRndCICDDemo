using CosmeticClinicManagement.Data;
using Serilog;
using Serilog.Events;
using Volo.Abp.Data;

namespace CosmeticClinicManagement;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        var loggerConfiguration = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Async(c => c.File("Logs/logs.txt"))
            .WriteTo.Async(c => c.Console());

        if (IsMigrateDatabase(args))
        {
            loggerConfiguration.MinimumLevel.Override("Volo.Abp", LogEventLevel.Warning);
            loggerConfiguration.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
        }

        Log.Logger = loggerConfiguration.CreateLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.AddAppSettingsSecretsJson()
                .UseAutofac()
                .UseSerilog();
            if (IsMigrateDatabase(args))
            {
                builder.Services.AddDataMigrationEnvironment();
            }
            await builder.AddApplicationAsync<CosmeticClinicManagementModule>();
            var app = builder.Build();
            await app.InitializeApplicationAsync();

            if (IsMigrateDatabase(args))
            {
                await app.Services.GetRequiredService<CosmeticClinicManagementDbMigrationService>().MigrateAsync();
                return 0;
            }

            using (var scope = app.Services.CreateScope())
            {
                await scope.ServiceProvider
                    .GetRequiredService<IDataSeeder>()
                    .SeedAsync();
            }

            Log.Information("Starting CosmeticClinicManagement.");
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            if (ex is HostAbortedException)
            {
                throw;
            }

            Log.Fatal(ex, "CosmeticClinicManagement terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static bool IsMigrateDatabase(string[] args)
    {
        return args.Any(x => x.Contains("--migrate-database", StringComparison.OrdinalIgnoreCase));
    }
}
