-- ============================================================================
-- Author:		Eric Kelley
-- Create date: 16 Dec 2021
-- Description:	Executes the LOD portion of the Case History Canned Report. 
-- ============================================================================
CREATE PROCEDURE [dbo].[Form348_sp_InitiateIncapExt]
@sc_Id int
as

Declare
@num int = 1,
@extNum int

Set @extNum = (Select MAX(ext_Number) from Form348_INCAP_Ext where @sc_Id = sc_Id)

If Exists(Select sc_Id, ext_Number from Form348_INCAP_Ext where @sc_Id = sc_Id and ext_Number = @extNum and cafr_ExtApproval is Null group by sc_Id, ext_Number) --check if not complete
Begin

	Select Max(ext_Number) from Form348_INCAP_Ext where @sc_Id = sc_Id and cafr_ExtApproval is null 
END
Else 
Begin
	If Exists(Select sc_Id, ext_Number from Form348_INCAP_Ext where @sc_Id = sc_Id and cafr_ExtApproval is not Null) --check if complete
	Begin
		Set @num = (Select max(ext_Number) from Form348_INCAP_Ext where @sc_Id = sc_Id and cafr_ExtApproval is not Null) + 1
	End
	Select @num as num
INSERT INTO [dbo].[Form348_INCAP_Ext]
           ([sc_Id],
		   ext_Number)
     VALUES
           (@sc_Id,
		   @num)

End

	

--[Form348_sp_InitiateIncapExt] 2
GO

