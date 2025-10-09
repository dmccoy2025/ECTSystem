--exec core_pascode_sp_GetReporting 3
-- ============================================================================
-- Author:			Nandita Srivastava
-- Create date:		Feb 20th 2009
-- Description:		Procedure to select reporting information reagarding the 
--					pascode.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	5/4/2016
-- Description:		Modified the select statement to also select the PAS code
--					of the units.
--					Cleaned up the stored procedure.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_pascode_sp_GetReporting]
	@CS_ID int 
AS

BEGIN 	 
	--Fetch the parents info
	SELECT	@CS_ID AS cs_id,
			chain.name AS Chain_Type,
			parent_cs_Id =	CASE cs_rpt.cs_id 
								WHEN NULL THEN NULL
								ELSE  cs_rpt.cs_id 
							END,
			parent_long_name =	CASE cs.long_name
									WHEN NULL THEN NULL
									ELSE  cs.long_name 
								END ,
			chain.description AS Chain_Description,
			parent_pas = CASE cs.PAS_CODE
							WHEN NULL THEN NULL
							ELSE cs.PAS_CODE
						 END
	FROM	core_lkupChainType AS chain
			LEFT JOIN COMMAND_STRUCT_CHAIN csc ON csc.Chain_Type = chain.name AND csc.cs_id = @CS_ID   
			LEFT JOIN COMMAND_STRUCT_CHAIN cs_rpt ON cs_rpt.csc_id = csc.csc_id_parent
			LEFT JOIN COMMAND_STRUCT cs ON cs_rpt.cs_id = cs.cs_id	  
	ORDER	BY chain.name

END
GO

