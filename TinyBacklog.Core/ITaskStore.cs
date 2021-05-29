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

        Task DeleteTask(TaskEntity task);

        Task<IEnumerable<TaskEntity>> GetAllTasks(string userId);

        Task CompleteTask(TaskEntity task);

        Task StartTask(TaskEntity task);
    }
}
