
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	8/24/2015	
-- Description:		Modified the stored procedure to select the icd_7th_Char
--					field for the following workflows: IRILO (ie FastTrack),
--					MEB, MH, MO, NE, PEPP, PW, WWD.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	1/14/2016	
-- Description:		Modified the PEPP selection to select the new 
--					waiver_required, waiver_expiration_date, and 
--					certification_date fields, and the existing DQ_Paragraph, 
--					med_off_approved, and med_off_approval_comments fields.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	2/1/2016	
-- Description:		Added new SELECT statement for the Recruiting Services (RS)
--					workflow. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	3/28/2016	
-- Description:		Added new SELECT statement for the Psychological Health (PH)
--					workflow. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	5/19/2016	
-- Description:		Updated the SELECT statement for the RS workflow to include
--					the new secondary_certification_stamp field.
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	12/7/2016	
-- Description:		Updated the IRILO workflow with a new column
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	4/17/2017	
-- Description:		Special cases no longer keep track of associated LOD cases
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	4/18/2017
-- Description:		- Added new SELECT statement for the Retention Waiver Renewal
--					(RW) workflow.
--					- Cleaned up the procedure.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	12/1/2017
-- Description:		- Added new Alternate_* fields to various workflows.
-- ============================================================================
CREATE PROCEDURE [dbo].[form348_SC_sp_Properties]
	@moduleId TINYINT,
	@scId INT
