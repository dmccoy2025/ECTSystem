using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupMilitaryService"/>.
/// </summary>
public class CoreLkupMilitaryServiceConfiguration : IEntityTypeConfiguration<CoreLkupMilitaryService>
{
    /// <summary>
    /// Configures the entity type for CoreLkupMilitaryService.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupMilitaryService> builder)
    {
        builder.ToTable("Core_Lkup_MilitaryService", "dbo");

        builder.HasKey(e => e.ServiceId)
            .HasName("PK_Core_Lkup_MilitaryService");

        builder.Property(e => e.ServiceId).HasColumnName("ServiceID");
        builder.Property(e => e.Service).HasMaxLength(100);

        builder.HasIndex(e => e.Service, "IX_Core_Lkup_MilitaryService_Service");
    }
}
