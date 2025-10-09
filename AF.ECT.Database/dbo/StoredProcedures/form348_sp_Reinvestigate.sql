
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	Creates a new LOD case for a Reinvestigation Request (RR) which 
--				was approved. If the case already exists then the case is 
--				updated. Data from the LOD case which the RR case revolved 
--				around is used to populate many of the fields for the new LOD 
--				case. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	12/7/2015
-- Work Item:		TFS Task 289
-- Description:		1) Changed the value of the @newStatus variable from 1 
--					(Med Tech Input) to be the ID for the Formal Action by
--					Appointing Authority step.
--					2) Made it so when a new LOD case is inserted it is
--					marked as a formal case. 
--					3) Changed the size of @newCaseId from 20 to 50. 
--					4) Made it so a Form261 record is created for the new
--					reinvestigation LOD case. This is required because the
--					case is now formal when it starts. 
--					5) Made it so the IO findings for the old LOD case is
--					copies over for the new LOD case. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	8/2/2016
-- Description:		Updated to use the most recently created LOD workflow when
--					selecting the initial workstatus ID of the new case.
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	10/27/2016
-- Description:		Adjusted to accomodate new LOD workflow fields
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/9/2017
-- Description:		- Modified to use the RR cases member info instead of
--					the LODs member info.
-- ============================================================================
CREATE PROCEDURE [dbo].[form348_sp_Reinvestigate]
(
	@userId int,
	@requestId int
)
AS 
 

SET NOCOUNT ON;

/* Get the  new caseID */
	 
Declare	@oldId int,
	@newId int,
	@newStatus int,
	@newWorkflow int,
	@newCaseId varchar(50),
	@newExists bit = 0,
	@pTypeId INT = 0,
	@memberSSN NVARCHAR(9),
	@currentMemberUnit NVARCHAR(100) = '',
	@currentMemberUnitId INT = 0,
	@currentMemberCompo CHAR(1),
	@currentMemberGrade INT

