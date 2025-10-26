using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Entity Framework configuration for the <see cref="Form348ScPeppType"/> entity.
/// Configures the many-to-many relationship between Form 348 Special Category (SC) cases
/// and PEPP (Personnel Evaluation Processing Program) types.
/// </summary>
/// <remarks>
/// Form348ScPeppType is a junction table that associates Form 348 Special Category cases
/// with one or more PEPP types. This allows a single SC case to be classified under multiple
/// PEPP type categories, supporting complex personnel evaluation processing workflows.
/// 
/// Key characteristics:
/// - Many-to-many junction table (Form348Sc ↔ CoreLkupPepptype)
/// - Composite primary key (RefId, TypeId)
/// - No additional properties beyond the relationship keys
/// - Enables multi-category classification of special category cases
/// </remarks>
public class Form348ScPeppTypeConfiguration : IEntityTypeConfiguration<Form348ScPeppType>
{
    /// <summary>
    /// Configures the Form348ScPeppType entity with table mapping, composite primary key,
    /// and foreign key relationships to Form348Sc and CoreLkupPepptype.
    /// </summary>
    /// <param name="builder">The entity type builder for Form348ScPeppType.</param>
    public void Configure(EntityTypeBuilder<Form348ScPeppType> builder)
    {
        builder.ToTable("Form348_SC_PEPPType", "dbo");

        // Composite primary key
        builder.HasKey(e => new { e.RefId, e.TypeId })
            .HasName("PK_Form348_SC_PEPPType");

        builder.Property(e => e.RefId)
            .HasColumnName("RefID");

        builder.Property(e => e.TypeId)
            .HasColumnName("TypeID");

        // Foreign key relationships
        builder.HasOne(d => d.Ref)
            .WithMany()
            .HasForeignKey(d => d.RefId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_Form348_SC_PEPPType_Form348_SC");

        builder.HasOne(d => d.Type)
            .WithMany()
            .HasForeignKey(d => d.TypeId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_Form348_SC_PEPPType_Core_Lkup_PEPPType");

        // Indexes for foreign keys
        builder.HasIndex(e => e.RefId, "IX_Form348_SC_PEPPType_RefID");
        builder.HasIndex(e => e.TypeId, "IX_Form348_SC_PEPPType_TypeID");
    }
}
