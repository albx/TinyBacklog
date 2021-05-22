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
using System.Linq;
using TinyBacklog.Shared;

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

            var entities = await Store.GetAllTasks();
            var tasks = entities
                .Select(t => new TaskViewModel
                {
                    Id = t.Id,
                    Description = t.Description,
                    Status = ConvertStatus(t.Status),
                    Title = t.Title,
                    User = ConvertUser(t.User)
                });

            return new OkObjectResult(tasks);
        }

        private TaskViewModel.UserDescriptor ConvertUser(Core.Entities.Task.UserDescriptor user)
        {
            return new TaskViewModel.UserDescriptor
            {
                UserId = user.UserId,
                UserName = user.UserName
            };
        }

        private TaskViewModel.TaskStatus ConvertStatus(Core.Entities.Task.TaskStatus status)
        {
            return status switch
            {
                Core.Entities.Task.TaskStatus.Completed => TaskViewModel.TaskStatus.Completed,
                _ => TaskViewModel.TaskStatus.Open
            };
        }
    }
}
