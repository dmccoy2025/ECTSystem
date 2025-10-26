using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpNonUnitPersonnel"/> entity.
/// Configures a staging table for importing non-unit affiliated personnel data from legacy systems.
/// </summary>
/// <remarks>
/// ImpNonUnitPersonnel is a temporary staging table used during data migration processes to load
/// personnel who are not assigned to standard unit structures (e.g., civilians, contractors,
/// augmentees, special assignment personnel). This entity has no primary key (keyless entity)
/// as it represents transient import data.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for import staging
/// - All nullable string properties to accommodate incomplete legacy data
/// - Personnel identification (PersId, username, name components)
/// - Command structure association (CsId)
/// - Duty section and sex tracking
/// - Email addresses (home, work, unit)
/// - Contact information
/// - Residential address (street, city, state, postal code, country)
/// - String-based audit fields (CreatedBy/ModifiedBy as strings, dates as strings)
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful migration to production personnel tables
/// </remarks>
public class ImpNonUnitPersonnelConfiguration : IEntityTypeConfiguration<ImpNonUnitPersonnel>
{
    /// <summary>
    /// Configures the ImpNonUnitPersonnel entity as a keyless staging table with non-unit
    /// personnel import fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpNonUnitPersonnel.</param>
    public void Configure(EntityTypeBuilder<ImpNonUnitPersonnel> builder)
    {
        builder.ToTable("ImpNonUnitPersonnel", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // Personnel identification properties
        builder.Property(e => e.PersId).HasMaxLength(50).IsUnicode(false).HasColumnName("PERS_ID");
        builder.Property(e => e.Username).HasMaxLength(50).IsUnicode(false).HasColumnName("USERNAME");
        builder.Property(e => e.Name).HasMaxLength(100).IsUnicode(false).HasColumnName("NAME");
        builder.Property(e => e.FirstName).HasMaxLength(50).IsUnicode(false).HasColumnName("FIRST_NAME");
        builder.Property(e => e.Mi).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("MI");
        builder.Property(e => e.LastName).HasMaxLength(50).IsUnicode(false).HasColumnName("LAST_NAME");
        builder.Property(e => e.NameSuffix).HasMaxLength(10).IsUnicode(false).HasColumnName("NAME_SUFFIX");

        // Assignment properties
        builder.Property(e => e.CsId).HasMaxLength(50).IsUnicode(false).HasColumnName("CS_ID");
        builder.Property(e => e.DutySection).HasMaxLength(100).IsUnicode(false).HasColumnName("DUTY_SECTION");
        builder.Property(e => e.Sex).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("SEX");

        // Contact properties
        builder.Property(e => e.EMailHome).HasMaxLength(100).IsUnicode(false).HasColumnName("E_MAIL_HOME");
        builder.Property(e => e.EMailWork).HasMaxLength(100).IsUnicode(false).HasColumnName("E_MAIL_WORK");
        builder.Property(e => e.EMailUnit).HasMaxLength(100).IsUnicode(false).HasColumnName("E_MAIL_UNIT");
        builder.Property(e => e.ContactInfo).HasMaxLength(200).IsUnicode(false).HasColumnName("CONTACT_INFO");

        // Residential address properties
        builder.Property(e => e.ResAddress1).HasMaxLength(100).IsUnicode(false).HasColumnName("RES_ADDRESS1");
        builder.Property(e => e.ResAddress2).HasMaxLength(100).IsUnicode(false).HasColumnName("RES_ADDRESS2");
        builder.Property(e => e.ResCity).HasMaxLength(50).IsUnicode(false).HasColumnName("RES_CITY");
        builder.Property(e => e.ResPostalCode).HasMaxLength(10).IsUnicode(false).HasColumnName("RES_POSTAL_CODE");
        builder.Property(e => e.ResCountry).HasMaxLength(50).IsUnicode(false).HasColumnName("RES_COUNTRY");
        builder.Property(e => e.ResState).HasMaxLength(2).IsUnicode(false).IsFixedLength().HasColumnName("RES_STATE");

        // Audit properties (string-based for import staging)
        builder.Property(e => e.CreatedBy).HasMaxLength(50).IsUnicode(false).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedDate).HasMaxLength(50).IsUnicode(false).HasColumnName("CREATED_DATE");
        builder.Property(e => e.ModifiedBy).HasMaxLength(50).IsUnicode(false).HasColumnName("MODIFIED_BY");
        builder.Property(e => e.ModifiedDate).HasMaxLength(50).IsUnicode(false).HasColumnName("MODIFIED_DATE");
    }
}
