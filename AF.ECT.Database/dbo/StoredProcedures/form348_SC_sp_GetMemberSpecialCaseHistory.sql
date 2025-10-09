
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/30/2017
-- Description:	Gets the special case history for a specified member. This 
--				procedure is meant to be used for the Member in the workflows.
-- ============================================================================
CREATE PROCEDURE [dbo].[form348_SC_sp_GetMemberSpecialCaseHistory]
	@member_ssn VARCHAR(12),
	@userId INT
AS
BEGIN
	DECLARE @userUnit AS INT, @scope TINYINT, @rptView TINYINT, @userSSN CHAR(9)

	SELECT	@userUnit = u.unit_id, 
			@scope = u.accessscope, 
			@userSSN = u.SSN,
			@rptView = u.ViewType
	FROM	vw_Users u
	WHERE	u.userId = @userId

	IF(dbo.TRIM(ISNULL(@userSSN, '')) = '')
	BEGIN
		SET @userSSN = NULL
	END

	CREATE TABLE #TempSearchData
	(
		RefId INT,
		SSN NVARCHAR(9),
		Name NVARCHAR(100),
		UnitName NVARCHAR(100),
		CaseId VARCHAR(50),
		WorkStatus VARCHAR(50),
		ModuleId INT,
		ModuleName NVARCHAR(50),
		ReceiveDate CHAR(11),
		Days INT,
		Sub_Type INT,
		Compo CHAR(1),
		IsFinal BIT,
		caseUnitId INT,
		memberCurrentUnitId INT,
		workflow_title NVARCHAR(50)
	)

	INSERT	INTO	#TempSearchData
			SELECT	DISTINCT sc_id,
					fsc.member_ssn,
					fsc.member_name,
					fsc.member_unit,
					fsc.Case_Id,
					vws.description,
					fsc.Module_Id,
					mods.moduleName,
					CONVERT(char(11), ISNULL(t.ReceiveDate, fsc.created_date), 101) AS ReceiveDate,
					DATEDIFF(d, ISNULL(t.ReceiveDate, fsc.created_date), GETDATE()) AS Days,
					sub_workflow_type,
					fsc.Member_Compo,
					vws.isFinal,
					fsc.Member_Unit_Id,
					mCS.CS_ID,
					(CASE fsc.sub_workflow_type WHEN 0 Then w.title ELSE sub.subTypeTitle END) AS workflow_title
			FROM	Form348_SC fsc
					JOIN vw_WorkStatus vws ON fsc.status = vws.ws_id
					JOIN core_lkupModule mods ON mods.moduleId = fsc.Module_Id
					JOIN core_workflow w ON w.workflowId = fsc.workflow
					LEFT JOIN MemberData md ON fsc.Member_ssn = md.SSAN
					LEFT JOIN Command_Struct mCS ON md.PAS_NUMBER = mCS.PAS_CODE
					LEFT JOIN core_lkupSCSubType sub ON sub.subTypeId = fsc.sub_workflow_type
					LEFT JOIN (
						SELECT	MAX(startDate) ReceiveDate, ws_id, refId 
						FROM	core_WorkStatus_Tracking 
						GROUP	BY ws_id, refId
					) t ON t.refId = fsc.sc_id AND t.ws_id = fsc.status
			WHERE	fsc.Member_ssn = @member_ssn


	SELECT	r.RefId,
			SUBSTRING(r.SSN, 6, 4) AS Protected_SSN,
			r.Name AS Member_Name,
			r.UnitName AS Unit_Name,
			r.CaseId AS Case_Id,
			r.WorkStatus AS Status,
			r.ModuleId AS Module_Id,
			r.ModuleName AS Module,
			r.ReceiveDate,
			r.Days,
			r.Sub_Type,
			r.workflow_title
	FROM	#TempSearchData r
	WHERE	(
				-- global scope
				(@scope = 3)
				OR
				-- component scope
				(@scope = 2)
				OR
				-- unit scope
				(
					@scope = 1  
					AND
					(
						(CASE r.IsFinal WHEN 1 THEN r.memberCurrentUnitId ELSE r.caseUnitId END) IN
						(	
							SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@userUnit and view_type=@rptView 
						)
					)
					AND	dbo.fn_IsUserTheMember(@userId, r.SSN) = 0	-- Don't return cases which revolve around the user doing the search...
				)
			)
	ORDER	BY r.ModuleName, r.WorkStatus, r.ReceiveDate DESC
END
GO

