namespace TinyBacklog.Api;

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

        var userId = identity.GetUserId();
        var userName = identity.GetUserName();

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
