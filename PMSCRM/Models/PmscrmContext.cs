using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PMSCRM.Models;

public partial class PmscrmContext : DbContext
{
    public PmscrmContext()
    {
    }

    public PmscrmContext(DbContextOptions<PmscrmContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<CommunicationLog> CommunicationLogs { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Email> Emails { get; set; }

    public virtual DbSet<PhoneCall> PhoneCalls { get; set; }

    public virtual DbSet<Process> Processes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TaskProcessArea> TaskProcessAreas { get; set; }

    public virtual DbSet<TaskProcessAreaUserCustomer> TaskProcessAreaUserCustomers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Application>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Application");

            entity.Property(e => e.ApplicationId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("application_id");
            entity.Property(e => e.Data).HasColumnName("data");
        });

        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.AreaId).HasName("PK__Area__985D6D6BE6F5CCBB");

            entity.ToTable("Area");

            entity.Property(e => e.AreaId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("area_id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime")
                .HasColumnName("timestamp");

            entity.HasOne(d => d.Company).WithMany(p => p.Areas)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Area__company_id__59063A47");
        });

        modelBuilder.Entity<CommunicationLog>(entity =>
        {
            entity.HasKey(e => e.CommunicationLogId).HasName("PK__Communic__6DB710724D8EE647");

            entity.ToTable("Communication_Log");

            entity.Property(e => e.CommunicationLogId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("communication_log_id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.LogDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("log_date");
            entity.Property(e => e.Notes)
                .HasMaxLength(255)
                .HasColumnName("notes");
            entity.Property(e => e.PhoneCallId).HasColumnName("phone_call_id");
            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime")
                .HasColumnName("timestamp");

            entity.HasOne(d => d.Company).WithMany(p => p.CommunicationLogs)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Communica__compa__0E6E26BF");

            entity.HasOne(d => d.Customer).WithMany(p => p.CommunicationLogs)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Communica__custo__0A9D95DB");

            entity.HasOne(d => d.Email).WithMany(p => p.CommunicationLogs)
                .HasForeignKey(d => d.EmailId)
                .HasConstraintName("FK__Communica__email__0C85DE4D");

            entity.HasOne(d => d.PhoneCall).WithMany(p => p.CommunicationLogs)
                .HasForeignKey(d => d.PhoneCallId)
                .HasConstraintName("FK__Communica__phone__0D7A0286");

            entity.HasOne(d => d.Task).WithMany(p => p.CommunicationLogs)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Communica__task___0B91BA14");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK__Company__3E267235BF4778DE");

            entity.ToTable("Company");

            entity.Property(e => e.CompanyId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("company_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime")
                .HasColumnName("timestamp");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__CD65CB858D1E2325");

            entity.ToTable("Customer");

            entity.Property(e => e.CustomerId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("customer_id");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .HasColumnName("country");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(100)
                .HasColumnName("email_address");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(100)
                .HasColumnName("phone_number");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(20)
                .HasColumnName("postal_code");
            entity.Property(e => e.StateOrProvince)
                .HasMaxLength(100)
                .HasColumnName("state_or_province");
            entity.Property(e => e.StreetAddress)
                .HasMaxLength(100)
                .HasColumnName("street_address");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime")
                .HasColumnName("timestamp");

            entity.HasOne(d => d.Company).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Customer__compan__656C112C");
        });

        modelBuilder.Entity<Email>(entity =>
        {
            entity.HasKey(e => e.EmailId).HasName("PK__Email__3FEF87661A87C9F7");

            entity.ToTable("Email");

            entity.Property(e => e.EmailId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("email_id");
            entity.Property(e => e.ApiResponse).HasColumnName("api_response");
            entity.Property(e => e.Body).HasColumnName("body");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Received).HasColumnName("received");
            entity.Property(e => e.Recipient)
                .HasMaxLength(255)
                .HasColumnName("recipient");
            entity.Property(e => e.Sender)
                .HasMaxLength(255)
                .HasColumnName("sender");
            entity.Property(e => e.SentDate)
                .HasColumnType("datetime")
                .HasColumnName("sent_date");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Subject)
                .HasMaxLength(255)
                .HasColumnName("subject");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime")
                .HasColumnName("timestamp");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Company).WithMany(p => p.Emails)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Email__company_i__7E37BEF6");

            entity.HasOne(d => d.Customer).WithMany(p => p.Emails)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Email__customer___7C4F7684");

            entity.HasOne(d => d.User).WithMany(p => p.Emails)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Email__user_id__7D439ABD");
        });

        modelBuilder.Entity<PhoneCall>(entity =>
        {
            entity.HasKey(e => e.PhoneCallId).HasName("PK__Phone_Ca__CB56F7BD19353EA6");

            entity.ToTable("Phone_Call");

            entity.Property(e => e.PhoneCallId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("phone_call_id");
            entity.Property(e => e.ApiResponse)
                .IsUnicode(false)
                .HasColumnName("api_response");
            entity.Property(e => e.CallType).HasColumnName("call_type");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("start_time");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime")
                .HasColumnName("timestamp");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Company).WithMany(p => p.PhoneCalls)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Phone_Cal__compa__04E4BC85");

            entity.HasOne(d => d.Customer).WithMany(p => p.PhoneCalls)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Phone_Cal__custo__02FC7413");

            entity.HasOne(d => d.User).WithMany(p => p.PhoneCalls)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Phone_Cal__user___03F0984C");
        });

        modelBuilder.Entity<Process>(entity =>
        {
            entity.HasKey(e => e.ProcessId).HasName("PK__Process__9446C3E11DFED589");

            entity.ToTable("Process");

            entity.Property(e => e.ProcessId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("process_id");
            entity.Property(e => e.AreaId).HasColumnName("area_id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime")
                .HasColumnName("timestamp");

            entity.HasOne(d => d.Area).WithMany(p => p.Processes)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK_Area_Process");

            entity.HasOne(d => d.Company).WithMany(p => p.Processes)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Process__company__4F7CD00D");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__760965CC2C29A195");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("role_id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime")
                .HasColumnName("timestamp");

            entity.HasOne(d => d.Company).WithMany(p => p.Roles)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Role__company_id__6A30C649");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__Task__0492148D2DD46EA4");

            entity.ToTable("Task");

            entity.Property(e => e.TaskId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("task_id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime")
                .HasColumnName("timestamp");

            entity.HasOne(d => d.Company).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Task__company_id__5441852A");
        });

        modelBuilder.Entity<TaskProcessArea>(entity =>
        {
            entity.HasKey(e => e.TaskProcessAreaId).HasName("PK__Task_Pro__42ABA6518838CF42");

            entity.ToTable("Task_Process_Area");

            entity.Property(e => e.TaskProcessAreaId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("task_process_area_id");
            entity.Property(e => e.AreaId).HasColumnName("area_id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.ProcessId).HasColumnName("process_id");
            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime")
                .HasColumnName("timestamp");

            entity.HasOne(d => d.Area).WithMany(p => p.TaskProcessAreas)
                .HasForeignKey(d => d.AreaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Task_Proc__area___5FB337D6");

            entity.HasOne(d => d.Company).WithMany(p => p.TaskProcessAreas)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Task_Proc__compa__60A75C0F");

            entity.HasOne(d => d.Process).WithMany(p => p.TaskProcessAreas)
                .HasForeignKey(d => d.ProcessId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Task_Proc__proce__5DCAEF64");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskProcessAreas)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Task_Proc__task___5EBF139D");
        });

        modelBuilder.Entity<TaskProcessAreaUserCustomer>(entity =>
        {
            entity.HasKey(e => e.TaskProcessAreaUserCustomerId).HasName("PK__Task_Pro__A077543E75EDDF30");

            entity.ToTable("Task_Process_Area_User_Customer");

            entity.Property(e => e.TaskProcessAreaUserCustomerId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("task_process_area_user_customer_id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.EndDate)
                .HasColumnType("smalldatetime")
                .HasColumnName("end_date");
            entity.Property(e => e.StartDate)
                .HasColumnType("smalldatetime")
                .HasColumnName("start_date");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TaskProcessAreaId).HasColumnName("task_process_area_id");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime")
                .HasColumnName("timestamp");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Company).WithMany(p => p.TaskProcessAreaUserCustomers)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Task_Proc__compa__74AE54BC");

            entity.HasOne(d => d.Customer).WithMany(p => p.TaskProcessAreaUserCustomers)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Task_Proc__custo__778AC167");

            entity.HasOne(d => d.TaskProcessArea).WithMany(p => p.TaskProcessAreaUserCustomers)
                .HasForeignKey(d => d.TaskProcessAreaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Task_Proc__task___75A278F5");

            entity.HasOne(d => d.User).WithMany(p => p.TaskProcessAreaUserCustomers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Task_Proc__user___76969D2E");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__B9BE370F84373947");

            entity.ToTable("User");

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("user_id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(100)
                .HasColumnName("email_address");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.Password)
                .HasMaxLength(64)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(100)
                .HasColumnName("phone_number");
            entity.Property(e => e.ResetToken)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("reset_token");
            entity.Property(e => e.ResetTokenExpiryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("reset_token_expiry_date");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime")
                .HasColumnName("timestamp");

            entity.HasOne(d => d.Company).WithMany(p => p.Users)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__company_id__6FE99F9F");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__role_id__6EF57B66");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
