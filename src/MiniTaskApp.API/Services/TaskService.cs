using MiniTaskApp.API.Entities;
using TaskEntity = MiniTaskApp.API.Entities.Task;

namespace MiniTaskApp.API.Services
{
    public class TaskService
    {
        public string ComputeStatus(TaskEntity task)
        {
            if (task.TaskItems == null || !task.TaskItems.Any())
                return "Empty";

            if (task.TaskItems.All(x => x.Status == TaskItemStatus.New))
                return "New";

            if (task.TaskItems.All(x => x.Status == TaskItemStatus.Done))
                return "Done";

            if (task.TaskItems.Any(x => x.Status == TaskItemStatus.InProgress))
                return "In Progress";

            return "In Progress";
        }
    }
}
