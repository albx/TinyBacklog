namespace TinyBacklog.Api;

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

        var identity = ClientPrincipalBuilder.BuildFromHttpRequest(req);
        var userId = identity.GetUserId();

        var task = await req.ParseRequestBodyAsync<TaskViewModel>();
        try
        {
            var entity = task.ToTaskEntity();

            await Store.UpdateTask(entity, userId);

            return new NoContentResult();
        }
        catch (ArgumentException ex)
        {
            log.LogError(ex, "Task with id {TaskId} not found: {ErrorMessage}", task.Id, ex.Message);
            return new BadRequestObjectResult(ex.Message);
        }
    }
}
