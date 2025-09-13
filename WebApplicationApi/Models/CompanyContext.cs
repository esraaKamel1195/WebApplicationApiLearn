using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplicationApi.Models
{
    public class CompanyContext: IdentityDbContext<ApplicationUser>
    {
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<Employee> Employees { get; set; } = null!;

        public CompanyContext() { }
        public CompanyContext(DbContextOptions<CompanyContext> options):base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-KK73L52;Initial Catalog=testWebApi;Integrated Security=True;Pooling=False;Encrypt=True;Trust Server Certificate=True");
        }
    }
}
