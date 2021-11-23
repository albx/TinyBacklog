using System.ComponentModel.DataAnnotations;

namespace TinyBacklog.Shared;

public class TaskViewModel
{
    public Guid Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public TaskStatus Status { get; set; } = TaskStatus.ToDo;

    public UserDescriptor? User { get; set; }

    public enum TaskStatus
    {
        ToDo,
        InProgress,
        Completed
    }

    public class UserDescriptor
    {
        public string UserId { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;
    }
}
