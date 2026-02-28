namespace MiniTaskApp.API.Models.DTOs.TaskItem
{
    public class TaskItemInputDto
    {
        public string ItemName { get; set; }
        public string Status { get; set; } 
        public int? EmployeeId { get; set; }
    }
}
