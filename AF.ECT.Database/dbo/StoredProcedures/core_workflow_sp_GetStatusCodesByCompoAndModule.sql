
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	8/7/2015	
-- Description:		Now also selects the isDisposition core_StatusCodes field.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/15/2016
-- Description:		Now also selects the isFormal core_StatusCodes field.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_workflow_sp_GetStatusCodesByCompoAndModule]
	@compo CHAR(1),
	@module TINYINT
AS
BEGIN
	SET NOCOUNT ON

	SELECT	a.statusId, a.description, a.moduleId, a.compo, 
			ISNULL(a.groupId,0) groupId, a.isFinal, a.isApproved, a.canAppeal,
			ISNULL(b.name,'None') groupName, c.moduleName, d.compo_descr,
			CASE 
				WHEN a.groupId IS NULL THEN a.description
				ELSE a.description + ' (' + b.name + ')' 
			END fullStatus, 
			displayOrder, isDisposition, isFormal
	FROM	core_StatusCodes a 
			LEFT JOIN core_UserGroups b ON a.groupId = b.groupId
			JOIN core_lkupModule c ON c.moduleId = a.moduleId
			JOIN core_lkupCompo d ON d.compo = a.compo
	WHERE	(a.compo = @compo OR b.accessScope = 4)
			AND a.moduleId = @module
	ORDER	BY a.displayOrder
END
GO

