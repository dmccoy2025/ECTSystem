using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Entity Framework Core configuration for the <see cref="Form348IncapExt"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the Form_348_Incap_Ext table,
/// which stores extension requests for incapacitation (INCAP) cases on Form 348.
/// Contains extension dates, recommendations from medical and investigating commander,
/// Wing JA concurrence, financial impact, approval flags for various reviewing authorities
/// (WCC, OPR, OCR, DOS, CCR, VCR, DOP, CAFR), and AMRO (Active Member Retention Office)
/// medical status tracking including IRILO (Individual Ready Reserve Inactive List Order).
/// Used for extending incapacitation periods beyond initial determination.
/// </remarks>
public class Form348IncapExtConfiguration : IEntityTypeConfiguration<Form348IncapExt>
{
    /// <summary>
    /// Configures the entity of type <see cref="Form348IncapExt"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<Form348IncapExt> builder)
    {
        // Table mapping
        builder.ToTable("Form_348_Incap_Ext", "dbo");

        // Primary key
        builder.HasKey(e => e.ExtId)
            .HasName("PK_Form_348_Incap_Ext");

        // Properties configuration
        builder.Property(e => e.ExtId)
            .HasColumnName("EXT_ID")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.ScId)
            .IsRequired()
            .HasColumnName("SC_ID");

        builder.Property(e => e.ExtNumber)
            .HasColumnName("Ext_Number");

        builder.Property(e => e.ExtStartDate)
            .HasColumnName("Ext_Start_Date")
            .HasColumnType("datetime");

        builder.Property(e => e.ExtEndDate)
            .HasColumnName("Ext_End_Date")
            .HasColumnType("datetime");

        builder.Property(e => e.MedExtRecommendation)
            .HasColumnName("Med_Ext_Recommendation");

        builder.Property(e => e.ICExtRecommendation)
            .HasColumnName("IC_Ext_Recommendation");

        builder.Property(e => e.WJaConcurWithIc)
            .HasColumnName("W_JA_Concur_With_IC");

        builder.Property(e => e.FinIncomeLost)
            .HasColumnName("Fin_Income_Lost");

        builder.Property(e => e.WccExtApproval)
            .HasColumnName("WCC_Ext_Approval");

        builder.Property(e => e.OprExtApproval)
            .HasColumnName("OPR_Ext_Approval");

        builder.Property(e => e.OcrExtApproval)
            .HasColumnName("OCR_Ext_Approval");

        builder.Property(e => e.DosExtApproval)
            .HasColumnName("DOS_Ext_Approval");

        builder.Property(e => e.CcrExtApproval)
            .HasColumnName("CCR_Ext_Approval");

        builder.Property(e => e.VcrExtApproval)
            .HasColumnName("VCR_Ext_Approval");

        builder.Property(e => e.DopExtApproval)
            .HasColumnName("DOP_Ext_Approval");

        builder.Property(e => e.CafrExtApproval)
            .HasColumnName("CAFR_Ext_Approval");

        builder.Property(e => e.MedAmrostartDate)
            .HasColumnName("Med_AMROStartDate")
            .HasColumnType("datetime");

        builder.Property(e => e.MedAmroendDate)
            .HasColumnName("Med_AMROEndDate")
            .HasColumnType("datetime");

        builder.Property(e => e.MedAmrodisposition)
            .HasMaxLength(500)
            .HasColumnName("Med_AMRODisposition");

        builder.Property(e => e.MedNextAmrostartDate)
            .HasColumnName("Med_NextAMROStartDate")
            .HasColumnType("datetime");

        builder.Property(e => e.MedNextAmroendDate)
            .HasColumnName("Med_NextAMROEndDate")
            .HasColumnType("datetime");

        builder.Property(e => e.MedIrilostatus)
            .HasMaxLength(500)
            .HasColumnName("Med_IRILOStatus");

        // Relationships
        builder.HasOne(e => e.Sc)
            .WithMany(e => e.Form348IncapExts)
            .HasForeignKey(e => e.ScId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Form_348_Incap_Ext_Finding");

        // Indexes
        builder.HasIndex(e => e.ScId, "IX_form_348_incap_ext_sc_id");

        builder.HasIndex(e => e.ExtStartDate, "IX_form_348_incap_ext_start_date");

        builder.HasIndex(e => e.ExtEndDate, "IX_form_348_incap_ext_end_date");
        
        builder.HasIndex(e => e.ExtNumber, "IX_form_348_incap_ext_number");
        
        builder.HasIndex(e => e.MedExtRecommendation, "IX_form_348_incap_ext_med_recommendation");
        
        builder.HasIndex(e => e.ICExtRecommendation, "IX_form_348_incap_ext_ic_recommendation");
    }
}
