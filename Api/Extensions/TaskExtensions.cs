using TinyBacklog.Core.Entities;
using TinyBacklog.Shared;

namespace TinyBacklog.Api.Extensions
{
    public static class TaskExtensions
    {
        public static TaskViewModel ToTaskViewModel(this Task entity)
        {
            return new TaskViewModel
            {
                Id = entity.Id,
                Description = entity.Description,
                Status = entity.Status.ToTaskViewModelStatus(),
                Title = entity.Title,
                User = new TaskViewModel.UserDescriptor
                {
                    UserId = entity.User.UserId,
                    UserName = entity.User.UserName
                }
            };
        }

        public static TaskViewModel.TaskStatus ToTaskViewModelStatus(this Task.TaskStatus status)
        {
            return status switch
            {
                Task.TaskStatus.Completed => TaskViewModel.TaskStatus.Completed,
                _ => TaskViewModel.TaskStatus.Open
            };
        }
    }
}
