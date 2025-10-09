

 --exec lod_sp_AccessHeader    2,'PHA_REPORT',4
 
CREATE PROCEDURE [dbo].[lod_sp_AccessHeader]
	 @lodid int 
	,@chain_type as varchar(20)
	,@userId int

AS

SET NOCOUNT ON

 
		SELECT 
			a.lodid, a.status
			,b.description, b.groupId, c.name groupName
			,a.MEMBER_SSN, a.MEMBER_NAME, a.MEMBER_COMPO, a.workflow, e.title, e.formal  
			, CASE	
					WHEN u.pas_code IS NOT NULL AND u.pas_code IN  (select * from core_fn_userPascodes(@chain_type,@userId)  )
						THEN CAST(1 AS BIT)
					ELSE 
						CAST(0 AS BIT)
					END ,u.pas_code
		FROM 
			Form348 a
		JOIN 
			core_StatusCodes b ON b.statusId = a.status
		LEFT JOIN 
			core_UserGroups c ON c.groupId = b.groupId
		JOIN
			core_Workflow e ON e.workflowId = a.workflow
		LEFT JOIN 
			COMMAND_STRUCT u on u.cs_id=a.member_unit_id 
		WHERE 
			a.lodid = @lodId
GO