BEGIN

	SELECT	@pTypeId = pt.Id
	FROM	core_lkupPersonnelTypes pt
	WHERE	pt.Description = 'IO'

	-- Get the currently active Line of Duty workflow...
	SET @newWorkflow =	(
							SELECT	TOP 1 w.workflowId
							FROM	core_Workflow w
									JOIN core_lkupModule m ON w.moduleId = m.moduleId
							WHERE	m.moduleName = 'LOD'
							ORDER	BY w.workflowId DESC
						)

	SELECT	@newStatus = ws.ws_id
	FROM	core_WorkStatus ws
			JOIN core_StatusCodes sc ON ws.statusId = sc.statusId
	WHERE	sc.description = 'Formal Action by Appointing Authority'
			AND ws.workflowId = @newWorkflow

	SELECT	@oldId = r.InitialLodId,
			@newId = r.ReinvestigationLodId,
			@memberSSN = r.member_ssn,
			@currentMemberCompo = r.member_compo,
			@currentMemberGrade = r.member_grade,
			@currentMemberUnit = r.member_unit,
			@currentMemberUnitId = r.member_unit_id 
	FROM	Form348_RR r
	WHERE	r.request_id = @requestId

	-- Update member information if necessary
	SELECT	@currentMemberCompo = md.SVC_COMP,
			@currentMemberGrade = md.GR_CURR,
			@currentMemberUnitId = cs.CS_ID,
			@currentMemberUnit = cs.LONG_NAME
	FROM	MemberData md
			JOIN Command_Struct cs ON md.PAS_NUMBER = cs.PAS_CODE
	WHERE	md.SSAN = @memberSSN

	If ISNULL(@oldId, 0) = 0
	BEGIN
		-- Do Nothing, as there is no connected LOD
		Select -1
	END
	ELSE
	BEGIN

		If ISNULL(@newId, 0) <> 0 
		BEGIN
			Select @newExists = 1 from Form348 where lodId = @newId
		END
		
		-- Create/Find the new LOD
		If ((IsNull(@newId, 0) = 0 ) Or (@newExists = 0))
		BEGIN 
			SET @newCaseId=dbo.GetNextCaseId(@oldId)
			;
			INSERT INTO Form348 (
				case_id
				, workflow
				, status
				, member_name
				, member_ssn
				, member_grade
				, member_unit
				, member_unit_id
				, member_DOB
				, member_compo
				, doc_group_id
				, formal_inv
				, med_tech_comments
				, appAuthUserId
				, deleted
				, rwoa_reason
				, rwoa_explantion
				, rwoa_date
				, FinalDecision
				, Board_For_General_YN
				, to_unit
				, from_unit
				, FinalFindings
				, sarc
				, restricted
				, aa_ptype
				, created_by
				, created_date
				, modified_by
				, modified_date
				,tmn
				,tms
				, appointing_unit
				)
			SELECT
				@newCaseId
				, @newWorkflow
				, @newStatus
				, member_name
				, member_ssn
				, @currentMemberGrade
				, @currentMemberUnit
				, @currentMemberUnitId
				, member_DOB
				, @currentMemberCompo
				, doc_group_id
				, 1
				, med_tech_comments
				, appAuthUserId
				, deleted
				, rwoa_reason
				, rwoa_explantion
				, rwoa_date
				, FinalDecision
				, Board_For_General_YN
				, to_unit
				, from_unit
				, FinalFindings
				, sarc
				, restricted
				, aa_ptype
				, @userId  -- created
				, GETDATE()
				, @userId  -- modified
				, GETDATE()
				,tmn
				,tms
				, appointing_unit
			FROM Form348
			Where lodId = @oldId
			;
			Select @newId = SCOPE_IDENTITY()
		END
		ELSE
		BEGIN 
			--Select @newCaseId = case_id from Form348 where lodId = @newId
			
			--If IsNull(@newCaseId, 0) = 0
			--BEGIN
				SET @newCaseId=dbo.GetNextCaseId(@oldId)
			--END
			
			UPDATE 
				form348 
			SET  
				case_id =@newCaseId
				,workflow=@newWorkflow
				, status = @newStatus
				,member_name=a.member_name
				,member_ssn=a.member_ssn
				,member_grade=@currentMemberGrade
				,member_unit=@currentMemberUnit
				,member_unit_id=@currentMemberUnitId
				,member_DOB=a.member_DOB
				,member_compo=@currentMemberCompo
				,doc_group_id=a.doc_group_id
				,formal_inv= 1
				,med_tech_comments=a.med_tech_comments
				,appAuthUserId=a.appAuthUserId
				,deleted=a.deleted
				,rwoa_reason=a.rwoa_reason
				,rwoa_explantion=a.rwoa_explantion
				,rwoa_date=a.rwoa_date
				,FinalDecision=a.FinalDecision
				,Board_For_General_YN=a.Board_For_General_YN
				--,sig_date_unit_commander=a.sig_date_unit_commander
				--,sig_name_unit_commander=a.sig_name_unit_commander
				--,sig_date_med_officer=a.sig_date_med_officer
				--,sig_name_med_officer=a.sig_name_med_officer
				--,sig_date_legal=a.sig_date_legal
				--,sig_name_legal=a.sig_name_legal
				--,sig_date_appointing=a.sig_date_appointing
				--,sig_name_appointing=a.sig_name_appointing
				--,sig_date_board_legal=a.sig_date_board_legal
				--,sig_name_board_legal=a.sig_name_board_legal
				--,sig_date_board_medical=a.sig_date_board_medical
				--,sig_name_board_medical=a.sig_name_board_medical
				--,sig_date_board_admin=a.sig_date_board_admin
				--,sig_name_board_admin=a.sig_name_board_admin
				--,sig_date_approval=a.sig_date_approval
				--,sig_name_approval=a.sig_name_approval
				--,sig_title_approval=a.sig_title_approval
				,to_unit=a.to_unit
				,from_unit=a.from_unit
				,FinalFindings=a.FinalFindings
				--,io_completion_date=a.io_completion_date
				--,io_instructions=a.io_instructions
				--,io_poc_info=a.io_poc_info
				,sarc=a.sarc
				,restricted=a.restricted
			--	,io_ssn=a.io_ssn
				,aa_ptype=a.aa_ptype
				, modified_by = @userId
				, modified_date = GETDATE()
				, tmn = a.tmn
				, tms = a.tms
				, appointing_unit = a.appointing_unit
			FROM
				(SELECT * FROM form348  WHERE  lodid=@oldId ) a
			WHERE
				form348.lodid =@newId
		END


		-- Update the Reinvestigation Requests table
			UPDATE Form348_RR
			Set ReinvestigationLodId = @newId
			Where request_id = @requestId


		-- Create/Update the Units table
		If Not Exists(Select 1 From Form348_Unit Where lodid = @newId)
		BEGIN
			INSERT INTO Form348_Unit (
				lodid
				, cmdr_circ_details
				, cmdr_duty_determination
				, cmdr_duty_from
				, cmdr_duty_others
				, cmdr_duty_to
				, cmdr_activated_yn
				, modified_by
				, modified_date
				, source_information
				, witnesses
				, source_information_specify
				, member_occurrence
				, absent_from
				, absent_to
				, member_on_orders
				, member_credible
				, proximate_cause
				, proximate_cause_specify
				, workflow
			)
			Select @newId
				, a.cmdr_circ_details
				, a.cmdr_duty_determination
				, a.cmdr_duty_from
				, a.cmdr_duty_others
				, a.cmdr_duty_to
				, a.cmdr_activated_yn
				, a.modified_by
				, a.modified_date
				, a.source_information
				, a.witnesses
				, a.source_information_specify
				, a.member_occurrence
				, a.absent_from
				, a.absent_to
				, (CASE a.workflow WHEN 27 THEN a.member_on_orders ELSE (CASE b.wasMemberOnOrders WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE NULL END) END)
				, (CASE a.workflow WHEN 27 THEN a.member_credible ELSE (CASE b.hasCredibleService WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE NULL END) END)
				, a.proximate_cause
				, a.proximate_cause_specify
				, 27
			From Form348_Unit a
			JOIN Form348 b ON b.lodId=a.lodid
			Where a.lodid = @oldId
		END
		ELSE
		BEGIN
			UPDATE form348_unit
				SET  cmdr_circ_details=a.cmdr_circ_details
					, cmdr_duty_determination=a.cmdr_duty_determination
					, cmdr_duty_from= a.cmdr_duty_from
					, cmdr_duty_others =a.cmdr_duty_others
					, cmdr_duty_to=a.cmdr_duty_to
					, cmdr_activated_yn= a.cmdr_activated_yn
					, modified_date= a.modified_date 
					, modified_by= a.modified_by
					, source_information = a.source_information
					, witnesses = a.witnesses
					, source_information_specify = a.source_information_specify
					, member_occurrence = a.member_occurrence
					, absent_from = a.absent_from
					, absent_to = a.absent_to
					, member_on_orders = (CASE a.workflow WHEN 27 THEN a.member_on_orders ELSE (CASE b.wasMemberOnOrders WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE NULL END) END)
					, member_credible = (CASE a.workflow WHEN 27 THEN a.member_credible ELSE (CASE b.hasCredibleService WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE NULL END) END)
					, proximate_cause = a.proximate_cause
					, proximate_cause_specify = a.proximate_cause_specify
					, workflow = 27
			   FROM
				 (SELECT * FROM form348_unit  WHERE  lodid=@oldId ) a
				 JOIN Form348 b ON b.lodId = @oldId 
			   WHERE
				 Form348_Unit.lodid =@newId
		END


		-- Create/Update the Medical table
		If Not Exists(Select 1 From Form348_Medical Where lodid = @newId)
		BEGIN
			INSERT INTO Form348_Medical (
				lodid
				, member_status
				, event_nature_type
				, event_nature_details
				, medical_facility
				, medical_facility_type
				, treatment_date
				, death_involved_yn
				, mva_involved_yn
				, icd9Id
				, epts_yn
				, physician_approval_comments
				, modified_by
				, modified_date
				, physician_cancel_reason
				, physician_cancel_explanation
				, diagnosis_text
				, icd_7th_Char
				, member_from
				, member_component
				, member_category
				, influence
				, member_responsible
				, psych_eval
				, psych_date
				, relevant_condition
				, other_test
				, other_test_date
				, deployed_location
				, condition_epts
				, service_aggravated
				, mobility_standards
				, member_condition
				, alcohol_test_done
				, drug_test_done
				, board_finalization
				, workflow
			)
			SELECT 
				@newId
				, (CASE a.member_status WHEN 'active' THEN a.member_status WHEN 'inactive' THEN a.member_status ELSE NULL END)
				, event_nature_type
				, event_nature_details
				, medical_facility
				, medical_facility_type
				, treatment_date
				, death_involved_yn
				, mva_involved_yn
				, icd9Id
				, epts_yn
				, physician_approval_comments
				, modified_by
				, modified_date
				, physician_cancel_reason
				, physician_cancel_explanation
				, diagnosis_text
				, icd_7th_Char
				, member_from
				, member_component
				, (CASE a.workflow WHEN 27 THEN a.member_category ELSE (CASE a.member_status WHEN 'active' THEN NULL 
																							 WHEN 'inactive' THEN NULL 
																							 ELSE (SELECT TOP 1 member_status_id FROM core_lkupMemberCategory b WHERE b.Member_Status_Desc = a.member_status) END) END)
				, influence
				, member_responsible
				, psych_eval
				, psych_date
				, relevant_condition
				, other_test
				, other_test_date
				, deployed_location
				, condition_epts
				, service_aggravated
				, mobility_standards
				, member_condition
				, alcohol_test_done
				, drug_test_done
				, board_finalization
				, 27
			FROM Form348_Medical a
			Where lodid = @oldId
		END
		ELSE
		BEGIN

			UPDATE form348_medical
				SET
					 member_status=a.member_status
					,event_nature_type=a.event_nature_type
					,event_nature_details=a.event_nature_details
					,medical_facility=a.medical_facility
					,medical_facility_type=a.medical_facility_type
					,treatment_date=a.treatment_date
					,death_involved_yn=a.death_involved_yn
					,mva_involved_yn=a.mva_involved_yn
					,icd9Id=a.icd9Id
					,epts_yn=a.epts_yn		
					,physician_approval_comments=a.physician_approval_comments
					,modified_by=a.modified_by
					,modified_date=a.modified_date
					,physician_cancel_reason=a.physician_cancel_reason
					,physician_cancel_explanation=a.physician_cancel_explanation
					,diagnosis_text=a.diagnosis_text
					,icd_7th_Char=a.icd_7th_Char
					,member_from=a.member_from
					,member_component=a.member_component
					,member_category=a.member_category
					,influence=a.influence
					,member_responsible=a.member_responsible
					,psych_eval=a.psych_eval
					,psych_date=a.psych_date
					,relevant_condition=a.relevant_condition
					,other_test=a.other_test
					,other_test_date=a.other_test_date
					,deployed_location=a.deployed_location
					,condition_epts=a.condition_epts
					,service_aggravated=a.service_aggravated
					,mobility_standards=a.mobility_standards
					,member_condition=a.member_condition
					,alcohol_test_done=a.alcohol_test_done
					,drug_test_done=a.drug_test_done
					,board_finalization=a.board_finalization
					,workflow=27

				 FROM
				 (SELECT * FROM form348_medical  WHERE  lodid=@oldId ) a
   				  WHERE
				   form348_medical.lodid =@newId
		END
		
		-- Create/Update the Form261 (Investigation) table record...
		IF NOT EXISTS(SELECT 1 From Form261 WHERE lodId = @newId)
		BEGIN
			INSERT	INTO	Form261 (
								lodId, 
								reportDate,
								investigationOf,
								status,
								inactiveDutyTraining,
								durationStartDate,
								durationFinishDate,
								IoUserId,
								otherPersonnels,
								final_approval_findings,
								findingsDate,
								place,
								howSustained,
								medicalDiagnosis,
								presentForDuty,
								absentWithAuthority,
								intentionalMisconduct,
								mentallySound,
								remarks,
								modified_by,
								modified_date,
								sig_date_io,
								sig_info_io,
								sig_date_appointing,
								sig_info_appointing
							)
					VALUES	(@newId,
							NULL,
							NULL,
							NULL,
							NULL,
							NULL,
							NULL,
							NULL,
							NULL,
							NULL,
							NULL,
							NULL,
							NULL,
							NULL,
							NULL,
							NULL,
							NULL,
							NULL,
							NULL,
							@userId,
							GETDATE(),
							NULL,
							NULL,
							NULL,
							NULL)
		END

		-- Create/Update the Findings table
		If Not Exists(Select * From FORM348_findings Where lodid = @newId)
		BEGIN
			INSERT INTO FORM348_findings (
				lodid
				,ptype
				,ssn
				,name
				,grade
				,compo
				,rank
				,pascode
				,finding
				,decision_yn
				,explanation
				,modified_by 
				,modified_date
				,created_by
				,created_date
				,FindingsText
			)
			SELECT 
				@newId
				,ptype
				,ssn
				,name
				,grade
				,compo
				,rank
				,pascode
				,finding
				,decision_yn
				,explanation
				,modified_by 
				,modified_date
				,created_by
				,created_date
				,FindingsText
			FROM FORM348_findings
			Where lodid = @oldId AND ptype IN (3,4,5,6,7,8,9,10,22)
		END
		ELSE
		BEGIN

			UPDATE FORM348_findings
				SET
				ptype = a.ptype
				,ssn = a.ssn
				,name = a.name
				,grade = a.grade
				,compo = a.compo
				,rank = a.rank
				,pascode = a.pascode
				,finding = a.finding
				,decision_yn = a.decision_yn
				,explanation = a.explanation
				,modified_by = a.modified_by
				,modified_date = a.modified_date
				,created_by = a.created_by
				,created_date = a.created_date
				,FindingsText = a.FindingsText

				 FROM
				 (SELECT * FROM FORM348_findings  WHERE  lodid=@oldId ) a
   				  WHERE
				   FORM348_findings.lodid =@newId
		END
	END
END
GO

