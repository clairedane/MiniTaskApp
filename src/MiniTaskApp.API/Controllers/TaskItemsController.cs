using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniTaskApp.API.Data;
using MiniTaskApp.API.DTOs.TaskItem;
using MiniTaskApp.API.Entities;

namespace MiniTaskApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaskItemsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _context.TaskItems.FirstOrDefaultAsync(x => x.TaskItemId == id);

            if (item == null) return NotFound();

            return Ok(new TaskItemDto
            {
                TaskItemId = item.TaskItemId,
                ItemName = item.ItemName,
                Status = (int)item.Status,
                EmployeeId = item.EmployeeId
            });
        }

        [HttpPost("/api/tasks/{taskId}/items")]
        public async Task<IActionResult> AddItem(int taskId, TaskItemInputDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ItemName) || dto.EmployeeId == 0)
                return BadRequest(new { message = "Item name and employee must be provided." });

            var taskExists = await _context.Tasks.AnyAsync(t => t.TaskId == taskId);
            if (!taskExists) return NotFound("Task not found");

            if (!Enum.TryParse<TaskItemStatus>(dto.Status, out var status))
                return BadRequest(new { message = "Invalid status value." });

            bool duplicateExists = await _context.TaskItems.AnyAsync(x => x.TaskId == taskId && x.EmployeeId == dto.EmployeeId && x.ItemName.ToLower() == dto.ItemName.Trim().ToLower());

            if (duplicateExists)
                return Conflict(new { message = "A task item with this name already exists for the selected employee." });


            var item = new TaskItem
            {
                TaskId = taskId,
                ItemName = dto.ItemName,
                Status = status,
                EmployeeId = dto.EmployeeId,
                CreatedAt = DateTime.Now
            };

            _context.TaskItems.Add(item);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskItemInputDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ItemName) || dto.EmployeeId == 0 || string.IsNullOrWhiteSpace(dto.Status))
                return BadRequest(new { message = "All fields must be provided." });

            var item = await _context.TaskItems.FindAsync(id);
            if (item == null) return NotFound();

            if (!Enum.TryParse<TaskItemStatus>(dto.Status, out var status))
                return BadRequest(new { message = "Invalid status value." });

            bool duplicateExists = await _context.TaskItems
               .AnyAsync(x => x.TaskItemId != id
                              && x.TaskId == item.TaskId
                              && x.EmployeeId == dto.EmployeeId
                              && x.ItemName.ToLower() == dto.ItemName.Trim().ToLower());

            if (duplicateExists)
                return Conflict(new { message = "Another task item with this name already exists for the selected employee." });
            
            item.ItemName = dto.ItemName.Trim();
            item.Status = status;
            item.EmployeeId = dto.EmployeeId;
            item.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.TaskItems.FindAsync(id);
            if (item == null) return NotFound();

            _context.TaskItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
