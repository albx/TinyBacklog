using TinyBacklog.Core.Entities;
using TinyBacklog.Shared;

namespace TinyBacklog.Api.Extensions
{
    public static class TaskViewModelExtensions
    {
        public static Task ToTaskEntity(this TaskViewModel model)
        {
            return new Task
            {
                Id = model.Id,
                Description = model.Description,
                Title = model.Title,
                Status = model.Status.ToTaskStatus(),
                User = new Task.UserDescriptor
                {
                    UserId = model.User.UserId,
                    UserName = model.User.UserName
                }
            };
        }

        public static Task.TaskStatus ToTaskStatus(this TaskViewModel.TaskStatus status)
        {
            return status switch
            {
                TaskViewModel.TaskStatus.Completed => Task.TaskStatus.Completed,
                TaskViewModel.TaskStatus.InProgress => Task.TaskStatus.InProgress,
                _ => Task.TaskStatus.ToDo
            };
        }
    }
}
