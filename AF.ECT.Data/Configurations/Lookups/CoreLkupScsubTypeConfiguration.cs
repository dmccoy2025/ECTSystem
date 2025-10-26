using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupScsubType"/>.
/// </summary>
public class CoreLkupScsubTypeConfiguration : IEntityTypeConfiguration<CoreLkupScsubType>
{
    /// <summary>
    /// Configures the entity type for CoreLkupScsubType.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupScsubType> builder)
    {
        builder.ToTable("Core_Lkup_SCSubType");

        builder.HasKey(e => e.SubTypeId);

        builder.Property(e => e.SubTypeId).HasColumnName("SubTypeID");
        builder.Property(e => e.SubTypeTitle).HasMaxLength(255);
        builder.Property(e => e.AssociatedWorkflowId).HasColumnName("AssociatedWorkflowID");

        builder.HasIndex(e => e.AssociatedWorkflowId, "IX_Core_Lkup_SCSubType_AssociatedWorkflowID");
    }
}
