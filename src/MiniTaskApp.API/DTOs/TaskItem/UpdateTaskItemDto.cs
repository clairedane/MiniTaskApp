namespace MiniTaskApp.API.DTOs.TaskItem
{
    public class UpdateTaskItemDto
    {
        public string ItemName { get; set; }
        public string Status { get; set; } 
        public int? EmployeeId { get; set; }
    }
}
