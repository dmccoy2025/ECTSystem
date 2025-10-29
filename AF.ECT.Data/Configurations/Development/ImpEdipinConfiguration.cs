using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpEdipin"/> entity.
/// Configures a staging table for importing EDIPIN (Electronic Data Interchange Personal Identifier Number)
/// mappings from external systems.
/// </summary>
/// <remarks>
/// ImpEdipin is a temporary staging table used during data import processes to map usernames to their
/// corresponding EDIPIN numbers (DoD unique identifier, also known as DoD ID number). This entity has
/// no primary key (keyless entity) as it represents transient import data used for user identity
/// synchronization with DoD systems.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for import staging
/// - All nullable string properties to accommodate raw import data
/// - Username to EDIPIN mapping
/// - Used for DoD identity synchronization
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful import and validation
/// </remarks>
public class ImpEdipinConfiguration : IEntityTypeConfiguration<ImpEdipin>
{
    /// <summary>
    /// Configures the ImpEdipin entity as a keyless staging table with EDIPIN mapping fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpEdipin.</param>
    public void Configure(EntityTypeBuilder<ImpEdipin> builder)
    {
        builder.ToTable("ImpEDIPIN", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // EDIPIN mapping properties
        builder.Property(e => e.Username)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("USERNAME");

        builder.Property(e => e.Edipin)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("EDIPIN");
        
        // Indexes for common queries
        builder.HasIndex(e => e.Username, "IX_imp_edipin_username");
        
        builder.HasIndex(e => e.Edipin, "IX_imp_edipin_edipin");
    }
}
