using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniTaskApp.API.Data;
using MiniTaskApp.API.DTOs.Employee;
using MiniTaskApp.API.DTOs.TaskItem;
using MiniTaskApp.API.Entities;

namespace MiniTaskApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _context.Employees
                .Include(e => e.TaskItems)
                .ToListAsync();

            var result = employees.Select(e => new EmployeeDto
            {
                EmployeeId = e.EmployeeId,
                EmployeeNo = e.EmployeeNo,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                IsActive = e.IsActive
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var e = await _context.Employees
                .Include(emp => emp.TaskItems)
                .FirstOrDefaultAsync(emp => emp.EmployeeId == id);

            if (e == null) return NotFound();

            var dto = new EmployeeDto
            {
                EmployeeId = e.EmployeeId,
                EmployeeNo = e.EmployeeNo,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                IsActive = e.IsActive,
                AssignedTaskItems = e.TaskItems.Select(ti => new TaskItemDto
                {
                    TaskItemId = ti.TaskItemId,
                    ItemName = ti.ItemName,
                    Status = (int)ti.Status,
                    EmployeeId = ti.EmployeeId
                }).ToList()
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeInputDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.EmployeeNo) ||
               string.IsNullOrWhiteSpace(dto.FirstName) ||
               string.IsNullOrWhiteSpace(dto.LastName) ||
               string.IsNullOrWhiteSpace(dto.Email))
            {
                return BadRequest(new { message = "All fields are required." });
            }

            bool exists = await _context.Employees.AnyAsync(e => e.EmployeeNo.ToLower() == dto.EmployeeNo.Trim().ToLower() || e.Email.ToLower() == dto.Email.Trim().ToLower());

            if (exists)
                return Conflict(new { message = "Employee with the same EmployeeNo or Email already exists." });


            var e = new Employee
            {
                EmployeeNo = dto.EmployeeNo.Trim(),
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                Email = dto.Email.Trim(),
                IsActive = dto.IsActive,
                CreatedAt = DateTime.Now
            };

            _context.Employees.Add(e);
            await _context.SaveChangesAsync();

            var result = new EmployeeDto
            {
                EmployeeId = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName
            };

            return CreatedAtAction(nameof(Get), new { id = e.EmployeeId }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, EmployeeDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.EmployeeNo) ||
                string.IsNullOrWhiteSpace(dto.FirstName) ||
                string.IsNullOrWhiteSpace(dto.LastName) ||
                string.IsNullOrWhiteSpace(dto.Email))
            {
                return BadRequest(new { message = "All fields are required." });
            }


            var e = await _context.Employees.FindAsync(id);
            if (e == null) return NotFound();

            bool exists = await _context.Employees.AnyAsync(x => x.EmployeeId != id && (x.EmployeeNo.ToLower() == dto.EmployeeNo.Trim().ToLower() || x.Email.ToLower() == dto.Email.Trim().ToLower()));

            if (exists)
                return Conflict(new { message = "Another employee with the same EmployeeNo or Email already exists." });

            e.EmployeeNo = dto.EmployeeNo.Trim();
            e.FirstName = dto.FirstName.Trim();
            e.LastName = dto.LastName.Trim();
            e.Email = dto.Email.Trim();
            e.IsActive = dto.IsActive;
            e.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var e = await _context.Employees
                .Include(x => x.TaskItems)
                .FirstOrDefaultAsync(x => x.EmployeeId == id);

            if (e == null) return NotFound();

            foreach (var item in e.TaskItems)
            {
                item.EmployeeId = null;
            }

            _context.Employees.Remove(e);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}