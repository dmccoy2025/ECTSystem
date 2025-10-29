using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework Core configuration for the <see cref="AfrcOracleDbaUsersDatum"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the AfrcOracle_DBA_Users_Data table,
/// which stores Oracle database user account information from AFRC Oracle system migration.
/// Contains user credentials, account status, and tablespace assignments from legacy Oracle database.
/// All properties are nullable strings representing raw Oracle data for import/migration purposes.
/// </remarks>
public class AfrcOracleDbaUsersDatumConfiguration : IEntityTypeConfiguration<AfrcOracleDbaUsersDatum>
{
    /// <summary>
    /// Configures the entity of type <see cref="AfrcOracleDbaUsersDatum"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<AfrcOracleDbaUsersDatum> builder)
    {
        // Table mapping
        builder.ToTable("AfrcOracle_DBA_Users_Data", "dbo");

        // No primary key - this is a staging/import table
        builder.HasNoKey();

        // Properties configuration
        builder.Property(e => e.Username)
            .HasColumnName("username");

        builder.Property(e => e.UserId)
            .HasColumnName("user_id");

        builder.Property(e => e.Password)
            .HasColumnName("password");

        builder.Property(e => e.AccountStatus)
            .HasColumnName("account_status");

        builder.Property(e => e.LockDate)
            .HasColumnName("lock_date");

        builder.Property(e => e.ExpiryDate)
            .HasColumnName("expiry_date");

        builder.Property(e => e.DefaultTablespace)
            .HasColumnName("default_tablespace");

        builder.Property(e => e.TemporaryTablespace)
            .HasColumnName("temporary_tablespace");

        builder.Property(e => e.Created)
            .HasColumnName("created");

        builder.Property(e => e.Profile)
            .HasColumnName("profile");

        // Indexes for common queries
        builder.HasIndex(e => e.Username, "IX_afrc_oracle_dba_users_username");

        builder.HasIndex(e => e.AccountStatus, "IX_afrc_oracle_dba_users_account_status");
        
        builder.HasIndex(e => e.Created, "IX_afrc_oracle_dba_users_created");
        
        builder.HasIndex(e => e.ExpiryDate, "IX_afrc_oracle_dba_users_expiry_date");
        
        builder.HasIndex(e => e.UserId, "IX_afrc_oracle_dba_users_user_id");
    }
}
