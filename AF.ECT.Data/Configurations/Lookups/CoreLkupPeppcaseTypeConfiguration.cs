using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupPeppcaseType"/>.
/// </summary>
public class CoreLkupPeppcaseTypeConfiguration : IEntityTypeConfiguration<CoreLkupPeppcaseType>
{
    /// <summary>
    /// Configures the entity type for CoreLkupPeppcaseType.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupPeppcaseType> builder)
    {
        builder.ToTable("Core_Lkup_PEPPCaseType", "dbo");

        builder.HasKey(e => e.CaseTypeId)
            .HasName("PK_Core_Lkup_PEPPCaseType");

        builder.Property(e => e.CaseTypeId).HasColumnName("CaseTypeID");
        builder.Property(e => e.CaseTypeName).HasMaxLength(255);

        builder.HasIndex(e => e.Active, "IX_Core_Lkup_PEPPCaseType_Active");
    }
}
