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

namespace TinyBacklog.Api
{
    public static class CreateTaskFunction
    {
        [FunctionName("CreateTask")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            using var reader = new StreamReader(req.Body);
            string requestBody = await reader.ReadToEndAsync();

            var task = JsonConvert.DeserializeObject<TaskViewModel>(requestBody);
            task.Id = ++TaskDatabase.LastId;

            TaskDatabase.Tasks.Add(task);

            return new OkObjectResult(task);
        }
    }
}
