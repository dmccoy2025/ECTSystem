-- =============================================
-- Author:		Nandita Srivastav 
 -- Description:	Get Reinvestigaion CaseId  
-- =============================================
CREATE FUNCTION [dbo].[GetReinvestigationCaseId] 
(
	-- Add the parameters for the function here
	@lodid  int
)
RETURNS varchar(20) 
AS

BEGIN

	Declare @initCaseId varchar(20) = '', @unlinkedCaseId varchar(20) = ''
	
	-- See if the initial LOD is not in the database (it SHOULD be)
	If Not Exists (Select 1 From Form348 Where LodId = @LodId)
	BEGIN
		Return 'Invalid'  -- invalid LodId
	END

	-- See if a reinvestigation has already been mapped
	If Exists (Select 1 From Form348_RR Where ReinvestigationLodId = @LodId)
	BEGIN
		Return 'Reinvestigation'  -- ID already under Reinvestigation (this IS the reinvestigation)
	END

	-- See if a reinvestigation has already been mapped
	If Exists (Select 1 From Form348_RR Where InitialLodId = @lodId)
	BEGIN
		Return 'Linked'  -- ID already under Reinvestigation (in the reinvestigation table)
	END

	-- See if unlinked reinvestigations exist (unlikely if PopulateReinvestigationsTable script has run)
	Select @unlinkedCaseId = Case_Id from Form348 Where LodId = @LodId

	If ((@unlinkedCaseId <> '') And (Len(@unlinkedCaseId) = 12))
	BEGIN
		Select Top 1 @initCaseId = Left(Case_Id, 12) From Form348 
		Where Len(Case_Id) > 12 
			And Case_Id Like @unlinkedCaseId
--			And SubString(Case_Id, 13, LEN(Case_Id) - 12) = '-RR'
	END		
			
	If @initCaseId <> ''
	BEGIN
		Return 'UnLinked'  -- ID already under Reinvestigation (not in the reinvestigation table)
	END

	-- Assign the Reinvestigation Case Id
	Select @initCaseId = Case_Id + '-RR' From Form348 Where lodid = @lodid 

--	Select @initCaseId = @initCaseId + '-RR'
	
	RETURN  @initCaseId

END
GO

