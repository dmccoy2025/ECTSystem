-- ============================================================================
-- Author:		Eric Kelley
-- Create date: 20 Dec 2021
-- Description:	Executes the LOD portion of the Case History Canned Report. 
-- ============================================================================
CREATE PROCEDURE [dbo].[Form348_sp_InitiateIncapAppeal]
@sc_Id int
as

Declare
@num int = 1,
@extNum int = 0,
@ext_Id int = 0,
@app_Id int,
@test varChar(50) = 0

Set @extNum = (Select MAX(ext_Number) from Form348_INCAP_Ext where @sc_Id = sc_Id)


IF Exists(Select sc_Id from Form348_INCAP_Ext where @sc_Id = sc_Id and ext_Number = @extNum and cafr_ExtApproval = 0) --check case is an Extention Appeal
	Set @ext_Id = (Select ext_Id from Form348_INCAP_Ext where sc_Id = @sc_Id and @extNum = ext_Number)
	IF NOT Exists(Select sc_Id from Form348_INCAP_Appeal where sc_Id = @sc_Id and @ext_Id = ext_Id and cafr_AppealApproval is null)
	Begin
		INSERT INTO [dbo].[Form348_INCAP_Appeal]
           (sc_Id, ext_Id)
     VALUES
           (@sc_Id,
		   @ext_Id)
	End
Else If Exists(Select sc_Id from Form348_INCAP_Findings where @sc_Id = sc_Id and wcc_InitApproval = 0)--check case is an Initial Appeal
	IF NOT Exists(Select sc_Id from Form348_INCAP_Appeal where sc_Id = @sc_Id and @ext_Id = ext_Id and cafr_AppealApproval is Null)
	Begin
		INSERT INTO [dbo].[Form348_INCAP_Appeal]
           (sc_Id)
     VALUES
           (@sc_Id)
	End

	
	Select @test
--[Form348_sp_InitiateIncapAppeal]
--[Form348_sp_InitiateIncapAppeal]
GO

