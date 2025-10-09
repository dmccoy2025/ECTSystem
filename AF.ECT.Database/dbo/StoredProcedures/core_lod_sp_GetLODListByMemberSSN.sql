--exec [core_lod_sp_GetLODListByMemberSSN] '010000004',1,430

-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	8/2/2016
-- Description:		Updated to no longer use a hard coded value for the LOD
--					statuses.
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	11/23/2016
-- Description:		Updated search type to only include in progress or comleted
--					LOD cases
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	4/14/2017
-- Description:		return refId, workflow, and caseId of an LOD case 
--					for a member
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lod_sp_GetLODListByMemberSSN]
	@memberSSN varchar(9)=null,
	@searchType int,
	@userId  int
AS
BEGIN
	Declare @scope tinyint, @userUnit int
	SELECT @scope = accessscope, @userUnit = unit_id from vw_Users where userId=@userId


	SELECT	a.lodId as refId,
			a.case_id  as caseId,
			a.workflow as workflowId  
	FROM	Form348  a
			JOIN vw_WorkStatus vws ON a.status = vws.ws_id
	WHERE	(   
				a.member_ssn LIKE '%' + IsNull(@memberSSN, a.member_ssn)
 				AND  
				(
					(@scope = 3)
					OR
					(@scope = 2)
					OR
					(	
						@scope = 1  
						AND
						a.member_unit_id  IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@userUnit) 
					)		 
				)
				AND
				(
					@searchType = 1 
					OR
					(
						@searchType = 2
						AND
						FinalFindings IN (1, 3)
						AND
						(vws.isFinal = 1 AND vws.isCancel = 0) -- Complete
					)
					OR
					(
						@searchType = 3
						AND
						FinalFindings Not IN (1, 3)
						AND
						(vws.isFinal = 1 AND vws.isCancel = 0) -- Complete
					)
					OR
					(
						@searchType = 4
						AND
						vws.isCancel = 0
					)
				)
			)
	ORDER	BY a.case_id
END
GO

