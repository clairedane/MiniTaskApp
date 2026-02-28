using MiniTaskApp.API.Models.DTOs.TaskItem;

namespace MiniTaskApp.API.Models.DTOs.Task
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