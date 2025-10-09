
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/26/2016
-- Description:	This stored procedure acts as the datasource for Certification
--				Stamps. It retrieves all of the necessary data to populate the
--				paramatized fields in the certification stamp bodies such as
--				{FREE_TEXT}. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	5/19/2016	
-- Description:		Modified to be able to select the secondary free text field
--					for the case.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CertificationStamp_sp_GetParameterFieldData]
	@refId INT,
	@selectSecondary BIT
AS

DECLARE @results TABLE
(
	id VARCHAR(100),
	value VARCHAR(MAX)
)

DECLARE @freeText NVARCHAR(500),
		@certificationDate DATETIME


-- GET PARAMETER DATA --
SET @certificationDate = GETDATE()

IF (ISNULL(@selectSecondary, 0) = 1)
BEGIN
	SELECT	@freeText = s.secondary_free_text
	FROM	Form348_SC s
	WHERE	s.SC_Id = @refId
END
ELSE
BEGIN
	SELECT	@freeText = s.free_text
	FROM	Form348_SC s
	WHERE	s.SC_Id = @refId
END


-- ASSIGN DATA TO PARAMETERS --
INSERT INTO @results (id, value) VALUES ('CERTIFICATION_DATE', STUFF(CONVERT(VARCHAR(17), @certificationDate, 106), 4, 3, DATENAME(month, @certificationDate)))
INSERT INTO @results (id, value) VALUES ('FREE_TEXT', @freeText)


-- SELECT DATA --
SELECT	r.id, r.value
FROM	@results r
GO

