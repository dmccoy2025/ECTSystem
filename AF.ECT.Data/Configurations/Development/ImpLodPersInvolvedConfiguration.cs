using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpLodPersInvolved"/> entity.
/// Configures a staging table for importing LOD (Line of Duty) personnel involved in incidents
/// from legacy systems.
/// </summary>
/// <remarks>
/// ImpLodPersInvolved is a temporary staging table used during data migration processes to load
/// information about personnel involved in LOD incidents from legacy systems. This entity has
/// no primary key (keyless entity) as it represents transient import data used for personnel
/// involvement tracking migration.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for import staging
/// - All nullable string properties to accommodate raw import data
/// - Personnel identification (name, SSN, grade)
/// - LOD investigation linkage (LI_ID, LIR_ID)
/// - Investigation flag (LOD investigation yes/no)
/// - String-based audit fields (CreatedBy/ModifiedBy as strings, dates as strings)
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful migration to production tables
/// </remarks>
public class ImpLodPersInvolvedConfiguration : IEntityTypeConfiguration<ImpLodPersInvolved>
{
    /// <summary>
    /// Configures the ImpLodPersInvolved entity as a keyless staging table with personnel
    /// involved import fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpLodPersInvolved.</param>
    public void Configure(EntityTypeBuilder<ImpLodPersInvolved> builder)
    {
        builder.ToTable("ImpLodPersInvolved", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // Investigation identifiers
        builder.Property(e => e.LiId)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("LI_ID");

        builder.Property(e => e.LirId)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("LIR_ID");

        // Personnel identification properties
        builder.Property(e => e.Name)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("NAME");

        builder.Property(e => e.Ssn)
            .HasMaxLength(11)
            .IsUnicode(false)
            .HasColumnName("SSN");

        builder.Property(e => e.Grade)
            .HasMaxLength(20)
            .IsUnicode(false)
            .HasColumnName("GRADE");

        // Investigation flag
        builder.Property(e => e.LodInvestigationYn)
            .HasMaxLength(1)
            .IsUnicode(false)
            .IsFixedLength()
            .HasColumnName("LOD_INVESTIGATION_YN");

        // Audit properties (string-based for import staging)
        builder.Property(e => e.CreatedBy)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("CREATED_BY");

        builder.Property(e => e.CreatedDate)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("CREATED_DATE");

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("MODIFIED_BY");

        builder.Property(e => e.ModifiedDate)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("MODIFIED_DATE");
        
        // Indexes for common queries
        builder.HasIndex(e => e.LiId, "IX_imp_lod_pers_involved_li_id");
        
        builder.HasIndex(e => e.LirId, "IX_imp_lod_pers_involved_lir_id");
        
        builder.HasIndex(e => e.Ssn, "IX_imp_lod_pers_involved_ssn");
        
        builder.HasIndex(e => e.Name, "IX_imp_lod_pers_involved_name");
    }
}
