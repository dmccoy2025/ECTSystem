-- ============================================================================
-- Author:		Eric Kelley
-- Create date: 14 Dec 2021
-- Description:	Executes the LOD portion of the Case History Canned Report. 
-- ============================================================================
CREATE PROCEDURE [dbo].[Form348_sp_InitiateIncapFindings]
@sc_Id int
as


SET IDENTITY_INSERT [dbo].[Form348_INCAP_Findings] ON;

INSERT INTO [dbo].[Form348_INCAP_Findings]
           ([sc_Id]
           )
     VALUES
           (@sc_Id
           )

SET IDENTITY_INSERT [dbo].[Form348_INCAP_Findings] OFF

--Form348_sp_InitiateIncapFindings 2
GO

