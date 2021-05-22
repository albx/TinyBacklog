using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TinyBacklog.Core;
using TinyBacklog.Core.Options;

[assembly: FunctionsStartup(typeof(TinyBacklog.Api.Startup))]
namespace TinyBacklog.Api
{
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
}
