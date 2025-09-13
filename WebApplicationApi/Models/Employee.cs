using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationApi.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime? HireDate { get; set; }

        public string ? Address { get; set; } = string.Empty;

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
