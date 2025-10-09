
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	9/9/2016
-- Description:		Altered to take in the ID of the workflow instead of the
--					workflow title. This is to allow the proper selection of
--					the workstatus for modules which have multiple workflows
--					that share the same Status Codes.
-- ============================================================================
CREATE PROCEDURE [dbo].[utility_workflow_sp_DeleteOptions]
	 @workflowId INT
AS
BEGIN
	declare @Proc nvarchar(50)
	declare @RowCnt int
	declare @MaxRows int
	declare @ExecSql nvarchar(255)

	select @RowCnt = 1
	select @Proc = 'core_workstatus_sp_DeleteOption'	

	-- Create a table with all of the wso_id's associated with the workflow
	declare @Import table (rownum int IDENTITY (1, 1) Primary key NOT NULL , wso_id int)
	insert into @Import (wso_id)	select wso_id
									from core_WorkStatus_Options
									where ws_id IN ( select	ws.ws_id
													 from	dbo.core_WorkStatus AS ws inner join
															dbo.core_StatusCodes AS sc ON sc.statusId = ws.statusId inner join
															core_Workflow wf on ws.workflowId = wf.workflowId
													 where	wf.workflowId = @workflowId )

	-- Get the number of rows in the table
	select @MaxRows=count(*) from @Import

	-- Loop through each record executing the delete option stored procedure
	while @RowCnt <= @MaxRows
	begin
		select @ExecSql = 'exec ' + @Proc + ' ' + convert(varchar(10), wso_id) from @Import where rownum = @RowCnt 
		--print @ExecSql
		execute sp_executesql @ExecSql
		Select @RowCnt = @RowCnt + 1
	end
END
GO