AS
BEGIN
	IF (@moduleId = 6)  -- Incaps
		SELECT	Suspense_Date,
				ISNULL(TMT_Number, '') AS TMT_Number,
				TMT_Receive_Date,
				Has_Admin_LOD,
				Case_Comments 
		FROM	Form348_SC 
		WHERE	SC_Id = @scId
	
	IF (@moduleId = 7)  -- Congress
		SELECT	Suspense_Date, 
				ISNULL(TMT_Number, '') AS TMT_Number, 
				TMT_Receive_Date 
		FROM	Form348_SC 
		WHERE	SC_Id = @scId

	IF (@moduleId = 8)  -- BMT
		SELECT	Approval_Date,
				Case_Comments,
				sub_workflow_type
		FROM	Form348_SC 
		WHERE	SC_Id = @scId

	IF (@moduleId = 9)  -- WWD
		SELECT	POC_Unit,
				POC_Phone_DSN,
				POC_Email,
				Initial_SC,
				ALC_Letter_Type,
				WWD_Docs_Attached,
				Cover_Letter_Uploaded,
				AF_Form469_Uploaded,
				Narrative_Summary_Uploaded,
				IPEB_Election,
				IPEB_Refusal,
				IPEB_Signature_Date,
				MUQ_Request_Date,
				MUQ_Upload_Date,
				MUQ_Valid,
				Cover_Ltr_Inc_Member_Statement,
				Unit_Cmdr_Memo_Uploaded,
				Med_Eval_Fact_Sheet_Sign_Date,
				Med_Eval_FS_Waiver_Sign_Date,
				Private_Physician_Docs_Uploaded,
				PS3811_Request_Date,
				PS3811_Sign_Date,
				PS3811_Uploaded,
				First_Class_Mail_Date,
				Cover_Ltr_Inc_Contact_Attempt_Details,
				Member_Letter_Uploaded,
				sig_date_poc,
				sig_name_poc,
				sig_title_poc,
				HQTech_Disposition,
				Expiration_Date,
				icd9_Id,
				icd9_description,
				FT_Diagnosis,
				Return_To_Duty_Date,
				DQ_Paragraph,
				DQ_Completion_Date,
				SAF_Letter_Upload_Date,
				memo_template_id,
				RMU_Name,
				Med_Group_Name,
				Code37_Init_Date,
				icd_7th_Char,
				Alternate_DQ_Paragraph,
				Alternate_DQ_Completion_Date,
				Alternate_Expiration_Date,
				Alternate_Return_To_Duty_Date,
				Alternate_ALC_Letter_Type,
				Alternate_memo_template_id
		FROM	Form348_SC 
		WHERE	SC_Id = @scId

	IF (@moduleId = 10)  -- PWaivers
		SELECT	Approval_Date,
				Expiration_Date,
				PWaiver_Category,
				PWaiver_Category_Text,
				PWaiver_Length,
				icd9_Id,
				icd9_description,
				FT_Diagnosis,
				icd_7th_Char,
				Alternate_Approval_Date,
				Alternate_Expiration_Date,
				Alternate_PWaiver_Length
		FROM	Form348_SC 
		WHERE	SC_Id = @scId

	IF (@moduleId = 11)  -- MEB
		SELECT	ALC_Letter_Type,
				Has_Admin_LOD,
				Initial_SC,
				Notification_Date,
				Med_Group_Name,
				RMU_Name,
				Member_Status_ID,
				icd9_Id,
				icd9_description,
				FT_Diagnosis,
				DAWG_Recommendation,
				ApprovingAuthorityType,
				Expiration_Date,
				Return_To_Duty_Date,
				Approval_Date,
				ForwardDate,
				med_off_concur,
				memo_template_id,
				Code37_Init_Date,
				icd_7th_Char,
				Alternate_med_off_concur,
				Alternate_ALC_Letter_Type,
				Alternate_memo_template_id
		FROM	Form348_SC 
		WHERE	SC_Id = @scId

	IF (@moduleId = 12)  -- BCMR
		SELECT	ISNULL(TMT_Number, '') AS TMT_Number, 
				TMT_Receive_Date, 
				Suspense_Date 
		FROM	Form348_SC 
		WHERE	SC_Id = @scId

	IF (@moduleId = 13)  -- Fast Track
		SELECT	Fast_Track_Type,
				ALC_Letter_Type,
				TMT_Receive_Date,
				DAFSC,
				DAFSC_Is_Suitable,
				Years_Satisfactory_Service,
				Body_Mass_Index,
				Requires_Specialist_For_Mgmt,
				Missed_Work_Days,
				Had_ER_Urgent_Care_Visits,
				ER_Urgent_Care_Visit_Details,
				Had_Hospitalizations,
				Hospitalization_List,
				Risk_For_Sudden_Incapacitation,
				FT_Diagnosis,
				DX_Interferes_With_Duties,
				FT_Prognosis,
				FT_Treatment,
				FT_Medications_And_Dosages,
				Recommended_Follow_Up_Interval,
				Daytime_Somnolence,
				Day_Sleep_Description,
				Has_Apnea_Episodes,
				Apnea_Episode_Description,
				Sleep_Study_Results,
				Oral_Devices_Used,
				CPAP_Required,
				BIPAP_Required,
				Response_To_Devices,
				Fasting_Blood_Sugar,
				HgbA1C,
				Current_Optometry_Exam,
				Has_Significant_Conditions,
				Other_Significant_Conditions_List,
				Controlled_With_Oral_Agents,
				Oral_Agents_List,
				Requires_Insulin,
				Insulin_Dosage_Regime,
				Requires_Non_Insulin_Med,
				Pulmonary_Function_Test,
				Methacholine_Challenge,
				Requires_Daily_Steroids,
				Daily_Steroids_Dosage,
				Rescue_Inhaler_Usage_Frequency,
				Symptoms_Exacerbated_By_Cold_Or_Exercise,
				Exercise_Or_Cold_Exacerbated_Symptom_Description,
				Exacerbated_Symptoms_Req_Oral_Steroids,
				Exacerbated_Symptoms_Oral_Steroids_Dosage,
				Normal_PFT_With_Treatment,
				HO_Intubation,
				DAWG_Recommendation,
				RMU_Initials,
				icd9_Id,
				icd9_description,
				Return_To_Duty_Date,
				DQ_Paragraph,
				DQ_Completion_Date,
				memo_template_id,
				RMU_Name,
				Med_Group_Name,
				icd_7th_Char,
				process,
				Alternate_DQ_Paragraph,
				Alternate_DQ_Completion_Date,
				Alternate_Expiration_Date,
				Alternate_Return_To_Duty_Date,
				Alternate_process,
				Alternate_ALC_Letter_Type,
				Alternate_memo_template_id
		FROM	Form348_SC 
		WHERE	SC_Id = @scId

	IF (@moduleId = 14)  -- CMAS
		SELECT	Case_Comments, 
				Date_In, 
				Date_Out 
		FROM	Form348_SC
		WHERE	SC_Id = @scId

	IF (@moduleId = 15)  -- MEPS
		SELECT	NULL
	
	IF (@moduleId = 16)  -- MMSO
		SELECT	fsc.Unit_POC_name,
				fsc.Unit_POC_rank,
				fsc.Unit_POC_title,
				cs.ADDRESS1 AS Unit_Address1,
				cs.ADDRESS2 AS Unit_Address2,
				cs.CITY AS Unit_City,
				cs.STATE AS Unit_State,
				cs.POSTAL_CODE AS Unit_Zip,
				fsc.Unit_POC_Phone,
				CASE WHEN ISNULL(Member_Address_Street, '') = '' THEN md.LOCAL_ADDR_STREET ELSE Member_Address_Street END AS Member_Street,
				CASE WHEN ISNULL(Member_Address_City, '') = '' THEN md.LOCAL_ADDR_CITY ELSE Member_Address_City END AS Member_City,
				CASE WHEN ISNULL(Member_Address_State, '') = '' THEN md.LOCAL_ADDR_STATE ELSE Member_Address_State END AS Member_State,
				CASE WHEN ISNULL(Member_Address_Zip, '') = '' THEN md.ZIP ELSE Member_Address_Zip END AS Member_Zip,
				CASE WHEN ISNULL(Member_Home_Phone, '') = '' THEN md.HOME_PHONE ELSE Member_Home_Phone END AS Member_Phone,
				Member_Tricare_Region,
				CASE 
					WHEN ISNULL(Military_Treatment_Facility_Initial, '') = '' THEN 
						CASE
							WHEN med.medical_facility_type = 'Military' THEN med.medical_facility 
							ELSE '' 
						END 
					ELSE Military_Treatment_Facility_Initial 
				END AS Military_Treatment_Facility,
				CASE WHEN ISNULL(MTF_Initial_Treatment_Date, '') = '' THEN med.treatment_date ELSE MTF_Initial_Treatment_Date END AS Military_Treatment_Date,
				CASE WHEN ISNULL(fsc.FT_Diagnosis, '') = '' THEN med.diagnosis_text ELSE FT_Diagnosis END AS Medical_Diagnosis,
				Follow_Up_Care,
				Medical_Provider,
				fsc.POC_Email,
				fsc.POC_Phone_DSN,
				MTF_Suggested,
				MTF_Suggested_Distance,
				MTF_Suggested_Choice,
				Medical_Profile_Info,
				fsc.Date_In,
				fsc.Date_Out,
				ISNULL(icd9.Value,'') + ' ' + ISNULL(icd9.text,'') AS icd9Id,
				CASE WHEN ISNULL(fsc.member_unit, '') = '' THEN oc.member_unit ELSE fsc.Member_Unit END AS unit_name,
				cs.UIC,
				fsc.Injury_Illness_Date
		FROM	Form348_SC fsc
				INNER JOIN MemberData md On md.SSAN = fsc.Member_ssn
				INNER JOIN Command_Struct cs On cs.CS_ID = fsc.Member_Unit_Id
				JOIN core_AssociatedCases alod on alod.refId = fsc.SC_Id and alod.workflowId = fsc.workflow
				INNER JOIN Form348_Medical med On med.lodid = alod.associated_refId
				INNER JOIN Form348 oc On oc.lodId = alod.associated_refId
				LEFT OUTER JOIN core_lkupGrade clg On clg.CODE = Unit_POC_rank		
				LEFT OUTER JOIN core_lkupICD9 icd9 on icd9.ICD9_ID = med.icd9Id
		WHERE	SC_Id = @scId
	
	IF (@moduleId = 17)  -- Medical Holds (MH)
		SELECT	Initial_SC,
				high_tenure_date,
				icd9_Id,
				icd9_description,
				FT_Diagnosis,				
				med_off_approved,
				med_off_approval_comment,
				Expiration_Date,
				sig_date_med_off,
				ForwardDate,
				Notification_Date,
				icd_7th_Char,
				Alternate_Expiration_Date
		FROM	Form348_SC 
		WHERE	SC_Id = @scId
	
	IF (@moduleId = 18)	-- Non-Emergent Surgery Request (NE)
		SELECT	icd9_Id,
				icd9_description,
				FT_Diagnosis,
				surgery_date,				
				med_off_approved,
				med_off_approval_comment,
				icd_7th_Char
		FROM	Form348_SC
		WHERE	SC_Id = @scId
	
	IF (@moduleId = 19) -- Deployment Waiver (DW)
		SELECT	majcom,
				sim_deployment,
				deploy_start_date,
				deploy_end_date,
				deploy_location,
				line_number,
				line_remarks,
				med_off_approved,
				med_off_approval_comment
		FROM	Form348_SC
		WHERE	SC_Id = @scId
	
	IF (@moduleId = 20)	-- Modifications (MO)
		SELECT	Has_Admin_LOD,
				icd9_Id,
				icd9_description,
				FT_Diagnosis,
				associated_SC,
				case_type,
				justification,
				high_tenure_date,
				med_off_approved,
				med_off_approval_comment,
				icd_7th_Char
		FROM	Form348_SC
		WHERE	SC_Id = @scId
	
	IF (@moduleId = 21)	-- Physical Examination Processing Program (PEPP)
		SELECT	icd9_Id,
				icd9_description,
				FT_Diagnosis,
				case_type,
				sub_workflow_type,
				pepp_type,
				rating,
				HQTech_Disposition,
				renewal,
				base_assign,
				completed_by_unit,
				ALC_Letter_Type,
				type_name,
				rating_name,
				date_received,
				Date_Out,
				Expiration_Date,
				icd_7th_Char,
				waiver_required,
				DQ_Paragraph,
				certification_date,
				med_off_approved,
				med_off_approval_comment,
				waiver_expiration_date,
				Alternate_DQ_Paragraph,
				Alternate_Expiration_Date,
				Alternate_certification_date
		FROM	Form348_SC
		WHERE	SC_Id = @scId

	IF (@moduleId = 22)	-- Recuriting Services (RS)
		SELECT	icd9_Id,
				icd9_description,
				FT_Diagnosis,
				case_type,
				sub_workflow_type,
				rating,
				HQTech_Disposition,
				base_assign,
				completed_by_unit,
				ALC_Letter_Type,
				type_name,
				rating_name,
				date_received,
				icd_7th_Char,
				sub_case_type,
				certification_stamp,
				secondary_certification_stamp,
				free_text,
				secondary_free_text,
				completed_by_unit_name,
				case_type_name,
				sub_case_type_name,
				med_off_approved,
				med_off_approval_comment,
				memo_template_id,
				stamped_doc_id,
				med_off_prev_disposition,
				Expiration_Date,
				alt_ALC_letter_type,
				Alternate_alt_ALC_letter_type,
				Alternate_Expiration_Date
		FROM	Form348_SC
		WHERE	SC_Id = @scId

	IF (@moduleId = 23)	-- PH Non-Clinical Tracking (PH)
		SELECT	sig_date_unit_ph,
				sig_name_unit_ph,
				sig_title_unit_ph,
				sig_date_hq_dph,
				sig_name_hq_dph,
				sig_title_hq_dph,
				ph_wing_rmu_id,
				ph_user_id,
				is_delinquent,
				ph_reporting_period,
				ph_last_modified
		FROM	Form348_SC
		WHERE	SC_Id = @scId

	IF (@moduleId = 27)	-- Retention Waiver Renewal (RW)
		SELECT	associated_SC,
				renewal_date,
				DAFSC,
				RMU_Name,
				Med_Group_Name,
				Years_Satisfactory_Service,
				Body_Mass_Index,
				Requires_Specialist_For_Mgmt,
				Missed_Work_Days,
				Had_ER_Urgent_Care_Visits,
				ER_Urgent_Care_Visit_Details,
				Had_Hospitalizations,
				Hospitalization_List,
				Risk_For_Sudden_Incapacitation,
				icd9_Id,
				icd9_description,
				FT_Diagnosis,
				FT_Prognosis,
				FT_Treatment,
				FT_Medications_And_Dosages,
				DX_Interferes_With_Duties,
				DAWG_Recommendation,
				Recommended_Follow_Up_Interval,
				Expiration_Date,
				Return_To_Duty_Date,
				process,
				DQ_Paragraph,
				DQ_Completion_Date,
				ALC_Letter_Type,
				memo_template_id,
				icd_7th_Char,
				Alternate_DQ_Paragraph,
				Alternate_DQ_Completion_Date,
				Alternate_Expiration_Date,
				Alternate_Return_To_Duty_Date,
				Alternate_process,
				Alternate_ALC_Letter_Type,
				Alternate_memo_template_id
		FROM	Form348_SC
		WHERE	SC_Id = @scId
END
GO

