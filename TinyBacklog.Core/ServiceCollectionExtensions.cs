using Microsoft.Extensions.DependencyInjection;
using TinyBacklog.Core.Options;

namespace TinyBacklog.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTaskStore(this IServiceCollection services, Action<TaskStoreOptions> configureOptions)
    {
        services.Configure(configureOptions);

        services.AddScoped<ITaskStore, TaskStore>();
        return services;
    }
}
