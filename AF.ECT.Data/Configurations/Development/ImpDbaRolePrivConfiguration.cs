using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpDbaRolePriv"/> entity.
/// Configures a staging table for importing database role privilege data from Oracle or other external systems.
/// </summary>
/// <remarks>
/// ImpDbaRolePriv is a temporary staging table used during data import processes to load database
/// role and privilege assignments from external database systems (typically Oracle DBA_ROLE_PRIVS view).
/// This entity has no primary key (keyless entity) as it represents transient import data used for
/// security auditing and role synchronization.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for import staging
/// - All nullable string properties to accommodate raw import data
/// - Grantee tracking (user/role receiving the privilege)
/// - Granted role tracking (role being granted)
/// - Admin option flag (WITH ADMIN OPTION privilege)
/// - Default role flag (whether role is default for the grantee)
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after security audit/synchronization
/// </remarks>
public class ImpDbaRolePrivConfiguration : IEntityTypeConfiguration<ImpDbaRolePriv>
{
    /// <summary>
    /// Configures the ImpDbaRolePriv entity as a keyless staging table with database role
    /// privilege import fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpDbaRolePriv.</param>
    public void Configure(EntityTypeBuilder<ImpDbaRolePriv> builder)
    {
        builder.ToTable("ImpDbaRolePriv");

        // Keyless entity for staging
        builder.HasNoKey();

        // Role privilege properties
        builder.Property(e => e.Grantee)
            .HasMaxLength(128)
            .IsUnicode(false)
            .HasColumnName("GRANTEE");

        builder.Property(e => e.GrantedRole)
            .HasMaxLength(128)
            .IsUnicode(false)
            .HasColumnName("GRANTED_ROLE");

        builder.Property(e => e.Adm)
            .HasMaxLength(3)
            .IsUnicode(false)
            .IsFixedLength()
            .HasColumnName("ADM");

        builder.Property(e => e.Def)
            .HasMaxLength(3)
            .IsUnicode(false)
            .IsFixedLength()
            .HasColumnName("DEF");
    }
}
