using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplicationApi.Models;

namespace WebApplicationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        CompanyContext Company = new CompanyContext();

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var employees = Company.Employees.ToList();
            return Ok(employees);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetEmployeeById(int id)
        {
            var employee = Company.Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            if (employee == null || string.IsNullOrEmpty(employee.FirstName) || string.IsNullOrEmpty(employee.LastName))
            {
                return BadRequest("Invalid employee data.");
            }
            Company.Employees.Add(employee);
            Company.SaveChanges();
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
        }

        /*[HttpPost]
        public IActionResult CreateEmployees([FromBody] List<Employee> employees)
        {
            if (employees == null || !employees.Any())
            {
                return BadRequest("Invalid employee data.");
            }
            Company.Employees.AddRange(employees);
            Company.SaveChanges();
            return CreatedAtAction(nameof(GetAllEmployees), employees);
        }*/

        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, [FromBody] Employee updatedEmployee)
        {
            if (updatedEmployee == null || string.IsNullOrEmpty(updatedEmployee.FirstName) || string.IsNullOrEmpty(updatedEmployee.LastName))
            {
                return BadRequest("Invalid employee data.");
            }
            var existingEmployee = Company.Employees.FirstOrDefault(e => e.Id == id);
            if (existingEmployee == null)
            {
                return NotFound();
            }
            existingEmployee.FirstName = updatedEmployee.FirstName;
            existingEmployee.LastName = updatedEmployee.LastName;
            existingEmployee.Email = updatedEmployee.Email;
            existingEmployee.HireDate = updatedEmployee.HireDate;
            existingEmployee.Address = updatedEmployee.Address;
            existingEmployee.DepartmentId = updatedEmployee.DepartmentId;
            Company.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id) {
            var employee = Company.Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null) {
                return NotFound();
            }
            Company.Employees.Remove(employee);
            Company.SaveChanges();
            return NoContent();
        }
    }
}
