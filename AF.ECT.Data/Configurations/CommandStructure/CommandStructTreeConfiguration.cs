using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.CommandStructure;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CommandStructTree"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the Command_Struct_Tree table,
/// which stores hierarchical parent-child relationships between command structures
/// as a flattened tree for efficient querying. Contains view type, parent/child PAS codes,
/// and parent/child IDs. Used for building organizational charts and unit hierarchies.
/// This is likely a materialized view or computed table for performance optimization.
/// </remarks>
public class CommandStructTreeConfiguration : IEntityTypeConfiguration<CommandStructTree>
{
    /// <summary>
    /// Configures the entity of type <see cref="CommandStructTree"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CommandStructTree> builder)
    {
        // Table mapping
        builder.ToTable("Command_Struct_Tree", "dbo");

        // No primary key - this is a view/materialized query result
        builder.HasNoKey();

        // Properties configuration
        builder.Property(e => e.ViewType)
            .IsRequired()
            .HasColumnName("ViewType");

        builder.Property(e => e.ParentPas)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("ParentPAS");

        builder.Property(e => e.ChildPas)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("ChildPAS");

        builder.Property(e => e.ParentId)
            .HasColumnName("ParentID");

        builder.Property(e => e.ChildId)
            .HasColumnName("ChildID");

        // Indexes for common queries
        builder.HasIndex(e => e.ParentPas, "IX_command_struct_tree_parent_pas");

        builder.HasIndex(e => e.ChildPas, "IX_command_struct_tree_child_pas");

        builder.HasIndex(e => e.ViewType, "IX_command_struct_tree_view_type");
    }
}
