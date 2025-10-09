-- =============================================
-- Author:		Kenneth Barnett
-- Create date: 4/11/2014
-- Description:	Insert a new record into the core_Pages table. 
-- =============================================
CREATE PROCEDURE [dbo].[utility_workflow_sp_InsertPage]
	 @pageId smallint
	,@title varchar(50)
AS

IF NOT EXISTS(SELECT * FROM dbo.core_Pages WHERE pageId = @pageId AND title = @title)
	BEGIN
		SET IDENTITY_INSERT dbo.core_Pages ON
		INSERT INTO [dbo].[core_Pages] ([pageId], [title]) 
			VALUES (@pageId, @title)
		SET IDENTITY_INSERT dbo.core_Pages OFF
		
		PRINT 'Inserted new values (' + CONVERT(VARCHAR(3),@pageId) + ', ' + 
										CONVERT(VARCHAR(50), @title) + ') into core_Pages table.'
										
		-- VERIFY NEW PAGE
		SELECT P.*
		FROM core_Pages P
		WHERE P.title = @title
	END
ELSE
	BEGIN
		PRINT 'Values already exist in core_Pages table.'
	END
GO

