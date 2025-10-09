CREATE PROCEDURE [dbo].[core_pageTitles_sp_Insert]
(
	@title as varchar(50)
	
)

AS
	SET NOCOUNT ON;
Declare @pageId as  int

SELECT @pageId=pageId FROM dbo.core_Pages WHERE  (title = @title)

if @pageId is null
	BEGIN

	INSERT INTO dbo.core_Pages
						  (title)
	VALUES     (@title)

	END
GO

