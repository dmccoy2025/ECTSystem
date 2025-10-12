-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 4/19/2016
-- Description:	Returns all of the Application Warmup Process Log records with pagination, filtering, and sorting.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	4/19/2016
-- Description:		Added selection of the Message field.
-- ============================================================================
-- Modified By:		Donell McCoy
-- Modified Date:	10/11/2025
-- Description:		Added filtering and dynamic sorting capabilities.
-- ============================================================================
CREATE PROCEDURE [dbo].[ApplicationWarmupProcess_sp_GetAllLogs_pagination]
	@PageNumber INT = 1,
	@PageSize INT = 10,
	@ProcessName NVARCHAR(255) = NULL,
	@StartDate DATETIME = NULL,
	@EndDate DATETIME = NULL,
	@MessageFilter NVARCHAR(500) = NULL,
	@SortBy NVARCHAR(50) = 'ExecutionDate',
	@SortOrder NVARCHAR(4) = 'DESC'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Validate sort parameters
	IF @SortBy NOT IN ('Id', 'Name', 'ExecutionDate', 'Message')
	BEGIN
		RAISERROR('Invalid @SortBy parameter. Valid values are: Id, Name, ExecutionDate, Message', 16, 1);
		RETURN;
	END
	
	IF @SortOrder NOT IN ('ASC', 'DESC')
	BEGIN
		RAISERROR('Invalid @SortOrder parameter. Valid values are: ASC, DESC', 16, 1);
		RETURN;
	END
	
	-- Return total count first
	SELECT COUNT(*) AS TotalCount
	FROM	ApplicationWarmupProcessLog l
			JOIN ApplicationWarmupProcess p ON l.ProcessId = p.Id
	WHERE	(@ProcessName IS NULL OR p.Name LIKE '%' + @ProcessName + '%')
			AND (@StartDate IS NULL OR l.ExecutionDate >= @StartDate)
			AND (@EndDate IS NULL OR l.ExecutionDate <= @EndDate)
			AND (@MessageFilter IS NULL OR l.Message LIKE '%' + @MessageFilter + '%');
	
	-- Return paginated data
	SELECT	l.Id, p.Name, l.ExecutionDate, l.Message
	FROM	ApplicationWarmupProcessLog l
			JOIN ApplicationWarmupProcess p ON l.ProcessId = p.Id
	WHERE	(@ProcessName IS NULL OR p.Name LIKE '%' + @ProcessName + '%')
			AND (@StartDate IS NULL OR l.ExecutionDate >= @StartDate)
			AND (@EndDate IS NULL OR l.ExecutionDate <= @EndDate)
			AND (@MessageFilter IS NULL OR l.Message LIKE '%' + @MessageFilter + '%')
	ORDER	BY 
		CASE WHEN @SortBy = 'Id' AND @SortOrder = 'ASC' THEN l.Id END ASC,
		CASE WHEN @SortBy = 'Id' AND @SortOrder = 'DESC' THEN l.Id END DESC,
		CASE WHEN @SortBy = 'Name' AND @SortOrder = 'ASC' THEN p.Name END ASC,
		CASE WHEN @SortBy = 'Name' AND @SortOrder = 'DESC' THEN p.Name END DESC,
		CASE WHEN @SortBy = 'ExecutionDate' AND @SortOrder = 'ASC' THEN l.ExecutionDate END ASC,
		CASE WHEN @SortBy = 'ExecutionDate' AND @SortOrder = 'DESC' THEN l.ExecutionDate END DESC,
		CASE WHEN @SortBy = 'Message' AND @SortOrder = 'ASC' THEN l.Message END ASC,
		CASE WHEN @SortBy = 'Message' AND @SortOrder = 'DESC' THEN l.Message END DESC
	OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
END
GO