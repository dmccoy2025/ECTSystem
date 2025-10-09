
--***********************************************
-- Author:			Evan Morrison
-- Created date:	12/23/2016
-- Description:		Given the workflow and the refid get the most recent RWOA
--***********************************************

CREATE PROCEDURE [dbo].[core_rwoa_GetRecent]
	 @workflow int,
	 @refId int
	

AS

SET NOCOUNT ON

SELECT TOP 1 * From Rwoa WHERE workflow = @workflow AND refId = @refId ORDER BY date_sent DESC
GO

