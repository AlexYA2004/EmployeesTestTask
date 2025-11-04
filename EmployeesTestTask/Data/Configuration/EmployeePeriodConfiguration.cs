using EmployeesTestTask.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeesTestTask.Data.Configuration;

public class EmployeePeriodConfiguration : IEntityTypeConfiguration<EmploymentPeriod>
{
    public void Configure(EntityTypeBuilder<EmploymentPeriod> builder)
    {
        builder.ToTable("EmploymentPeriods");

        builder.HasKey(ep => ep.Id);

        builder.Property(ep => ep.StartDate)
            .IsRequired()
            .HasColumnType("date");

        builder.Property(ep => ep.EndDate)
            .HasColumnType("date");

        builder.Property(ep => ep.Position)
            .HasMaxLength(100);

        builder.Property(ep => ep.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        // Индексы для улучшения производительности
        builder.HasIndex(ep => ep.EmployeeId)
            .HasDatabaseName("IX_EmploymentPeriods_EmployeeId");

        builder.HasIndex(ep => ep.StartDate)
            .HasDatabaseName("IX_EmploymentPeriods_StartDate");

        // Проверка на корректность дат
        builder.HasCheckConstraint("CK_EmploymentPeriods_Dates", 
            "[EndDate] IS NULL OR [EndDate] >= [StartDate]");
    }
}