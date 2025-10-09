CREATE PROCEDURE [usp].[GetCustomerDemand_ECT]
AS
-- ====================================================================================
-- Author:		Eric Kelley
-- Create date: 2/16/2021
-- Description: Produce raw numbers of all ECT cases that came in since last work day
-- ====================================================================================
DECLARE 
@startDate DATETIME ,				-- INPUT: PAST DATE TO BEGIN SEARCHING FROM (format: YYYY-MM-DD)
-------- Removed: 
-------- @endDate DATETIME = '2020-10-19 13:00:00.000'					-- INPUT: DATE TO STOP THE SEARCH AT (format: YYYY-MM-DD)


------time set to 13:00 like like appended code------
@endDate DATETIME = CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP)) + '09:00',
@today VARCHAR(20) = CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP)) + '09:00',
@thursday VARCHAR(10) = CONVERT(VARCHAR(10), DATEPART(weekday, getdate()), 111),
@test1 DATETIME,
@endDateName VARCHAR(15),
@reportStartDate VARCHAR(20) = CONVERT(VARCHAR(10), getdate(), 111),
@statusAsOf VARCHAR(20),
@timesMoves smallInt = 0,
@testDate DATETIME = CONVERT(DATETIME, CONVERT(varchar(10), '2020-11-02')) + '09:00'

SET @startDate =
CASE DATEPART(weekday, CURRENT_TIMESTAMP)
		WHEN 2 THEN CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP - 3)) + '09:00' 
		--WHEN 3 THEN CONVERT(VARCHAR(15), DATENAME(WEEKDAY, getdate()-5))
		--WHEN 4 THEN CONVERT(VARCHAR(15), DATENAME(WEEKDAY, getdate()-6))
		--WHEN 5 THEN CONVERT(VARCHAR(15), DATENAME(WEEKDAY, getdate()-7))
		--WHEN 6 THEN CONVERT(VARCHAR(15), DATENAME(WEEKDAY, getdate()-1))
		ELSE CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP - 1)) + '09:00'
		END
-- to go to last Thusday --
--SET @reportStartDate = 
--CASE DATEPART(weekday, @reportStartDate)
--		WHEN 2 THEN CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP-4)) + '09:00'
--		WHEN 3 THEN CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP-5)) + '09:00'
--		WHEN 4 THEN CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP-6)) + '09:00'
--		WHEN 5 THEN CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP-7)) + '09:00'
--		WHEN 6 THEN CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP-1)) + '09:00'
--		ELSE CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP))
--		END

-- to go to yesterday unless it is Monday then go to Friday
SET @reportStartDate = 
CASE DATEPART(weekday, @reportStartDate)
		WHEN 2 THEN CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP-3)) + '09:00'
--If holiday weekend (3 is Tuesday)--
--WHEN 3 THEN CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP-4)) + '09:00'
		ELSE CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP-1)) + '09:00'
		END

Set @statusAsOf = 'Status as of:' + @today

SELECT @startDate 'Report Start Date',
		@today 'Report End Date',
		@testDate


		
		

	

----------------------------- to get data--------------------------------------------------------------------
--SELECT wst.workflowId, 
--CASE WHEN Form348.member_name IS NOT NULL THEN Form348.member_name ELSE
--	CASE WHEN Form348_SARC.member_name IS NOT NULL THEN Form348_SARC.member_name ELSE
--		CASE WHEN Form348_RR.member_name IS NOT NULL THEN Form348_RR.member_name ELSE
--			CASE WHEN Form348_SC.member_name IS NOT NULL THEN Form348_SC.member_name ELSE
--				CASE WHEN Form348_AP.member_name IS NOT NULL THEN Form348_AP.member_name ELSE
--					CASE WHEN Form348_AP_SARC.member_name IS NOT NULL THEN Form348_AP_SARC.member_name END
--				END
--			END
--		END
--	END
--END AS member_name,

--CASE WHEN Form348.case_id IS NOT NULL THEN Form348.case_id ELSE
--	CASE WHEN Form348_SARC.case_id IS NOT NULL THEN Form348_SARC.case_id ELSE
--		CASE WHEN Form348_RR.Case_Id IS NOT NULL THEN Form348_RR.Case_Id ELSE
--			CASE WHEN Form348_SC.Case_Id IS NOT NULL THEN Form348_SC.Case_Id ELSE
--				CASE WHEN Form348_AP.Case_Id IS NOT NULL THEN Form348_AP.Case_Id ELSE
--					CASE WHEN Form348_AP_SARC.Case_Id IS NOT NULL THEN Form348_AP_SARC.Case_Id END
--				END
--			END
--		END
--	END
--END AS 'Case_Id',

