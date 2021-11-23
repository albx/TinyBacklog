using TaskEntity = TinyBacklog.Core.Entities.Task;

namespace TinyBacklog.Core;

public interface ITaskStore
{
    Task AddNewTask(TaskEntity task);

    Task UpdateTask(TaskEntity task, string userId);

    Task DeleteTask(Guid taskId, string userId);

    Task<IEnumerable<TaskEntity>> GetAllTasks(string userId);

    Task UpdateTaskStatus(Guid taskId, TaskEntity.TaskStatus status, string userId);
}
