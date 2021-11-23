namespace TinyBacklog.Api;

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
        var userId = identity.GetUserId();

        var entities = await Store.GetAllTasks(userId);
        var tasks = entities.Select(t => t.ToTaskViewModel());

        return new OkObjectResult(tasks);
    }
}