--CASE WHEN Form348.member_unit IS NOT NULL THEN Form348.member_unit ELSE
--	CASE WHEN Form348_SARC.member_unit IS NOT NULL THEN Form348_SARC.member_unit ELSE
--		CASE WHEN Form348_RR.member_unit IS NOT NULL THEN Form348_RR.member_unit ELSE
--			CASE WHEN Form348_SC.member_unit IS NOT NULL THEN Form348_SC.member_unit ELSE
--				CASE WHEN Form348_AP.member_unit IS NOT NULL THEN Form348_AP.member_unit ELSE
--					CASE WHEN Form348_AP_SARC.member_unit IS NOT NULL THEN Form348_AP_SARC.member_unit END
--				END
--			END
--		END
--	END
--END AS MemberUnit,
		
--sc.description, 
--wst.name, 
--wst.startDate, 
--wst.endDate, 
--scl.Position, 
--scl.Location,
--CASE 
--	When pw.Description <> sc.description THEN @timesMoves + 1
--	WHEN @timesMoves = 0 OR @timesMoves IS NULL THEN 0
--END 'Times_Moved',
--CASE 
--	WHEN wst.startDate < GETDATE() AND wst.endDate <= GETDATE() OR wst.endDate IS NULL THEN 'Open'
--END 'StatusAsOf'
-----------------------------------------------------------------------------------------------------------
----------------- To get raw total-------------------------------------------------------------------------
SELECT COUNT(*)
FROM	
core_WorkStatus_Tracking wst
JOIN core_WorkStatus ws ON wst.ws_id = ws.ws_id
JOIN core_StatusCodes sc ON ws.statusId = sc.statusId
LEFT JOIN core_Users u ON wst.completedBy = u.userID
LEFT JOIN dbo.[Special_Cases_&_LOD_Cases] scl ON sc.description = scl.Status
LEFT JOIN Form348 ON Form348.lodId = wst.refid AND wst.workflowId IN (1, 27)
LEFT JOIN Form348_SARC ON Form348_SARC.sarc_id = wst.refId AND wst.workflowId IN (28)
LEFT JOIN Form348_RR ON Form348_RR.request_id = wst.refid AND wst.workflowId IN (5)
LEFT JOIN Form348_SC ON Form348_SC.sc_id = wst.refid AND wst.workflowId IN (23,15,12,6,21,7,24,18,30,19,25,13,22,16,11,20,14,8,31)
LEFT JOIN Form348_AP ON Form348_AP.appeal_id = wst.refid AND wst.workflowId IN (26)
LEFT JOIN Form348_AP_SARC ON Form348_AP_SARC.appeal_sarc_id = wst.refid AND wst.workflowId IN (29)
LEFT JOIN Memberdata md ON md.SSAN = Form348.member_ssn 
LEFT JOIN Memberdata md1 ON md1.SSAN = Form348_SARC.member_ssn
LEFT JOIN Memberdata md2 ON md2.SSAN = Form348_RR.member_ssn
LEFT JOIN Memberdata md3 ON md3.SSAN = Form348_SC.Member_ssn
LEFT JOIN Memberdata md4 ON md4.SSAN = Form348_AP.member_ssn
LEFT JOIN Memberdata md5 ON md5.SSAN = Form348_AP_SARC.member_ssn
LEFT JOIN PreviousWeeksECTCases pw ON pw.CaseId = 'Case_Id'
WHERE wst.StartDate BETWEEN @startDate AND @endDate 
--only receiving the day before--
AND wst.startDate > @testDate -- Change from @testDate to @reportStartDate
AND (wst.EndDate BETWEEN @startDate AND @endDate OR (wst.EndDate IS NULL)) AND wst.endDate IS NULL AND scl.Location = 'SG' AND(Form348.member_compo = 6 OR Form348_SARC.member_compo = 6
OR Form348_RR.member_compo = 6 OR Form348_SC.Member_Compo = 6 OR Form348_AP.member_compo = 6 OR Form348_AP_SARC.member_compo = 6)
--ORDER BY wst.startDate DESC


--[usp].[GetCustomerDemand_ECT]
--ORDER BY wst.startDate DESC


--[usp].[GetCustomerDemand_ECT]
GO

