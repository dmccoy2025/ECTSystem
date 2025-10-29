using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.CommandStructure;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CommandStructTreeTmp"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the Command_Struct_Tree_Tmp table,
/// which serves as a temporary staging table for building command structure hierarchies.
/// Contains the same structure as Command_Struct_Tree (view type, parent/child PAS codes,
/// parent/child IDs) but used during tree construction or reorganization operations.
/// Temporary data is computed here before being materialized into the main tree table.
/// </remarks>
public class CommandStructTreeTmpConfiguration : IEntityTypeConfiguration<CommandStructTreeTmp>
{
    /// <summary>
    /// Configures the entity of type <see cref="CommandStructTreeTmp"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CommandStructTreeTmp> builder)
    {
        // Table mapping
        builder.ToTable("Command_Struct_Tree_Tmp", "dbo");

        // No primary key - this is a temporary staging table
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
        builder.HasIndex(e => e.ParentPas, "IX_command_struct_tree_tmp_parent_pas");

        builder.HasIndex(e => e.ChildPas, "IX_command_struct_tree_tmp_child_pas");
    }
}
