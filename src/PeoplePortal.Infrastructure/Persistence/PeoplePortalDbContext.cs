using Microsoft.EntityFrameworkCore;
using PeoplePortal.Domain.Entities;

namespace PeoplePortal.Infrastructure.Persistence;

public class PeoplePortalDbContext(DbContextOptions<PeoplePortalDbContext> options) : DbContext(options)
{
    public DbSet<HrRequest> HrRequests => Set<HrRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HrRequest>(entity =>
        {
            entity.ToTable("hr_requests");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.EmployeeId).HasColumnName("employee_id").HasMaxLength(150).IsRequired();
            entity.Property(x => x.Type).HasColumnName("type").HasConversion<string>().HasMaxLength(30).IsRequired();
            entity.Property(x => x.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(30).IsRequired();
            entity.Property(x => x.VacationStartDate).HasColumnName("vacation_start_date");
            entity.Property(x => x.VacationEndDate).HasColumnName("vacation_end_date");
            entity.Property(x => x.CertificateType).HasColumnName("certificate_type").HasMaxLength(120);
            entity.Property(x => x.Reason).HasColumnName("reason").HasMaxLength(500);
            entity.Property(x => x.HrComment).HasColumnName("hr_comment").HasMaxLength(500);
            entity.Property(x => x.ReviewedBy).HasColumnName("reviewed_by").HasMaxLength(150);
            entity.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
            entity.Property(x => x.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(x => x.EmployeeId).HasDatabaseName("ix_hr_requests_employee_id");
            entity.HasIndex(x => x.Status).HasDatabaseName("ix_hr_requests_status");
        });
    }
}