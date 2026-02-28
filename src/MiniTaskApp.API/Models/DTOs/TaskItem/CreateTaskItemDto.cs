namespace MiniTaskApp.API.Models.DTOs.TaskItem
{
    public class CreateTaskItemDto
    {
        public string ItemName { get; set; }
        public int? EmployeeId { get; set; }
    }
}
