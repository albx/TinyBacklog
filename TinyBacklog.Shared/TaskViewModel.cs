using System.ComponentModel.DataAnnotations;

namespace TinyBacklog.Shared
{
    public class TaskViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public TaskStatus Status { get; set; } = TaskStatus.Open;

        public enum TaskStatus
        {
            Open,
            Completed
        }
    }
}
