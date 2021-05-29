using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TinyBacklog.Shared;
using System.Linq;
using System.Security.Claims;
using TinyBacklog.Core;
using TinyBacklog.Api.Extensions;

namespace TinyBacklog.Api
{
    public class CreateTaskFunction
    {
        public ITaskStore Store { get; }

        public CreateTaskFunction(ITaskStore store)
        {
            Store = store ?? throw new ArgumentNullException(nameof(store));
        }

        [FunctionName("CreateTask")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var identity = ClientPrincipalBuilder.BuildFromHttpRequest(req);

            var task = await req.ParseRequestBodyAsync<TaskViewModel>();
            task.Id = Guid.NewGuid();

            var userId = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userName = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            task.User = new TaskViewModel.UserDescriptor
            {
                UserId = string.IsNullOrWhiteSpace(userId) ? "Dev-alberto" : userId,
                UserName = string.IsNullOrWhiteSpace(userName) ? "albx" : userName
            };

            var entity = task.ToTaskEntity();

            await Store.AddNewTask(entity);

            return new OkObjectResult(task);
        }
    }
}
