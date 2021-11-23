using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TinyBacklog.Api;

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
            var identity = ClientPrincipalBuilder.BuildFromHttpRequest(req);
            var userId = identity.GetUserId();

            await Store.DeleteTask(id, userId);
            return new NoContentResult();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            log.LogError(ex, "Task with id {TaskId} not found: {ErrorMessage}", id, ex.Message);
            return new BadRequestObjectResult("Task not found");
        }
    }
}
