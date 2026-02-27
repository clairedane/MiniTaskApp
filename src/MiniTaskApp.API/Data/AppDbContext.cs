using Microsoft.EntityFrameworkCore;
using MiniTaskApp.API.Entities;
using TaskEntity = MiniTaskApp.API.Entities.Task;

namespace MiniTaskApp.API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}
