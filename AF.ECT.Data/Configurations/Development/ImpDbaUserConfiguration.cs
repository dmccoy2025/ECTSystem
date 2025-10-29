using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpDbaUser"/> entity.
/// Configures a staging table for importing database user account information from Oracle DBA_USERS view.
/// </summary>
/// <remarks>
/// ImpDbaUser is a temporary staging table used during data import processes to load database user
/// account information from Oracle or other external database systems. This entity has no primary key
/// (keyless entity) as it represents transient import data used for user account synchronization
/// and security auditing.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for import staging
/// - All nullable string properties to accommodate raw import data
/// - Username and user ID tracking
/// - Account status and security tracking (lock date, expiry date)
/// - Tablespace assignments (default and temporary)
/// - Profile and resource consumer group settings
/// - External name for directory service integration
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful import and validation
/// </remarks>
public class ImpDbaUserConfiguration : IEntityTypeConfiguration<ImpDbaUser>
{
    /// <summary>
    /// Configures the ImpDbaUser entity as a keyless staging table with database user
    /// account import fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpDbaUser.</param>
    public void Configure(EntityTypeBuilder<ImpDbaUser> builder)
    {
        builder.ToTable("ImpDbaUser", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // User identification properties
        builder.Property(e => e.Username)
            .HasMaxLength(128)
            .IsUnicode(false)
            .HasColumnName("USERNAME");

        builder.Property(e => e.UserId)
            .HasMaxLength(40)
            .IsUnicode(false)
            .HasColumnName("USER_ID");

        builder.Property(e => e.Password)
            .HasMaxLength(128)
            .IsUnicode(false)
            .HasColumnName("PASSWORD");

        // Account status properties
        builder.Property(e => e.AccountStatus)
            .HasMaxLength(32)
            .IsUnicode(false)
            .HasColumnName("ACCOUNT_STATUS");

        builder.Property(e => e.LockDate)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("LOCK_DATE");

        builder.Property(e => e.ExpiryDate)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("EXPIRY_DATE");

        // Tablespace properties
        builder.Property(e => e.DefaultTablespace)
            .HasMaxLength(30)
            .IsUnicode(false)
            .HasColumnName("DEFAULT_TABLESPACE");

        builder.Property(e => e.TemporaryTablespace)
            .HasMaxLength(30)
            .IsUnicode(false)
            .HasColumnName("TEMPORARY_TABLESPACE");

        // Account creation and profile properties
        builder.Property(e => e.Created)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("CREATED");

        builder.Property(e => e.Profile)
            .HasMaxLength(128)
            .IsUnicode(false)
            .HasColumnName("PROFILE");

        builder.Property(e => e.InitialRsrcConsumerGroup)
            .HasMaxLength(128)
            .IsUnicode(false)
            .HasColumnName("INITIAL_RSRC_CONSUMER_GROUP");

        builder.Property(e => e.ExternalName)
            .HasMaxLength(4000)
            .IsUnicode(false)
            .HasColumnName("EXTERNAL_NAME");
        
        // Indexes for common queries
        builder.HasIndex(e => e.Username, "IX_imp_dba_user_username");
        
        builder.HasIndex(e => e.UserId, "IX_imp_dba_user_user_id");
        
        builder.HasIndex(e => e.AccountStatus, "IX_imp_dba_user_account_status");
        
        builder.HasIndex(e => e.Created, "IX_imp_dba_user_created");
        
        builder.HasIndex(e => e.ExpiryDate, "IX_imp_dba_user_expiry_date");
    }
}
