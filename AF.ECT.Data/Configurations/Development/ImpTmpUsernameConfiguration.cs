using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpTmpUsername"/> entity.
/// Configures a temporary staging table for storing usernames during import processing.
/// </summary>
/// <remarks>
/// ImpTmpUsername is a temporary working table used during data import processes to store usernames
/// for batch operations, user validation, or temporary tracking. This lightweight table serves as
/// a scratch space for storing username identifiers that need to be processed, validated, or
/// cross-referenced during complex user import or migration operations. This entity has no primary
/// key (keyless entity) as it represents temporary working data.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for temporary storage
/// - Single nullable string property (USERNAME)
/// - Used for batch operations and temporary tracking during user imports
/// - Facilitates set-based operations on usernames
/// - Supports deduplication, validation, and cross-referencing of user accounts
/// - No foreign key relationships (temporary isolation)
/// - Typically cleared after each import operation or batch completes
/// - Companion to ImpTmpPersid for user account processing
/// </remarks>
public class ImpTmpUsernameConfiguration : IEntityTypeConfiguration<ImpTmpUsername>
{
    /// <summary>
    /// Configures the ImpTmpUsername entity as a keyless temporary table with username field.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpTmpUsername.</param>
    public void Configure(EntityTypeBuilder<ImpTmpUsername> builder)
    {
        builder.ToTable("ImpTmpUsername", "dbo");

        // Keyless entity for temporary storage
        builder.HasNoKey();

        // Username property
        builder.Property(e => e.Username)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("USERNAME");
        
        // Index for common queries
        builder.HasIndex(e => e.Username, "IX_imp_tmp_username_username");
    }
}
