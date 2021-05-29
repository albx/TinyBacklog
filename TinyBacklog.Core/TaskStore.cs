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

        public Task DeleteTask(Guid taskId)
        {
            var entity = _client.Query<TableEntity>(t => t[nameof(Entities.Task.Id)].Equals(taskId))
                .FirstOrDefault();

            if (entity is null)
            {
                throw new ArgumentOutOfRangeException(nameof(taskId));
            }

            var response = _client.DeleteEntity(entity.PartitionKey, entity.RowKey, entity.ETag);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Entities.Task>> GetAllTasks(string userId)
        {
            var entities = _client.Query<TableEntity>(t => t[nameof(Entities.Task.UserDescriptor.UserId)].Equals(userId));
            var tasks = entities.Select(e => e.ToTaskEntity());

            return Task.FromResult(tasks);
        }

        public Task UpdateTask(Entities.Task task)
        {
            var entity = task.ToTableEntity();
            return _client.UpsertEntityAsync(entity);
        }

        public Task UpdateTaskStatus(Entities.Task task, Entities.Task.TaskStatus status)
        {
            task.Status = status;
            var entity = task.ToTableEntity();

            return _client.UpsertEntityAsync(entity);
        }
    }
}
