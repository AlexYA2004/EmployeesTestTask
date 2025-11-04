using EmployeesTestTask.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeesTestTask.Data.Configuration;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.MiddleName)
            .HasMaxLength(50);

        builder.Property(e => e.DateOfBirth)
            .HasColumnType("date");

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        // Индексы для улучшения производительности
        builder.HasIndex(e => e.LastName)
            .HasDatabaseName("IX_Employees_LastName");

        builder.HasIndex(e => e.FirstName)
            .HasDatabaseName("IX_Employees_FirstName");

        builder.HasIndex(e => e.IsActive)
            .HasDatabaseName("IX_Employees_IsActive");

        // Связь с EmploymentPeriods
        builder.HasMany(e => e.EmploymentPeriods)
            .WithOne(ep => ep.Employee)
            .HasForeignKey(ep => ep.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}