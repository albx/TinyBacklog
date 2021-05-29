using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskEntity = TinyBacklog.Core.Entities.Task;

namespace TinyBacklog.Core
{
    public interface ITaskStore
    {
        Task AddNewTask(TaskEntity task);

        Task UpdateTask(TaskEntity task);

        Task DeleteTask(Guid taskId);

        Task<IEnumerable<TaskEntity>> GetAllTasks(string userId);

        Task UpdateTaskStatus(TaskEntity task, TaskEntity.TaskStatus status);
    }
}
