CREATE PROCEDURE [usp].[GetNumbersInQueue_Daily]
AS
DECLARE 
----appedd:@startDate DATETIME = '2020-10-09 14:00:00.000', -- INPUT: PAST DATE TO BEGIN SEARCHING FROM (format: YYYY-MM-DD)----
----append:@endDate DATETIME = '2020-10-13 14:00:00.000' -- INPUT: DATE TO STOP THE SEARCH AT (format: YYYY-MM-DD)----
@date DATETIME = CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP)) + '9:00',
@endDate DATETIME = CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP)) + '9:00',
@startDate DATETIME;
----Set @startDate to day prior, But if @date is a Monday THEN SET to Friday's date----
SET @startDate =
	CASE DATEPART(weekday, @date)
		WHEN 2 THEN CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP-3)) + '9:00'
		ELSE CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP-1)) + '9:00'
		END
----To show/verify the @startDate and @endDate and adds to the report---- 
SELECT @startDate 'Start Date',
		@endDate 'End Date'
--1
select count (*) as "New Cases Queued per day w/ Initial Tech" from core_WorkStatus_Tracking
where ws_id in ('53', '59', '65', '41', '47', '45', '68', '71', '76', '84', '106', '112', '114', '125', '131', '132', '147', '242', '244', '246','247')
and StartDate BETWEEN @startDate AND @endDate AND (EndDate BETWEEN @startDate AND @endDate OR (EndDate IS NULL))

--2
select count (*) as "New Cases Queued per day w/ Med Review" from core_WorkStatus_Tracking
where ws_id in (select ws_id from core_WorkStatus
where statusId in (select statusId from core_StatusCodes
where groupId = 9)) and StartDate BETWEEN @startDate AND @endDate AND (EndDate BETWEEN @startDate AND @endDate OR (EndDate IS NULL))

--3
select count (*) as "New Cases Queued per day w/ Sr Med Review" from core_WorkStatus_Tracking
where ws_id in (select ws_id from core_WorkStatus
where statusId in (select statusId from core_StatusCodes
where groupId = 92)) and StartDate BETWEEN @startDate AND @endDate AND (EndDate BETWEEN @startDate AND @endDate OR (EndDate IS NULL))

--4
select count (*) as "New Cases Queued per day w/ Final Tech" from core_WorkStatus_Tracking
where ws_id in ('56', '62', '44','49', '83', '86', '108', '116', '121', '123', '127', '134', '152', '154', '162', '164', '166', '168', '170'
,'175', '249') and StartDate BETWEEN @startDate AND @endDate AND (EndDate BETWEEN @startDate AND @endDate OR (EndDate IS NULL))

--5
select count (*) as "Total Cases Queued per day w/ Initial Tech" from core_WorkStatus_Tracking
where ws_id in ('53', '59', '65', '41', '47', '45', '68', '71', '76', '84', '106', '112', '114', '125', '131', '132', '147', '242', '244', '246','247') and endDate is Null

--6
select count (*) as "Total Cases Queued per day w/ Med Review" from core_WorkStatus_Tracking
where ws_id in (select ws_id from core_WorkStatus
where statusId in (select statusId from core_StatusCodes
where groupId = 9)) and endDate is Null

--7
select count (*) as "Total Cases Queued per day w/ Sr Med Review" from core_WorkStatus_Tracking
where ws_id in (select ws_id from core_WorkStatus
where statusId in (select statusId from core_StatusCodes
where groupId = 92)) and endDate is Null

--8
select count (*) as "Total Cases Queued per day w/ Final Tech" from core_WorkStatus_Tracking
where ws_id in ('56', '62', '44','49', '83', '86', '108', '116', '121', '123', '127', '134', '152', '154', '162', '164', '166', '168', '170'
,'175', '249') and endDate is Null


--[usp].[GetNumbersInQueue_Daily]
GO

