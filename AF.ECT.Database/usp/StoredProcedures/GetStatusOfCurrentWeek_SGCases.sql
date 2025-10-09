-- ============================================================
-- Author:		Eric Kelley
-- Create date: 05/06/2021
-- Description:	Seperates the results into 2 queries to only 
-- give the data NEEDED for the Daily
-- ============================================================
CREATE PROCEDURE [usp].[GetStatusOfCurrentWeek_SGCases]


AS

DECLARE 
/******--TO BEGIN SEARCHING FROM (format: YYYY-MM-DD)
-------- append: @endDate DATETIME = '2020-10-19 13:00:00.000'					-- INPUT: DATE TO STOP THE SEARCH AT (format: YYYY-MM-DD)

------if time does not matter------------ 
--@endDate DATETIME = GETDATE()
******/

@startDate DATETIME = '2014-01-01 00:00:00.000',
@endDate DATETIME = CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP)) + '09:00',
@yesterdayAt9 DATETIME;

SET @yesterdayAt9 =
	CASE DATEPART(weekday, @endDate)
		WHEN 2 THEN CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP-3)) + '9:00'
		ELSE CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP-1)) + '9:00'
		END
-- ============================================================
-- Author:		Eric Kelley
-- Create date: 05/06/2021
-- Description:	Seperates the results into 2 queries to only 
-- give the data NEEDED for the Daily
-- ============================================================

/****** -------------------- to update the Status of current week on SG Case Managment -------------------- ******/

Select @yesterdayAt9 'StartDate', @endDate 'EndDate'

SELECT wst.workflowId, 
CASE WHEN Form348.member_name IS NOT NULL THEN Form348.member_name ELSE
	CASE WHEN Form348_SARC.member_name IS NOT NULL THEN Form348_SARC.member_name ELSE
		CASE WHEN Form348_RR.member_name IS NOT NULL THEN Form348_RR.member_name ELSE
			CASE WHEN Form348_SC.member_name IS NOT NULL THEN Form348_SC.member_name ELSE
				CASE WHEN Form348_AP.member_name IS NOT NULL THEN Form348_AP.member_name ELSE
					CASE WHEN Form348_AP_SARC.member_name IS NOT NULL THEN Form348_AP_SARC.member_name END
				END
			END
		END
	END
END AS member_name,

CASE WHEN Form348.case_id IS NOT NULL THEN Form348.case_id ELSE
	CASE WHEN Form348_SARC.case_id IS NOT NULL THEN Form348_SARC.case_id ELSE
		CASE WHEN Form348_RR.Case_Id IS NOT NULL THEN Form348_RR.Case_Id ELSE
			CASE WHEN Form348_SC.Case_Id IS NOT NULL THEN Form348_SC.Case_Id ELSE
				CASE WHEN Form348_AP.Case_Id IS NOT NULL THEN Form348_AP.Case_Id ELSE
					CASE WHEN Form348_AP_SARC.Case_Id IS NOT NULL THEN Form348_AP_SARC.Case_Id END
				END
			END
		END
	END
END AS CaseId,

CASE WHEN Form348.member_unit IS NOT NULL THEN Form348.member_unit ELSE
	CASE WHEN Form348_SARC.member_unit IS NOT NULL THEN Form348_SARC.member_unit ELSE
		CASE WHEN Form348_RR.member_unit IS NOT NULL THEN Form348_RR.member_unit ELSE
			CASE WHEN Form348_SC.member_unit IS NOT NULL THEN Form348_SC.member_unit ELSE
				CASE WHEN Form348_AP.member_unit IS NOT NULL THEN Form348_AP.member_unit ELSE
					CASE WHEN Form348_AP_SARC.member_unit IS NOT NULL THEN Form348_AP_SARC.member_unit END
				END
			END
		END
	END
END AS MemberUnit,
		
sc.description, wst.name, wst.startDate, wst.endDate, scl.Position, scl.Location
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
WHERE wst.StartDate BETWEEN @startDate AND @endDate AND endDate is null and Location = 'SG' and (wst.EndDate BETWEEN @startDate AND @endDate OR (wst.EndDate IS NULL)) AND (Form348.member_compo = 6 OR Form348_SARC.member_compo = 6
OR Form348_RR.member_compo = 6 OR Form348_SC.Member_Compo = 6 OR Form348_AP.member_compo = 6 OR Form348_AP_SARC.member_compo = 6)

ORDER BY LOCATION

/****** ---------------------------------to update the Doc Prod on SG Case Managment --------------------------------- ******/

SELECT wst.workflowId, 
CASE WHEN Form348.member_name IS NOT NULL THEN Form348.member_name ELSE
	CASE WHEN Form348_SARC.member_name IS NOT NULL THEN Form348_SARC.member_name ELSE
		CASE WHEN Form348_RR.member_name IS NOT NULL THEN Form348_RR.member_name ELSE
			CASE WHEN Form348_SC.member_name IS NOT NULL THEN Form348_SC.member_name ELSE
				CASE WHEN Form348_AP.member_name IS NOT NULL THEN Form348_AP.member_name ELSE
					CASE WHEN Form348_AP_SARC.member_name IS NOT NULL THEN Form348_AP_SARC.member_name END
				END
			END
		END
	END
END AS member_name,

CASE WHEN Form348.case_id IS NOT NULL THEN Form348.case_id ELSE
	CASE WHEN Form348_SARC.case_id IS NOT NULL THEN Form348_SARC.case_id ELSE
		CASE WHEN Form348_RR.Case_Id IS NOT NULL THEN Form348_RR.Case_Id ELSE
			CASE WHEN Form348_SC.Case_Id IS NOT NULL THEN Form348_SC.Case_Id ELSE
				CASE WHEN Form348_AP.Case_Id IS NOT NULL THEN Form348_AP.Case_Id ELSE
					CASE WHEN Form348_AP_SARC.Case_Id IS NOT NULL THEN Form348_AP_SARC.Case_Id END
				END
			END
		END
	END
END AS CaseId,

CASE WHEN Form348.member_unit IS NOT NULL THEN Form348.member_unit ELSE
	CASE WHEN Form348_SARC.member_unit IS NOT NULL THEN Form348_SARC.member_unit ELSE
		CASE WHEN Form348_RR.member_unit IS NOT NULL THEN Form348_RR.member_unit ELSE
			CASE WHEN Form348_SC.member_unit IS NOT NULL THEN Form348_SC.member_unit ELSE
				CASE WHEN Form348_AP.member_unit IS NOT NULL THEN Form348_AP.member_unit ELSE
					CASE WHEN Form348_AP_SARC.member_unit IS NOT NULL THEN Form348_AP_SARC.member_unit END
				END
			END
		END
	END
END AS MemberUnit,
		
sc.description, wst.name, wst.startDate, wst.endDate, scl.Position, scl.Location
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
WHERE wst.StartDate BETWEEN @startDate AND @endDate and endDate between @yesterdayAt9 and @endDate and Location = 'SG' and (Position = 'Med Rev' or Position = 'Sr Med Rev')  and (wst.EndDate BETWEEN @startDate AND @endDate OR (wst.EndDate IS NULL)) AND (Form348.member_compo = 6 OR Form348_SARC.member_compo = 6
OR Form348_RR.member_compo = 6 OR Form348_SC.Member_Compo = 6 OR Form348_AP.member_compo = 6 OR Form348_AP_SARC.member_compo = 6)

ORDER BY LOCATION, Position

--[usp].[GetStatusOfCurrentWeek_SGCases]
GO

