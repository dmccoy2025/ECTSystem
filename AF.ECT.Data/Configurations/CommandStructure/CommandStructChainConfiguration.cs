using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.CommandStructure;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CommandStructChain"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the command_struct_chain table,
/// which represents hierarchical chains of command in the military organizational structure. Supports multiple
/// chain types (administrative, operational, medical) and hierarchical relationships between units.
/// </remarks>
public class CommandStructChainConfiguration : IEntityTypeConfiguration<CommandStructChain>
{
    /// <summary>
    /// Configures the CommandStructChain entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CommandStructChain> builder)
    {
        // Table mapping
        builder.ToTable("command_struct_chain", "dbo");

        // Primary key
        builder.HasKey(e => e.CscId)
            .HasName("PK_command_struct_chain");

        // Property configurations
        builder.Property(e => e.CscId)
            .HasColumnName("csc_id");

        builder.Property(e => e.CsId)
            .HasColumnName("cs_id");

        builder.Property(e => e.ChainType)
            .HasMaxLength(50)
            .HasColumnName("chain_type");

        builder.Property(e => e.CscIdParent)
            .HasColumnName("csc_id_parent");

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .HasColumnName("created_by");

        builder.Property(e => e.CreatedDate)
            .HasColumnType("datetime")
            .HasDefaultValueSql("getdate()")
            .HasColumnName("created_date");

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(100)
            .HasColumnName("modified_by");

        builder.Property(e => e.ModifiedDate)
            .HasColumnType("datetime")
            .HasColumnName("modified_date");

        builder.Property(e => e.ViewType)
            .HasColumnName("view_type");

        builder.Property(e => e.UserModified)
            .HasColumnName("user_modified");

        // Relationships
        builder.HasOne(d => d.ViewTypeNavigation)
            .WithMany()
            .HasForeignKey(d => d.ViewType)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_command_struct_chain_core_lkup_chain_type");

        // Indexes
        builder.HasIndex(e => e.CsId, "IX_command_struct_chain_cs_id");

        builder.HasIndex(e => e.CscIdParent, "IX_command_struct_chain_parent");

        builder.HasIndex(e => new { e.CsId, e.ViewType }, "IX_command_struct_chain_cs_view");

        builder.HasIndex(e => e.ViewType, "IX_command_struct_chain_view_type");
    }
}
