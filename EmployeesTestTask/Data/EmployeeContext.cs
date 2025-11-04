using EmployeesTestTask.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeesTestTask.Data;

public class EmployeeContext : DbContext
{
    public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmploymentPeriod> EmploymentPeriods { get; set; }
}