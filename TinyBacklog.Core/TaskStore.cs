using Azure.Data.Tables;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyBacklog.Core.Options;

namespace TinyBacklog.Core
{
    public class TaskStore : ITaskStore
    {
        private readonly TaskStoreOptions _options;

        private readonly TableClient _client;

        public TaskStore(IOptions<TaskStoreOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _client = new TableClient(_options.ConnectionString, _options.TableName);

            _client.CreateIfNotExists();
        }

        public Task AddNewTask(Entities.Task task)
        {
            var entity = task.ToTableEntity();
            return _client.AddEntityAsync(entity);
        }

        public Task AddNewTask(Guid taskId, string taskTitle, string taskDescription, string userId, string userName)
        {
            var task = new Entities.Task
            {
                Id = taskId,
                Title = taskTitle,
                Description = taskDescription,
                Status = Entities.Task.TaskStatus.Open,
                User = new Entities.Task.UserDescriptor
                {
                    UserId = userId,
                    UserName = userName
                }
            };

            return AddNewTask(task);
        }

        public Task CompleteTask(Entities.Task task)
        {
            task.Status = Entities.Task.TaskStatus.Completed;
            var entity = task.ToTableEntity();

            return _client.UpsertEntityAsync(entity);
        }

        public Task DeleteTask(Entities.Task task)
        {
            var entity = task.ToTableEntity();
            return _client.DeleteEntityAsync(entity.PartitionKey, entity.RowKey);
        }

        public Task<IEnumerable<Entities.Task>> GetAllTasks()
        {
            var entities = _client.Query<TableEntity>();
            var tasks = entities.Select(e => e.ToTaskEntity());

            return Task.FromResult(tasks);
        }

        public Task UpdateTask(Entities.Task task)
        {
            var entity = task.ToTableEntity();
            return _client.UpsertEntityAsync(entity);
        }
    }
}
