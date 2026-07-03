using Microsoft.EntityFrameworkCore;
using PeoplePortal.Domain.Entities;

namespace PeoplePortal.Infrastructure.Persistence;

public class PeoplePortalDbContext(DbContextOptions<PeoplePortalDbContext> options) : DbContext(options)
{
    public DbSet<HrRequest> HrRequests => Set<HrRequest>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<Voucher> Vouchers => Set<Voucher>();
    public DbSet<Announcement> Announcements => Set<Announcement>();
    public DbSet<Benefit> Benefits => Set<Benefit>();

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
            entity.Property(x => x.Period).HasColumnName("period").HasMaxLength(50);
            entity.Property(x => x.Reason).HasColumnName("reason").HasMaxLength(500);
            entity.Property(x => x.HrComment).HasColumnName("hr_comment").HasMaxLength(500);
            entity.Property(x => x.ReviewedBy).HasColumnName("reviewed_by").HasMaxLength(150);
            entity.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
            entity.Property(x => x.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(x => x.EmployeeId).HasDatabaseName("ix_hr_requests_employee_id");
            entity.HasIndex(x => x.Status).HasDatabaseName("ix_hr_requests_status");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("employees");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.KeycloakId).HasColumnName("keycloak_id").HasMaxLength(150).IsRequired();
            entity.Property(x => x.Code).HasColumnName("code").HasMaxLength(50).IsRequired();
            entity.Property(x => x.FullName).HasColumnName("full_name").HasMaxLength(200).IsRequired();
            entity.Property(x => x.Email).HasColumnName("email").HasMaxLength(200).IsRequired();
            entity.Property(x => x.Phone).HasColumnName("phone").HasMaxLength(30);
            entity.Property(x => x.Department).HasColumnName("department").HasMaxLength(100).IsRequired();
            entity.Property(x => x.Position).HasColumnName("position").HasMaxLength(100).IsRequired();
            entity.Property(x => x.HireDate).HasColumnName("hire_date").IsRequired();
            entity.Property(x => x.ContractType).HasColumnName("contract_type").HasConversion<string>().HasMaxLength(20).IsRequired();
            entity.Property(x => x.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20).IsRequired();
            entity.Property(x => x.EmergencyContact).HasColumnName("emergency_contact").HasMaxLength(200);
            entity.Property(x => x.Site).HasColumnName("site").HasMaxLength(100);
            entity.Property(x => x.ManagerId).HasColumnName("manager_id").HasMaxLength(150);
            entity.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
            entity.Property(x => x.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(x => x.KeycloakId).IsUnique().HasDatabaseName("ix_employees_keycloak_id");
            entity.HasIndex(x => x.Code).IsUnique().HasDatabaseName("ix_employees_code");
            entity.HasIndex(x => x.Email).HasDatabaseName("ix_employees_email");
            entity.HasIndex(x => x.ManagerId).HasDatabaseName("ix_employees_manager_id");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.ToTable("documents");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.EmployeeId).HasColumnName("employee_id").HasMaxLength(150).IsRequired();
            entity.Property(x => x.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
            entity.Property(x => x.Type).HasColumnName("type").HasMaxLength(100).IsRequired();
            entity.Property(x => x.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20).IsRequired();
            entity.Property(x => x.FileUrl).HasColumnName("file_url").HasMaxLength(500);
            entity.Property(x => x.ExpiresAt).HasColumnName("expires_at");
            entity.Property(x => x.UploadedAt).HasColumnName("uploaded_at").IsRequired();
            entity.Property(x => x.ReviewedBy).HasColumnName("reviewed_by").HasMaxLength(150);

            entity.HasIndex(x => x.EmployeeId).HasDatabaseName("ix_documents_employee_id");
            entity.HasIndex(x => x.Status).HasDatabaseName("ix_documents_status");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.ToTable("vouchers");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.EmployeeId).HasColumnName("employee_id").HasMaxLength(150).IsRequired();
            entity.Property(x => x.Period).HasColumnName("period").HasMaxLength(50).IsRequired();
            entity.Property(x => x.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(30).IsRequired();
            entity.Property(x => x.FileUrl).HasColumnName("file_url").HasMaxLength(500);
            entity.Property(x => x.Reason).HasColumnName("reason").HasMaxLength(500);
            entity.Property(x => x.RequestedAt).HasColumnName("requested_at").IsRequired();
            entity.Property(x => x.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(x => x.EmployeeId).HasDatabaseName("ix_vouchers_employee_id");
            entity.HasIndex(x => x.Status).HasDatabaseName("ix_vouchers_status");
        });

        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.ToTable("announcements");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.Title).HasColumnName("title").HasMaxLength(200).IsRequired();
            entity.Property(x => x.Body).HasColumnName("body").HasColumnType("text").IsRequired();
            entity.Property(x => x.Type).HasColumnName("type").HasConversion<string>().HasMaxLength(30).IsRequired();
            entity.Property(x => x.PublishedAt).HasColumnName("published_at").IsRequired();
            entity.Property(x => x.ExpiresAt).HasColumnName("expires_at");
            entity.Property(x => x.CreatedBy).HasColumnName("created_by").HasMaxLength(150).IsRequired();
            entity.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();

            entity.HasIndex(x => x.IsActive).HasDatabaseName("ix_announcements_is_active");
        });

        modelBuilder.Entity<Benefit>(entity =>
        {
            entity.ToTable("benefits");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
            entity.Property(x => x.Description).HasColumnName("description").HasMaxLength(500);
            entity.Property(x => x.Type).HasColumnName("type").HasMaxLength(100).IsRequired();
            entity.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();

            entity.HasIndex(x => x.IsActive).HasDatabaseName("ix_benefits_is_active");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.ToTable("vouchers");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.EmployeeId).HasColumnName("employee_id").HasMaxLength(150).IsRequired();
            entity.Property(x => x.Period).HasColumnName("period").HasMaxLength(50).IsRequired();
            entity.Property(x => x.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(30).IsRequired();
            entity.Property(x => x.FileUrl).HasColumnName("file_url").HasMaxLength(500);
            entity.Property(x => x.Reason).HasColumnName("reason").HasMaxLength(500);
            entity.Property(x => x.RequestedAt).HasColumnName("requested_at").IsRequired();
            entity.Property(x => x.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(x => x.EmployeeId).HasDatabaseName("ix_vouchers_employee_id");
            entity.HasIndex(x => x.Status).HasDatabaseName("ix_vouchers_status");
        });
    }
}