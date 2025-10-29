using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Users;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreUserGroup"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema and relationships for the core_user_group table,
/// which represents role/permission groups in the ECT system. User groups define sets of
/// permissions that control access to pages, features, and data within the application.
/// Groups are component-specific (Active, Reserve, Guard) and have hierarchical access levels
/// that determine visibility across the command structure. The access scope defines whether
/// users can see only their own data, their unit's data, or data across the entire component.
/// Groups support HIPAA restrictions, self-registration capabilities, and custom reporting views.
/// </remarks>
public class CoreUserGroupConfiguration : IEntityTypeConfiguration<CoreUserGroup>
{
    /// <summary>
    /// Configures the entity of type <see cref="CoreUserGroup"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreUserGroup> builder)
    {
        // Table mapping
        builder.ToTable("core_user_group", "dbo");

        // Primary key
        builder.HasKey(e => e.GroupId)
            .HasName("PK_core_user_group");

        // Properties configuration
        builder.Property(e => e.GroupId)
            .HasColumnName("group_id");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("name");

        builder.Property(e => e.Abbr)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("abbr");

        builder.Property(e => e.Compo)
            .IsRequired()
            .HasMaxLength(10)
            .HasColumnName("compo");

        builder.Property(e => e.AccessScope)
            .HasColumnName("access_scope");

        builder.Property(e => e.Active)
            .HasColumnName("active");

        builder.Property(e => e.PartialMatch)
            .HasColumnName("partial_match");

        builder.Property(e => e.ShowInfo)
            .HasColumnName("show_info");

        builder.Property(e => e.SortOrder)
            .HasColumnName("sort_order");

        builder.Property(e => e.HipaaRequired)
            .HasColumnName("hipaa_required");

        builder.Property(e => e.CanRegister)
            .HasColumnName("can_register");

        builder.Property(e => e.ReportView)
            .HasColumnName("report_view");

        builder.Property(e => e.GroupLevel)
            .HasColumnName("group_level");

        // Relationships
        builder.HasOne(d => d.AccessScopeNavigation)
            .WithMany(p => p.CoreUserGroups)
            .HasForeignKey(d => d.AccessScope)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_user_group_core_lkup_access_scope");

        builder.HasOne(d => d.CompoNavigation)
            .WithMany(p => p.CoreUserGroups)
            .HasForeignKey(d => d.Compo)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_user_group_core_lkup_compo");

        builder.HasOne(d => d.GroupLevelNavigation)
            .WithMany(p => p.CoreUserGroups)
            .HasForeignKey(d => d.GroupLevel)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_user_group_core_user_group_level");

        builder.HasOne(d => d.ReportViewNavigation)
            .WithMany(p => p.CoreUserGroups)
            .HasForeignKey(d => d.ReportView)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_user_group_core_lkup_chain_type");

        // Indexes
        builder.HasIndex(e => e.Name)
            .HasDatabaseName("IX_core_user_group_name")
            .IsUnique();

        builder.HasIndex(e => e.Abbr)
            .HasDatabaseName("IX_core_user_group_abbr")
            .IsUnique();

        builder.HasIndex(e => e.Compo, "IX_core_user_group_compo");

        builder.HasIndex(e => e.Active, "IX_core_user_group_active");

        builder.HasIndex(e => new { e.Compo, e.Active }, "IX_core_user_group_compo_active");

        builder.HasIndex(e => e.SortOrder, "IX_core_user_group_sort_order");
        
        builder.HasIndex(e => e.AccessScope, "IX_core_user_group_access_scope");
        
        builder.HasIndex(e => e.GroupLevel, "IX_core_user_group_group_level");
        
        builder.HasIndex(e => e.HipaaRequired)
            .HasDatabaseName("IX_core_user_group_hipaa_required")
            .HasFilter("hipaa_required = 1");
        
        builder.HasIndex(e => e.CanRegister)
            .HasDatabaseName("IX_core_user_group_can_register")
            .HasFilter("can_register = 1");
    }
}
