using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using TinyBacklog.Core;

[assembly: FunctionsStartup(typeof(TinyBacklog.Api.Startup))]
namespace TinyBacklog.Api;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.GetContext().Configuration;

        services.AddTaskStore(options =>
        {
            options.ConnectionString = configuration["TaskDatabaseConnection"];
            options.TableName = configuration["TaskTableName"];
        });
    }
}
