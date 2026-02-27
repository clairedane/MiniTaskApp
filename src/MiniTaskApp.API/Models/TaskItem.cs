namespace MiniTaskApp.API.Entities
{
    public class TaskItem
    {
        public int TaskItemId { get; set; }

        public int TaskId { get; set; }
        public virtual Task Task { get; set; }

        public string ItemName { get; set; }
        public TaskItemStatus Status { get; set; }

        public int? EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
