namespace MiniTaskApp.API.DTOs.TaskItem
{
    public class CreateTaskItemDto
    {
        public string ItemName { get; set; }
        public int? EmployeeId { get; set; }
    }
}
