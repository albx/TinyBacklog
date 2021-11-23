namespace TinyBacklog.Core.Entities;

public class Task
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public TaskStatus Status { get; set; }

    public UserDescriptor User { get; set; }

    public enum TaskStatus
    {
        ToDo = 0,
        Completed = 1,
        InProgress = 2
    }

    public class UserDescriptor
    {
        public string UserId { get; set; }

        public string UserName { get; set; }
    }
}
