using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniTaskApp.API.Data;
using MiniTaskApp.API.Models.DTOs.Task;
using MiniTaskApp.API.Models.DTOs.TaskItem;
using MiniTaskApp.API.Services;
using TaskEntity = MiniTaskApp.API.Models.Entities.Task;

namespace MiniTaskApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TaskService _service;
        public TasksController(AppDbContext context, TaskService service)
        {
            _context = context;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _context.Tasks
                .Include(t => t.TaskItems)
                    .ThenInclude(ti => ti.Employee)
                .ToListAsync();

            var result = tasks.Select(t => new TaskDto
            {
                TaskId = t.TaskId,
                Title = t.Title,
                Description = t.Description,
                Status = _service.ComputeStatus(t),
                Items = t.TaskItems.Select(x => new TaskItemDto
                {
                    TaskItemId = x.TaskItemId,
                    ItemName = x.ItemName,
                    Status = (int)x.Status,
                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.Employee != null ? x.Employee.FirstName + " " + x.Employee.LastName : null
                }).ToList()
            });

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var t = await _context.Tasks
                .Include(x => x.TaskItems)
                    .ThenInclude(ti => ti.Employee)
                .FirstOrDefaultAsync(x => x.TaskId == id);

            if (t == null) return NotFound();

            var dto = new TaskDto
            {
                TaskId = t.TaskId,
                Title = t.Title,
                Description = t.Description,
                Status = _service.ComputeStatus(t),
                Items = t.TaskItems.Select(x => new TaskItemDto
                {
                    TaskItemId = x.TaskItemId,
                    ItemName = x.ItemName,
                    Status = (int)x.Status,
                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.Employee != null ? x.Employee.FirstName + " " + x.Employee.LastName : null
                }).ToList()
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskInputDto dto)
        {
            bool exists = await _context.Tasks.AnyAsync(t => t.Title.ToLower() == dto.Title.Trim().ToLower());

            if (exists)
                return Conflict(new { message = "A task with this title already exists." });

            var task = new TaskEntity
            {
                Title = dto.Title,
                Description = dto.Description,
                CreatedAt = DateTime.Now
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var result = new TaskDto
            {
                TaskId = task.TaskId,
                Title = task.Title.Trim(),
                Description = task.Description.Trim(),
                Status = _service.ComputeStatus(task)
            };
            return CreatedAtAction(nameof(Get), new { id = task.TaskId }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskInputDto dto)
        {
            var t = await _context.Tasks.FindAsync(id);
            if (t == null) return NotFound();

            bool exists = await _context.Tasks.AnyAsync(x => x.TaskId != id && x.Title.ToLower() == dto.Title.Trim().ToLower());

            if (exists)
                return Conflict(new { message = "Another task with this title already exists." });

            t.Title = dto.Title.Trim();
            t.Description = dto.Description.Trim();
            t.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var t = await _context.Tasks.FindAsync(id);
            if (t == null) return NotFound();

            _context.Tasks.Remove(t);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
