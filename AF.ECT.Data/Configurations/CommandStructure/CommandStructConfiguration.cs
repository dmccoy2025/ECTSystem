using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.CommandStructure;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CommandStruct"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema and relationships for the command_struct table,
/// which represents the hierarchical military command structure in the ECT system. The
/// command structure includes units at all levels from wing/division down to detachments,
/// supporting all components (Active Duty, Reserve, Guard). This hierarchy is used for
/// user assignments, case routing, reporting chains, and permission management throughout
/// the application.
/// </remarks>
public class CommandStructConfiguration : IEntityTypeConfiguration<CommandStruct>
{
    /// <summary>
    /// Configures the entity of type <see cref="CommandStruct"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CommandStruct> builder)
    {
        // Table mapping
        builder.ToTable("command_struct", "dbo");

        // Primary key
        builder.HasKey(e => e.CsId)
            .HasName("PK_command_struct");

        // Properties configuration
        builder.Property(e => e.CsId)
            .HasColumnName("cs_id");

        builder.Property(e => e.Address1)
            .HasMaxLength(100)
            .HasColumnName("address1");

        builder.Property(e => e.Address2)
            .HasMaxLength(100)
            .HasColumnName("address2");

        builder.Property(e => e.BaseCode)
            .HasMaxLength(10)
            .HasColumnName("base_code");

        builder.Property(e => e.City)
            .HasMaxLength(50)
            .HasColumnName("city");

        builder.Property(e => e.CommandCode)
            .HasMaxLength(20)
            .HasColumnName("command_code");

        builder.Property(e => e.CommandStructUtc)
            .HasMaxLength(20)
            .HasColumnName("command_struct_utc");

        builder.Property(e => e.Component)
            .HasMaxLength(10)
            .HasColumnName("component");

        builder.Property(e => e.Country)
            .HasMaxLength(50)
            .HasColumnName("country");

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(50)
            .HasColumnName("created_by");

        builder.Property(e => e.CreatedDate)
            .HasColumnType("datetime")
            .HasColumnName("created_date");

        builder.Property(e => e.CsIdParent)
            .HasColumnName("cs_id_parent");

        builder.Property(e => e.CsLevel)
            .HasMaxLength(20)
            .HasColumnName("cs_level");

        builder.Property(e => e.CsOperType)
            .HasMaxLength(20)
            .HasColumnName("cs_oper_type");

        builder.Property(e => e.EMail)
            .HasMaxLength(100)
            .HasColumnName("email");

        builder.Property(e => e.GainingCommandCsId)
            .HasColumnName("gaining_command_cs_id");

        builder.Property(e => e.GeoLoc)
            .HasMaxLength(50)
            .HasColumnName("geo_loc");

        builder.Property(e => e.LongName)
            .HasMaxLength(200)
            .HasColumnName("long_name");

        builder.Property(e => e.MedicalService)
            .HasMaxLength(50)
            .HasColumnName("medical_service");

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(50)
            .HasColumnName("modified_by");

        builder.Property(e => e.ModifiedDate)
            .HasColumnType("datetime")
            .HasColumnName("modified_date");

        builder.Property(e => e.MrdssDocDate)
            .HasColumnType("date")
            .HasColumnName("mrdss_doc_date");

        builder.Property(e => e.MrdssDocId)
            .HasMaxLength(50)
            .HasColumnName("mrdss_doc_id");

        builder.Property(e => e.MrdssDocReview)
            .HasMaxLength(50)
            .HasColumnName("mrdss_doc_review");

        builder.Property(e => e.MrdssKind)
            .HasMaxLength(20)
            .HasColumnName("mrdss_kind");

        builder.Property(e => e.PasCode)
            .HasMaxLength(20)
            .HasColumnName("pas_code");

        builder.Property(e => e.PhysExamYn)
            .HasMaxLength(1)
            .HasColumnName("phys_exam_yn");

        builder.Property(e => e.PostalCode)
            .HasMaxLength(20)
            .HasColumnName("postal_code");

        builder.Property(e => e.SchedulingYn)
            .HasMaxLength(1)
            .HasColumnName("scheduling_yn");

        builder.Property(e => e.State)
            .HasMaxLength(2)
            .HasColumnName("state");

        builder.Property(e => e.TimeZone)
            .HasMaxLength(50)
            .HasColumnName("time_zone");

        builder.Property(e => e.Uic)
            .HasMaxLength(20)
            .HasColumnName("uic");

        builder.Property(e => e.UnitDet)
            .HasMaxLength(50)
            .HasColumnName("unit_det");

        builder.Property(e => e.UnitKind)
            .HasMaxLength(20)
            .HasColumnName("unit_kind");

        builder.Property(e => e.UnitNbr)
            .HasMaxLength(20)
            .HasColumnName("unit_nbr");

        builder.Property(e => e.UnitType)
            .HasMaxLength(20)
            .HasColumnName("unit_type");

        builder.Property(e => e.Inactive)
            .HasColumnName("inactive");

        builder.Property(e => e.UserModified)
            .HasColumnName("user_modified");

        builder.Property(e => e.IsCollocated)
            .HasColumnName("is_collocated");

        // Relationships
        builder.HasOne(d => d.TimeZoneNavigation)
            .WithMany(p => p.CommandStructs)
            .HasForeignKey(d => d.TimeZone)
            .HasConstraintName("FK_command_struct_core_lkup_time_zone");

        // Self-referencing relationship for hierarchy
        builder.HasOne<CommandStruct>()
            .WithMany()
            .HasForeignKey(d => d.CsIdParent)
            .HasConstraintName("FK_command_struct_command_struct");

        // Indexes
        builder.HasIndex(e => e.CommandCode, "IX_command_struct_command_code");

        builder.HasIndex(e => e.Uic, "IX_command_struct_uic");

        builder.HasIndex(e => e.PasCode, "IX_command_struct_pas_code");

        builder.HasIndex(e => e.CsIdParent, "IX_command_struct_cs_id_parent");

        builder.HasIndex(e => e.Component, "IX_command_struct_component");

        builder.HasIndex(e => e.Inactive, "IX_command_struct_inactive");

        builder.HasIndex(e => new { e.Component, e.Inactive }, "IX_command_struct_component_inactive");
    }
}
