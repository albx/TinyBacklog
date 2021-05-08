using System.Collections.Generic;
using TinyBacklog.Shared;

namespace TinyBacklog.Api
{
    public static class TaskDatabase
    {
        public static List<TaskViewModel> Tasks { get; set; } = new List<TaskViewModel>();

        public static int LastId { get; set; } = 0;
    }
}
