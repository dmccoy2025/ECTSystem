
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 4/14/2017
-- Description:	Save Associated Cases for a case
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified date:	6/13/2017
-- Description:		Save the CaseId as well
-- ============================================================================
CREATE PROCEDURE [dbo].[core_AssocaitedCases_SaveAssociatedCases]
	@refId INT,
	@workflowId INT,
	@associated_refIds tblIntegerListUnordered READONLY,
	@associated_workflowIds tblIntegerListUnordered READONLY,
	@associated_CaseIds tblVarCharListUnordered READONLY
	
AS
BEGIN
	INSERT	INTO	core_AssociatedCases (refId, workflowId, associated_refId, associated_workflow, associated_CaseId)
			SELECT	@refId, @workflowId, ref_ROW.n, WORKS_ROW.n, CaseId_Row.n 
			FROM	(
						SELECT ref.n, ROW_NUMBER() OVER (Order by (select(null))) as r
						FROM @associated_refIds ref
					) AS ref_ROW,
					(
						SELECT work.n, ROW_NUMBER() OVER (Order by (select (null))) as r
						FROM @associated_workflowIds work
					) AS WORKS_ROW,
					(
						SELECT CaseId.n, ROW_NUMBER() OVER (Order by (select (null))) as r
						FROM @associated_CaseIds CaseId
					) AS CaseId_ROW
			WHERE	ref_ROW.r = WORKS_ROW.r
					AND caseId_Row.r = WORKS_ROW.r
END
GO

