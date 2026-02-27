using MiniTaskApp.API.DTOs.TaskItem;

namespace MiniTaskApp.API.DTOs.Task
{
    public class TaskDto
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public List<TaskItemDto> Items { get; set; } = new List<TaskItemDto>();
    }
}