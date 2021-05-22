using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskEntity = TinyBacklog.Core.Entities.Task;

namespace TinyBacklog.Core
{
    public interface ITaskStore
    {
        Task AddNewTask(TaskEntity task);

        Task AddNewTask(Guid taskId, string taskTitle, string taskDescription, string userId, string userName);

        Task UpdateTask(TaskEntity task);

        Task DeleteTask(TaskEntity task);

        Task<IEnumerable<TaskEntity>> GetAllTasks();

        Task CompleteTask(TaskEntity task);
    }
}
