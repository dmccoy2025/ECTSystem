using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpLodActiveDuty"/> entity.
/// Configures a staging table for importing LOD (Line of Duty) active duty personnel information
/// from legacy systems.
/// </summary>
/// <remarks>
/// ImpLodActiveDuty is a temporary staging table used during data migration processes to load
/// active duty personnel information associated with LOD cases from legacy systems. This entity
/// has no primary key (keyless entity) as it represents transient import data used for personnel
/// data migration and validation.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for import staging
/// - All nullable string properties to accommodate raw import data
/// - Personnel identification (SSN, name, rank, AFSC)
/// - Contact information (email, telephone, unit address)
/// - Duty assignment tracking (unit, duty title)
/// - Expiration date for active duty status
/// - String-based audit fields (CreatedBy/ModifiedBy as strings, dates as strings)
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful migration to production tables
/// </remarks>
public class ImpLodActiveDutyConfiguration : IEntityTypeConfiguration<ImpLodActiveDuty>
{
    /// <summary>
    /// Configures the ImpLodActiveDuty entity as a keyless staging table with active duty
    /// personnel import fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpLodActiveDuty.</param>
    public void Configure(EntityTypeBuilder<ImpLodActiveDuty> builder)
    {
        builder.ToTable("ImpLodActiveDuty", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // Active duty identifier
        builder.Property(e => e.AdId)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("AD_ID");

        // Personnel identification properties
        builder.Property(e => e.Ssn)
            .HasMaxLength(11)
            .IsUnicode(false)
            .HasColumnName("SSN");

        builder.Property(e => e.FirstName)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("FIRST_NAME");

        builder.Property(e => e.MiddleInitial)
            .HasMaxLength(1)
            .IsUnicode(false)
            .IsFixedLength()
            .HasColumnName("MIDDLE_INITIAL");

        builder.Property(e => e.LastName)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("LAST_NAME");

        builder.Property(e => e.Rank)
            .HasMaxLength(20)
            .IsUnicode(false)
            .HasColumnName("RANK");

        // Duty assignment properties
        builder.Property(e => e.DutyTitle)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("DUTY_TITLE");

        builder.Property(e => e.Afsc)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("AFSC");

        // Contact information properties
        builder.Property(e => e.Email)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("EMAIL");

        builder.Property(e => e.Telephone)
            .HasMaxLength(20)
            .IsUnicode(false)
            .HasColumnName("TELEPHONE");

        builder.Property(e => e.Unit)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("UNIT");

        builder.Property(e => e.UnitAddress)
            .HasMaxLength(200)
            .IsUnicode(false)
            .HasColumnName("UNIT_ADDRESS");

        // Status tracking properties
        builder.Property(e => e.ExpirationDate)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("EXPIRATION_DATE");

        // Audit properties (string-based for import staging)
        builder.Property(e => e.CreatedDate)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("CREATED_DATE");

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("CREATED_BY");

        builder.Property(e => e.ModifiedDate)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("MODIFIED_DATE");

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("MODIFIED_BY");
        
        // Indexes for common queries
        builder.HasIndex(e => e.AdId, "IX_imp_lod_active_duty_ad_id");
        
        builder.HasIndex(e => e.Ssn, "IX_imp_lod_active_duty_ssn");
        
        builder.HasIndex(e => e.LastName, "IX_imp_lod_active_duty_last_name");
        
        builder.HasIndex(e => e.Unit, "IX_imp_lod_active_duty_unit");
        
        builder.HasIndex(e => e.CreatedDate, "IX_imp_lod_active_duty_created_date");
    }
}
