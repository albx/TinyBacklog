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
    public class UpdateTaskStatusFunction
    {
        public ITaskStore Store { get; }

        public UpdateTaskStatusFunction(ITaskStore store)
        {
            Store = store ?? throw new ArgumentNullException(nameof(store));
        }

        [FunctionName("UpdateTaskStatus")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "patch", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var model = await req.ParseRequestBodyAsync<UpdateTaskStatusViewModel>();

            try
            {
                var status = model.Status.ToTaskStatus();
                await Store.UpdateTaskStatus(model.TaskId, status);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                log.LogError(ex, "Task with id {TaskId} not found: {ErrorMessage}", model.TaskId, ex.Message);
                return new BadRequestObjectResult("Task not found");
            }

            return new NoContentResult();
        }
    }
}
