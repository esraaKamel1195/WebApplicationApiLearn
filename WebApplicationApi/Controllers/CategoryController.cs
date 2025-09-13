using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplicationApi.Models;

namespace WebApplicationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        CompanyContext CompanyContext = new CompanyContext();

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categories = CompanyContext.Categories.ToList();
            return Ok(categories);
        }

        [HttpGet("{id:int}")]   
        public IActionResult GetCategory(int id) {
            var category = CompanyContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] Category category) {
            if (category == null || string.IsNullOrEmpty(category.Name)) {
                return BadRequest("Invalid category data.");
            }
            CompanyContext.Categories.Add(category);
            CompanyContext.SaveChanges();
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] Category updatedCategory) {
            if (updatedCategory == null || string.IsNullOrEmpty(updatedCategory.Name)) {
                return BadRequest("Invalid category data.");
            }
            var existingCategory = CompanyContext.Categories.FirstOrDefault(c => c.Id == id);
            if (existingCategory == null) {
                return NotFound();
            }
            existingCategory.Name = updatedCategory.Name;
            existingCategory.Description = updatedCategory.Description;
            CompanyContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id) {
            var category = CompanyContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) {
                return NotFound();
            }
            CompanyContext.Categories.Remove(category);
            CompanyContext.SaveChanges();
            return NoContent();
        }
    }
}
