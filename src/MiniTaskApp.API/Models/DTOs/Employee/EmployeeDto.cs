using MiniTaskApp.API.Models.DTOs.TaskItem;

namespace MiniTaskApp.API.Models.DTOs.Employee
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public List<TaskItemDto> AssignedTaskItems { get; set; } = new List<TaskItemDto>();

    }
}
