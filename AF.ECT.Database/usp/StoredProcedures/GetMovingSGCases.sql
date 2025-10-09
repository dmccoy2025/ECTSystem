Create Procedure [usp].[GetMovingSGCases]
AS
DECLARE 
@startDate DATETIME = '2014-01-01 00:00:00.000',                -- INPUT: PAST DATE TO BEGIN SEARCHING FROM (format: YYYY-MM-DD)
-------- Removed: 
-------- @endDate DATETIME = '2020-10-19 13:00:00.000'                    -- INPUT: DATE TO STOP THE SEARCH AT (format: YYYY-MM-DD)



------time set to 13:00 like like appended code------
@endDate DATETIME = CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP)) + '09:00',
@today VARCHAR(20) = CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP)) + '09:00',
@thursday VARCHAR(10) = CONVERT(VARCHAR(10), DATEPART(weekday, getdate()), 111),
@test1 DATETIME,
@endDateName VARCHAR(15),
@reportStartDate VARCHAR(20) = CONVERT(VARCHAR(10), getdate(), 111),
@statusAsOf VARCHAR(20),
@timesMoves smallInt = 0,
@Case_Id varchar(50) 


SET @endDateName =
CASE DATEPART(weekday, CURRENT_TIMESTAMP)
        WHEN 2 THEN CONVERT(VARCHAR(15), DATENAME(WEEKDAY, getdate()-4)) 
        WHEN 3 THEN CONVERT(VARCHAR(15), DATENAME(WEEKDAY, getdate()-5))
        WHEN 4 THEN CONVERT(VARCHAR(15), DATENAME(WEEKDAY, getdate()-6))
        WHEN 5 THEN CONVERT(VARCHAR(15), DATENAME(WEEKDAY, getdate()-7))
        WHEN 6 THEN CONVERT(VARCHAR(15), DATENAME(WEEKDAY, getdate()-1))
        ELSE CONVERT(VARCHAR(15), CONVERT(DATE, CURRENT_TIMESTAMP))
        END


SET @reportStartDate = 
CASE DATEPART(weekday, @reportStartDate)
        WHEN 2 THEN CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP-4)) + '09:00'
        WHEN 3 THEN CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP-5)) + '09:00'
        WHEN 4 THEN CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP-6)) + '09:00'
        WHEN 5 THEN CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP-7)) + '09:00'
        WHEN 6 THEN CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP-1)) + '09:00'
        ELSE CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP))
        END


Set @statusAsOf = 'Status as of:' + @today





    

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
END AS 'Case_Id',


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
        
sc.description, 
wst.name, 
wst.startDate, 
wst.endDate, 
scl.Position, 
scl.[POSITION_RANK],
scl.Location,
--CASE 
--    When pw.Description <> sc.description THEN @timesMoves + 1
--    WHEN @timesMoves = 0 OR @timesMoves IS NULL THEN 0
--END 'Times_Moved',
CASE 
    WHEN wst.startDate < GETDATE() AND wst.endDate <= GETDATE() OR wst.endDate IS NULL THEN 'Open'
END 'StatusAsOf',
		@startDate 'Report_Start_Date',
        @endDate 'Report_End_Date',

--Select Count(CASE WHEN Form348.case_id IS NOT NULL THEN Form348.case_id ELSE
--    CASE WHEN Form348_SARC.case_id IS NOT NULL THEN Form348_SARC.case_id ELSE
--        CASE WHEN Form348_RR.Case_Id IS NOT NULL THEN Form348_RR.Case_Id ELSE
--            CASE WHEN Form348_SC.Case_Id IS NOT NULL THEN Form348_SC.Case_Id ELSE
--                CASE WHEN Form348_AP.Case_Id IS NOT NULL THEN Form348_AP.Case_Id ELSE
--                    CASE WHEN Form348_AP_SARC.Case_Id IS NOT NULL THEN Form348_AP_SARC.Case_Id END
--                END
--            END
--        END
--    END
--END) AS 'Case_Id',
--CASE 
-- When = 'CaseId''20150127-005-WD'  Then @timesMoves + 1
-- Else @timesMoves 
--End AS 'Moved'
CASE 
	WHEN 'Case_Id' = pe.CaseId THEN @timesMoves + 1
END 'Moved',
pe.caseId


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
left JOIN [dbo].[PreviousDaysECTCases] pe on pe.CaseId = (CASE WHEN Form348.case_id IS NOT NULL THEN Form348.case_id ELSE
    CASE WHEN Form348_SARC.case_id IS NOT NULL THEN Form348_SARC.case_id ELSE
        CASE WHEN Form348_RR.Case_Id IS NOT NULL THEN Form348_RR.Case_Id ELSE
            CASE WHEN Form348_SC.Case_Id IS NOT NULL THEN Form348_SC.Case_Id ELSE
                CASE WHEN Form348_AP.Case_Id IS NOT NULL THEN Form348_AP.Case_Id ELSE
                    CASE WHEN Form348_AP_SARC.Case_Id IS NOT NULL THEN Form348_AP_SARC.Case_Id END
                END
            END
        END
    END
END)
WHERE wst.StartDate BETWEEN @startDate AND @endDate 
--AND (wst.EndDate BETWEEN @startDate AND @endDate OR (wst.EndDate IS NULL)) 
AND wst.endDate IS NULL 
AND scl.Location = 'SG' 
--AND scl.POSITION = 'Tech Pre'
AND(Form348.member_compo = 6 OR Form348_SARC.member_compo = 6
OR Form348_RR.member_compo = 6 OR Form348_SC.Member_Compo = 6 OR Form348_AP.member_compo = 6 OR Form348_AP_SARC.member_compo = 6)
--ORDER BY 'Case_Id'


--[usp].[GetStatusOfCurrentWeek_SGCases]

--SELECT * 
--FROM [dbo].[PreviousDaysECTCases]
--where CaseId = '20201118-034'
GO

