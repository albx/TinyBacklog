using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TinyBacklog.Core;
using System.Linq;
using TinyBacklog.Api.Extensions;
using TinyBacklog.Shared;
using System.Security.Claims;

namespace TinyBacklog.Api
{
    public class TaskListFunction
    {
        public ITaskStore Store { get; }

        public TaskListFunction(ITaskStore store)
        {
            Store = store ?? throw new ArgumentNullException(nameof(store));
        }

        [FunctionName("TaskList")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var identity = ClientPrincipalBuilder.BuildFromHttpRequest(req);
            var userId = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
            {
                userId = "Dev-alberto";
            }

            var entities = await Store.GetAllTasks(userId);
            var tasks = entities.Select(t => t.ToTaskViewModel());

            return new OkObjectResult(tasks);
        }
    }
}
