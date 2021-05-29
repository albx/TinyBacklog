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
using System.ComponentModel.DataAnnotations;

namespace TinyBacklog.Api
{
    public class DeleteTaskFunction
    {
        public ITaskStore Store { get; }

        public DeleteTaskFunction(ITaskStore store)
        {
            Store = store ?? throw new ArgumentNullException(nameof(store));
        }

        [FunctionName("DeleteTask")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "DeleteTask/{id}")] HttpRequest req,
            [Required] Guid id,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (id == Guid.Empty)
            {
                return new BadRequestObjectResult("Id cannot be empty");
            }

            try
            {
                await Store.DeleteTask(id);
                return new NoContentResult();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                log.LogError(ex, "Task with id {TaskId} not found: {ErrorMessage}", id, ex.Message);
                return new BadRequestObjectResult("Task not found");
            }
        }
    }
}
