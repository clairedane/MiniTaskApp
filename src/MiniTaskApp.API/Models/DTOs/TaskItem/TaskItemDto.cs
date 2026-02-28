namespace MiniTaskApp.API.Models.DTOs.TaskItem
{
    public class TaskItemDto
    {
        public int TaskItemId { get; set; }
        public string ItemName { get; set; }
        public int Status { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }
}
