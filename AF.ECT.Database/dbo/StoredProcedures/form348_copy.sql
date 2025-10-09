
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/27/2015
-- Work Item:		TFS Task 289
-- Description:		1) Changed the size of @newCaseId from 20 to 50.
--					2) Added an ORDER BY clause to the Form348_RR SELECT 
--					statement. This was done because multilple RR cases can 
--					now be associated with a single LOD case. This stored 
--					procedure now makes the assumption that the caller wants 
--					the MOST RECENT RR case with an initialLodId equal to 
--					@lodId. 
-- ============================================================================
CREATE PROCEDURE [dbo].[form348_copy]
(
	@userId int,
	@oldId int,
	@newId int 
)

AS 
 

/* Get the  new caseID */
	 
DEClARE @newCaseId varchar(50)
SET @newCaseId=dbo.GetNextCaseId(@oldId)
 
SET NOCOUNT ON;

UPDATE 
	form348 
SET  
	case_id =@newCaseId
	,workflow=a.workflow
	,member_name=a.member_name
	,member_ssn=a.member_ssn
	,member_grade=a.member_grade
	,member_unit=a.member_unit
	,member_unit_id=a.member_unit_id
	,member_DOB=a.member_DOB
	,member_compo=a.member_compo
	,doc_group_id=a.doc_group_id
	,formal_inv=a.formal_inv
	,med_tech_comments=a.med_tech_comments
	,appAuthUserId=a.appAuthUserId
	,deleted=a.deleted
	,rwoa_reason=a.rwoa_reason
	,rwoa_explantion=a.rwoa_explantion
	 ,rwoa_date=a.rwoa_date
	,FinalDecision=a.FinalDecision
	,Board_For_General_YN=a.Board_For_General_YN
	,sig_date_unit_commander=a.sig_date_unit_commander
	,sig_name_unit_commander=a.sig_name_unit_commander
	,sig_date_med_officer=a.sig_date_med_officer
	,sig_name_med_officer=a.sig_name_med_officer
	,sig_date_legal=a.sig_date_legal
	,sig_name_legal=a.sig_name_legal
	,sig_date_appointing=a.sig_date_appointing
	,sig_name_appointing=a.sig_name_appointing
	,sig_date_board_legal=a.sig_date_board_legal
	,sig_name_board_legal=a.sig_name_board_legal
	,sig_date_board_medical=a.sig_date_board_medical
	,sig_name_board_medical=a.sig_name_board_medical
	,sig_date_board_admin=a.sig_date_board_admin
	,sig_name_board_admin=a.sig_name_board_admin
	,sig_date_approval=a.sig_date_approval
	,sig_name_approval=a.sig_name_approval
	,sig_title_approval=a.sig_title_approval
	,to_unit=a.to_unit
	,from_unit=a.from_unit
	,FinalFindings=a.FinalFindings
	,io_completion_date=a.io_completion_date
	,io_instructions=a.io_instructions
	,io_poc_info=a.io_poc_info
	,sarc=a.sarc
	,restricted=a.restricted
	,io_ssn=a.io_ssn
	,aa_ptype=a.aa_ptype
FROM
	(SELECT * FROM form348  WHERE  lodid=@oldId ) a
WHERE
	form348.lodid =@newId



UPDATE form348_unit
	SET  cmdr_circ_details=a.cmdr_circ_details
		, cmdr_duty_determination=a.cmdr_duty_determination
		, cmdr_duty_from= a.cmdr_duty_from
		, cmdr_duty_others =a.cmdr_duty_others
		, cmdr_duty_to=a.cmdr_duty_to
		, cmdr_activated_yn= a.cmdr_activated_yn
		, modified_date= a.modified_date 
        , modified_by= a.modified_by
   FROM
	 (SELECT * FROM form348_unit  WHERE  lodid=@oldId ) a
   WHERE
	 form348_unit.lodid =@newId



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

	 FROM
	 (SELECT * FROM form348_medical  WHERE  lodid=@oldId ) a
   	  WHERE
	   form348_medical.lodid =@newId



-- Added 03/28/2012

Declare @requestId int
Select @requestId = request_id from Form348_RR
where InitialLodId = @oldId
ORDER BY Case_Id ASC

Update Form348_RR
Set ReinvestigationLodId = @newId
Where request_id = @requestId
GO

