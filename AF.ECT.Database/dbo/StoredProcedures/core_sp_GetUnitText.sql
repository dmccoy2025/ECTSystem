CREATE PROCEDURE [dbo].[core_sp_GetUnitText]
	@cs_id int

AS

	SET NOCOUNT ON;

--declare @cs_id int
--set @cs_id = 3

select long_name + '(' + pas_code +')' as text from COMMAND_STRUCT WHERE cs_id = @cs_id
GO

