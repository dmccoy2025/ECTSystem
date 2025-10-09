-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 8/27/2015
-- Description:	Attempts to select the ICD ID values associated with a specific
--				set of ICD diagnosises. 
--				Current set of type names that will return values:
--					* Diabetes
--					* Asthma
--					* Sleep
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookups_sp_GetIRILOTypeICDCodeIDs]
	 @diagnosisType VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @valid INT = 1
	
	IF @diagnosisType IS NULL OR @diagnosisType = ''
		BEGIN
		SET @valid = 0
		END
	
	IF @valid = 1 
		BEGIN
		
		SELECT	icd.ICD9_ID As ICD_ID
		FROM	core_KeyVal_Value v
				INNER JOIN core_KeyVal_Key k ON v.Key_Id = k.ID
				INNER JOIN core_KeyVal_KeyType kt ON k.Key_Type_Id = kt.Id
				INNER JOIN core_lkupICD9 icd ON (icd.ICDVersion = 9 AND CAST(icd.ICD9_ID AS VARCHAR(10)) = v.Value)
		WHERE	kt.Type_Name = 'IRILOKey'
				AND k.Description = 'IRILO ICD9 IDs'
				AND v.Value_Description = @diagnosisType
		UNION
		SELECT	icd.ICD9_ID AS ICD_ID
		FROM	core_KeyVal_Value v
				INNER JOIN core_KeyVal_Key k ON v.Key_Id = k.ID
				INNER JOIN core_KeyVal_KeyType kt ON k.Key_Type_Id = kt.Id
				INNER JOIN core_lkupICD9 icd ON (icd.ICDVersion = 10 AND icd.value LIKE v.Value + '%')
		WHERE	kt.Type_Name = 'IRILOKey'
				AND k.Description = 'IRILO ICD10 Base Codes'
				AND v.Value_Description = @diagnosisType
				
		END
END
GO

