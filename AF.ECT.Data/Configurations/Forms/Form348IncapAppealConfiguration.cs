using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Entity Framework Core configuration for the <see cref="Form348IncapAppeal"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the Form_348_Incap_Appeal table,
/// which stores appeal decisions for incapacitation (INCAP) cases on Form 348.
/// Contains approval flags for various reviewing authorities (WCC, OPR, OCR, DOS, CCR, VCR, DOP, CAFR)
/// with reference to the incapacitation finding and extension. Used for tracking appeal workflow
/// through multiple approval levels for incapacitation determinations.
/// </remarks>
public class Form348IncapAppealConfiguration : IEntityTypeConfiguration<Form348IncapAppeal>
{
    /// <summary>
    /// Configures the entity of type <see cref="Form348IncapAppeal"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<Form348IncapAppeal> builder)
    {
        // Table mapping
        builder.ToTable("Form_348_Incap_Appeal", "dbo");

        // Primary key
        builder.HasKey(e => e.ApId)
            .HasName("PK_Form_348_Incap_Appeal");

        // Properties configuration
        builder.Property(e => e.ApId)
            .HasColumnName("AP_ID")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.ScId)
            .IsRequired()
            .HasColumnName("SC_ID");

        builder.Property(e => e.ExtId)
            .HasColumnName("EXT_ID");

        builder.Property(e => e.WccAppealApproval)
            .HasColumnName("WCC_Appeal_Approval");

        builder.Property(e => e.OprAppealApproval)
            .HasColumnName("OPR_Appeal_Approval");

        builder.Property(e => e.OcrAppealApproval)
            .HasColumnName("OCR_Appeal_Approval");

        builder.Property(e => e.DosAppealApproval)
            .HasColumnName("DOS_Appeal_Approval");

        builder.Property(e => e.CcrAppealApproval)
            .HasColumnName("CCR_Appeal_Approval");

        builder.Property(e => e.VcrAppealApproval)
            .HasColumnName("VCR_Appeal_Approval");

        builder.Property(e => e.DopAppealApproval)
            .HasColumnName("DOP_Appeal_Approval");

        builder.Property(e => e.CafrAppealApproval)
            .HasColumnName("CAFR_Appeal_Approval");

        // Relationships
        builder.HasOne(e => e.Sc)
            .WithMany(e => e.Form348IncapAppeals)
            .HasForeignKey(e => e.ScId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Form_348_Incap_Appeal_Finding");

        // Indexes
        builder.HasIndex(e => e.ScId, "IX_form_348_incap_appeal_sc_id");

        builder.HasIndex(e => e.ExtId, "IX_form_348_incap_appeal_ext_id");
        
        builder.HasIndex(e => e.WccAppealApproval, "IX_form_348_incap_appeal_wcc");
        
        builder.HasIndex(e => e.OprAppealApproval, "IX_form_348_incap_appeal_opr");
        
        builder.HasIndex(e => e.CafrAppealApproval, "IX_form_348_incap_appeal_cafr");
    }
}
