namespace MiniTaskApp.API.Models.DTOs.Employee
{
    public class EmployeeInputDto
    {
        public string EmployeeNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
