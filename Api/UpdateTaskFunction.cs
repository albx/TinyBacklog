using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TinyBacklog.Core;
using TinyBacklog.Api.Extensions;
using TinyBacklog.Shared;

namespace TinyBacklog.Api
{
    public class UpdateTaskFunction
    {
        public ITaskStore Store { get; }

        public UpdateTaskFunction(ITaskStore store)
        {
            Store = store ?? throw new ArgumentNullException(nameof(store));
        }

        [FunctionName("UpdateTask")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var task = await req.ParseRequestBodyAsync<TaskViewModel>();
            var entity = task.ToTaskEntity();

            await Store.UpdateTask(entity);

            return new NoContentResult();
        }
    }
}
