namespace TinyBacklog.Api;

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
            var identity = ClientPrincipalBuilder.BuildFromHttpRequest(req);
            var userId = identity.GetUserId();

            var status = model.Status.ToTaskStatus();
            await Store.UpdateTaskStatus(model.TaskId, status, userId);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            log.LogError(ex, "Task with id {TaskId} not found: {ErrorMessage}", model.TaskId, ex.Message);
            return new BadRequestObjectResult("Task not found");
        }

        return new NoContentResult();
    }
}
