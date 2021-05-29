using System;

namespace TinyBacklog.Shared
{
    public class UpdateTaskStatusViewModel
    {
        public Guid TaskId { get; set; }

        public TaskViewModel.TaskStatus Status { get; set; }
    }
}
