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

            using var reader = new StreamReader(req.Body);
            string requestBody = await reader.ReadToEndAsync();

            var task = JsonConvert.DeserializeObject<TaskViewModel>(requestBody);
            task.Id = Guid.NewGuid();

            task.User = new TaskViewModel.UserDescriptor
            {
                UserId = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                UserName = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value
            };

            var userId = string.IsNullOrWhiteSpace(task.User?.UserId) ? "Dev-alberto" : task.User.UserId;
            var userName = string.IsNullOrWhiteSpace(task.User?.UserName) ? "albx" : task.User.UserName;

            await Store.AddNewTask(
                task.Id,
                task.Title,
                task.Description,
                userId,
                userName);

            return new OkObjectResult(task);
        }
    }
}
