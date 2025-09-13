using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplicationApi.Models;

namespace WebApplicationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        CompanyContext Company = new CompanyContext();

        [HttpGet]
        public IActionResult GetAllDepartments()
        {
            var departments = Company.Departments.ToList();
            return Ok(departments);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetDepartmentById(int id)
        {
            var department = Company.Departments.FirstOrDefault(d => d.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }

        [HttpPost]
        public IActionResult CreateDepartment([FromBody] Department department)
        {
            if (department == null || string.IsNullOrEmpty(department.Name))
            {
                return BadRequest("Invalid department data.");
            }
            Company.Departments.Add(department);
            Company.SaveChanges();
            return CreatedAtAction(nameof(GetDepartmentById), new { id = department.Id }, department);
        }

        /*
        [HttpPost]
        public IActionResult CreateDepartments([FromBody] List<Department> departments)
        {
            if (departments == null || !departments.Any())
            {
                return BadRequest("Invalid department data.");
            }
            Company.Departments.AddRange(departments);
            Company.SaveChanges();
            return CreatedAtAction(nameof(GetAllDepartments), departments);
        }
        */
        [HttpPut("{id}")]
        public IActionResult UpdateDepartment(int id, [FromBody] Department updatedDepartment)
        {
            if (updatedDepartment == null || string.IsNullOrEmpty(updatedDepartment.Name))
            {
                return BadRequest("Invalid department data.");
            }
            var existingDepartment = Company.Departments.FirstOrDefault(d => d.Id == id);
            if (existingDepartment == null)
            {
                return NotFound();
            }
            existingDepartment.Name = updatedDepartment.Name;
            existingDepartment.ManagerName = updatedDepartment.ManagerName;
            Company.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDepartment(int id) {
            var department = Company.Departments.FirstOrDefault(d => d.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            Company.Departments.Remove(department);
            Company.SaveChanges();
            return NoContent();
        }
    }
}
