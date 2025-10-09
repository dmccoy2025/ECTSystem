
-- ============================================================================
-- Author:			?
-- Create date:		?
-- Description:		Returns a string detailing where the specified case has
--					come from in terms of its status and direction of routing.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	2/22/2017
-- Description:		Refactored the stored procedure to no longer use a cursor 
--					to iterate over the last three statuses the case was in.
--					Using a cursor was causing a major slowdown to occur for
--					queries that were returnign 28K+ records and having to 
--					call this function for each of those records.
-- ============================================================================
CREATE FUNCTION [dbo].[GetFromAndDirection]
(
	@lodid INT
)
RETURNS varchar(100) 
AS
BEGIN
	DECLARE @CurrentStatus			Int = 0
	DECLARE @PreviousStatus			Int = 0
	DECLARE @PreviousPreviousStatus	Int = 0
	DECLARE @UserName				Varchar(50)
	DECLARE @Role					Varchar(50)
	DECLARE @Direction				Varchar(15)
	DECLARE @Counter				Int = 0
	DECLARE @Combo					Varchar(150)

	DECLARE @TrackingRecords TABLE
	(
		Indx INT IDENTITY(1,1),
		lodId INT,
		UserName VARCHAR(200),
		StatusCode INT,
		UserGroup VARCHAR(100)
	)

	INSERT	INTO	@TrackingRecords([lodId], [UserName], [StatusCode], [UserGroup])
			SELECT	TOP 3 F.lodId, UPPER(CU.FirstName + ' ' + CU.LastName) as UserName, CSC.statusId as StatusCode, CUG.name
			FROM	core_WorkStatus_Tracking CWST
					JOIN Form348 F on CWST.refId = F.lodId
					JOIN core_Users CU on CWST.completedBy = CU.userID
					JOIN core_WorkStatus CWS on CWS.ws_id = CWST.ws_id
					JOIN core_StatusCodes CSC on CWS.statusId = CSC.statusId
					JOIN core_UserRoles CUR on CU.currentRole = CUR.userRoleID
					JOIN core_UserGroups CUG on CUR.groupId = CUG.groupId
			WHERE	lodId = @LodID
					AND cwst.module = 2
			ORDER	BY F.LODID, CWST.wst_id DESC

	SET @Counter = (SELECT COUNT(*) FROM @TrackingRecords)

	IF (@Counter = 0)
	BEGIN
		SET @Direction = 'Initiated By '
		SELECT	@UserName = UPPER(u.FirstName + ' ' + u.LastName), @Role = CUG.name
		FROM	Form348 f
				JOIN core_Users u ON f.created_by = u.userID
				JOIN core_UserRoles CUR on u.currentRole = CUR.userRoleID
				JOIN core_UserGroups CUG on CUR.groupId = CUG.groupId
		WHERE	f.lodId = @lodid
	END
	ELSE
	BEGIN
		SET @CurrentStatus = (SELECT tr.StatusCode FROM @TrackingRecords tr WHERE tr.Indx = 1)

		IF (@Counter > 1)
			SET @PreviousStatus = (SELECT tr.StatusCode FROM @TrackingRecords tr WHERE tr.Indx = 2)
	
		If (@Counter > 2)
			SET @PreviousPreviousStatus = (SELECT tr.StatusCode FROM @TrackingRecords tr WHERE tr.Indx = 3)

		SELECT	@UserName = tr.UserName,
				@Role = tr.UserGroup
		FROM	@TrackingRecords tr
		WHERE	tr.Indx < 3
		ORDER	BY tr.Indx

		IF (@CurrentStatus = 16)
		BEGIN
			Set @Direction = 'Completed By'
		END
		ELSE
		BEGIN
			IF (@PreviousStatus < @CurrentStatus)
				BEGIN
					IF (@PreviousPreviousStatus = @CurrentStatus)
						BEGIN
							SET @Direction = 'Reply From'
						END
					ELSE
						Begin
							SET @Direction = 'Forwarded By'
						END
				END
			ELSE
				BEGIN
					--wing sarc sent to board
					If((@PreviousStatus = 28) AND (@CurrentStatus = 11))
						BEGIN
							Set @Direction = 'Forwarded By'
						END
					ELSE
						BEGIN
							SET @Direction = 'Returned By'
						END
				END
		END
	END

	SET @Combo = @Direction + ' ' + @UserName + ', ' + @Role
		
	RETURN @Combo
END
GO

