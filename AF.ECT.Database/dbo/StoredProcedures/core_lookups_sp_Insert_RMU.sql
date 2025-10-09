-- ============================================================================
-- Author:		Kenneth Barnett
-- Create date: 6/18/2014
-- Description:	Inserts a new record into or updates an existing record in core_lkupRMUs
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified date:	4/7/2016
-- Description:		Modified to insert/update the CS ID of the RMU based on the
--					PAS Code. Also, the collocated bit is inserted/updated.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookups_sp_Insert_RMU] 
	@id INT = NULL,
	@rmuName NVARCHAR(50) = '',
	@rmuPAS NVARCHAR(4) = '',
	@collocated INT
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @rmuCSId INT = NULL
	
	IF (ISNULL(@rmuPAS, '') <> '')
	BEGIN
		SELECT	@rmuCSId = cs.CS_ID
		FROM	Command_Struct cs
		WHERE	cs.PAS_CODE = @rmuPAS
	END
	
	IF (@id IS NOT NULL) AND (@id <> 0)
	BEGIN
		UPDATE	core_lkupRMUs
		SET		RMU = @rmuName,
				cs_id = @rmuCSId,
				collocated = CONVERT(BIT, @collocated)
		WHERE	Id = @id
	END
	 	
	ELSE
	BEGIN
		DECLARE @name NVARCHAR(50)
			
		SELECT	@name = RMU 
		FROM	core_lkupRMUs
		WHERE	RMU = @rmuName	
		
		IF @name is null
		BEGIN
			INSERT	INTO	core_lkupRMUs([RMU], [cs_id], [collocated])
					VALUES	(@rmuName, @rmuCSId, CONVERT(BIT, @collocated))
		END
	END

END
GO

