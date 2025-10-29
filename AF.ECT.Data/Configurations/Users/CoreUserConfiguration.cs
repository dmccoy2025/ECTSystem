using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Users;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreUser"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_user table,
/// which represents system users in the Electronic Case Tracking (ECT) system. Users can be military
/// personnel, civilian employees, or contractors who interact with the ALOD system for case management,
/// workflow processing, and reporting functions.
/// </remarks>
public class CoreUserConfiguration : IEntityTypeConfiguration<CoreUser>
{
    /// <summary>
    /// Configures the entity of type <see cref="CoreUser"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreUser> builder)
    {
        // Table mapping
        builder.ToTable("core_user", "dbo");

        // Primary key
        builder.HasKey(e => e.UserId)
            .HasName("PK_core_user");

        // Properties configuration
        builder.Property(e => e.UserId)
            .HasColumnName("user_id");

        builder.Property(e => e.Username)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("username");

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("last_name");

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("first_name");

        builder.Property(e => e.MiddleName)
            .HasMaxLength(50)
            .HasColumnName("middle_name");

        builder.Property(e => e.Edipin)
            .HasMaxLength(10)
            .HasColumnName("edipin");

        builder.Property(e => e.Edipin2)
            .HasMaxLength(10)
            .HasColumnName("edipin2");

        builder.Property(e => e.Title)
            .HasMaxLength(100)
            .HasColumnName("title");

        builder.Property(e => e.Ssn)
            .HasMaxLength(11)
            .HasColumnName("ssn");

        builder.Property(e => e.DateOfBirth)
            .HasColumnType("date")
            .HasColumnName("date_of_birth");

        builder.Property(e => e.Phone)
            .HasMaxLength(20)
            .HasColumnName("phone");

        builder.Property(e => e.Dsn)
            .HasMaxLength(20)
            .HasColumnName("dsn");

        builder.Property(e => e.Email)
            .HasMaxLength(100)
            .HasColumnName("email");

        builder.Property(e => e.Email2)
            .HasMaxLength(100)
            .HasColumnName("email2");

        builder.Property(e => e.Email3)
            .HasMaxLength(100)
            .HasColumnName("email3");

        builder.Property(e => e.WorkCompo)
            .IsRequired()
            .HasMaxLength(10)
            .HasColumnName("work_compo");

        builder.Property(e => e.WorkStreet)
            .HasMaxLength(100)
            .HasColumnName("work_street");

        builder.Property(e => e.WorkCity)
            .HasMaxLength(50)
            .HasColumnName("work_city");

        builder.Property(e => e.WorkState)
            .HasMaxLength(2)
            .HasColumnName("work_state");

        builder.Property(e => e.WorkZip)
            .HasMaxLength(10)
            .HasColumnName("work_zip");

        builder.Property(e => e.WorkCountry)
            .HasMaxLength(50)
            .HasColumnName("work_country");

        builder.Property(e => e.AccessStatus)
            .HasColumnName("access_status");

        builder.Property(e => e.ReceiveEmail)
            .HasColumnName("receive_email");

        builder.Property(e => e.ReceiveReminderEmail)
            .HasColumnName("receive_reminder_email");

        builder.Property(e => e.ExpirationDate)
            .HasColumnType("datetime")
            .HasColumnName("expiration_date");

        builder.Property(e => e.Comment)
            .HasColumnName("comment");

        builder.Property(e => e.LastAccessDate)
            .HasColumnType("datetime")
            .HasColumnName("last_access_date");

        builder.Property(e => e.RankCode)
            .HasColumnName("rank_code");

        builder.Property(e => e.CreatedDate)
            .HasColumnType("datetime")
            .HasColumnName("created_date")
            .HasDefaultValueSql("(getdate())");

        builder.Property(e => e.ModifiedDate)
            .HasColumnType("datetime")
            .HasColumnName("modified_date");

        builder.Property(e => e.ModifiedBy)
            .HasColumnName("modified_by");

        builder.Property(e => e.CsId)
            .HasColumnName("cs_id");

        builder.Property(e => e.AdaCsId)
            .HasColumnName("ada_cs_id");

        builder.Property(e => e.CurrentRole)
            .HasColumnName("current_role");

        builder.Property(e => e.DisabledDate)
            .HasColumnType("datetime")
            .HasColumnName("disabled_date");

        builder.Property(e => e.DisabledBy)
            .HasColumnName("disabled_by");

        builder.Property(e => e.ReportView)
            .HasColumnName("report_view");

        builder.Property(e => e.UnitView)
            .HasColumnName("unit_view");

        builder.Property(e => e.IsPinActive)
            .HasColumnName("is_pin_active");

        // Relationships
        builder.HasOne(d => d.AccessStatusNavigation)
            .WithMany(p => p.CoreUsers)
            .HasForeignKey(d => d.AccessStatus)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_user_core_lkup_access_status");

        builder.HasOne(d => d.AdaCs)
            .WithMany(p => p.CoreUserAdaCs)
            .HasForeignKey(d => d.AdaCsId)
            .HasConstraintName("FK_core_user_command_struct_ada");

        builder.HasOne(d => d.Cs)
            .WithMany(p => p.CoreUserCs)
            .HasForeignKey(d => d.CsId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_user_command_struct");

        builder.HasOne(d => d.CurrentRoleNavigation)
            .WithMany(p => p.CoreUsers)
            .HasForeignKey(d => d.CurrentRole)
            .HasConstraintName("FK_core_user_core_user_role");

        builder.HasOne(d => d.ModifiedByNavigation)
            .WithMany(p => p.InverseModifiedByNavigation)
            .HasForeignKey(d => d.ModifiedBy)
            .HasConstraintName("FK_core_user_core_user");

        builder.HasOne(d => d.RankCodeNavigation)
            .WithMany(p => p.CoreUsers)
            .HasForeignKey(d => d.RankCode)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_user_core_lkup_grade");

        builder.HasOne(d => d.ReportViewNavigation)
            .WithMany(p => p.CoreUsers)
            .HasForeignKey(d => d.ReportView)
            .HasConstraintName("FK_core_user_core_lkup_chain_type");

        builder.HasOne(d => d.WorkCompoNavigation)
            .WithMany(p => p.CoreUsers)
            .HasForeignKey(d => d.WorkCompo)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_user_core_lkup_compo");

        // Indexes
        builder.HasIndex(e => e.Username)
            .HasDatabaseName("IX_core_user_username")
            .IsUnique();

        builder.HasIndex(e => e.Edipin, "IX_core_user_edipin");

        builder.HasIndex(e => e.CsId, "IX_core_user_cs_id");

        builder.HasIndex(e => e.AccessStatus, "IX_core_user_access_status");
        
        builder.HasIndex(e => e.Email, "IX_core_user_email");
        
        builder.HasIndex(e => e.LastAccessDate, "IX_core_user_last_access_date");
        
        builder.HasIndex(e => e.ExpirationDate, "IX_core_user_expiration_date");
        
        builder.HasIndex(e => e.CreatedDate, "IX_core_user_created_date");
        
        builder.HasIndex(e => e.ModifiedDate, "IX_core_user_modified_date");
        
        builder.HasIndex(e => e.ModifiedBy, "IX_core_user_modified_by");
        
        builder.HasIndex(e => e.CurrentRole, "IX_core_user_current_role");
        
        builder.HasIndex(e => e.WorkCompo, "IX_core_user_work_compo");
        
        builder.HasIndex(e => new { e.LastName, e.FirstName }, "IX_core_user_name");
        
        builder.HasIndex(e => new { e.AccessStatus, e.ExpirationDate }, "IX_core_user_access_expiration");
    }
}
