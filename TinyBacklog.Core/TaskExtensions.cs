using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Text;
using TinyBacklog.Core.Entities;

namespace TinyBacklog.Core
{
    public static class TaskExtensions
    {
        public static TableEntity ToTableEntity(this Task task)
        {
            var taskId = task.Id.ToString();
            return new TableEntity(taskId, taskId)
            {
                { nameof(Task.Id), task.Id },
                { nameof(Task.Title), task.Title },
                { nameof(Task.Description), task.Description },
                { nameof(Task.Status), (int)task.Status },
                { nameof(Task.UserDescriptor.UserId), task.User?.UserId },
                { nameof(Task.UserDescriptor.UserName), task.User?.UserName }
            };
        }

        public static Task ToTaskEntity(this TableEntity entity)
        {
            return new Task
            {
                Id = Guid.Parse(entity[nameof(Task.Id)].ToString()),
                Title = entity[nameof(Task.Title)]?.ToString(),
                Description = entity[nameof(Task.Description)]?.ToString(),
                User = new Task.UserDescriptor
                {
                    UserId = entity[nameof(Task.UserDescriptor.UserId)]?.ToString(),
                    UserName = entity[nameof(Task.UserDescriptor.UserName)]?.ToString()
                },
                Status = (Task.TaskStatus)Enum.Parse(typeof(Task.TaskStatus), entity[nameof(Task.Status)].ToString())
            };
        }
    }
}
