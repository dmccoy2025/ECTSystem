








-- ================================================================= 

-- Author:	Raymond Hall

-- Create date: November 2 2021 

-- Description:	Returns all special cases in consult  

-- ================================================================== 

CREATE PROCEDURE [dbo].[form348_sp_InConsultCaseSearch_all] 
AS 

DECLARE 
@post_Completion_Sign_Date DateTime;

 
Select  

DISTINCT(f.SC_Id) as RefId,f.case_id  as CaseId   
--, w.ws_id
--,w.statusId
, f.member_name  as Name 
, CS.STATE as State 
, RIGHT( f.member_ssn, 4) as SSN  
, f.member_compo as Compo 
, cs.PAS_CODE as Pascode 
, f.status as WorkStatusId 
--, s.description as Description 
--, s.isFinal as IsFinal 
, f.workflow  as WorkflowId 
, wf.title as Workflow 
, wf.moduleId as ModuleId 
--, f.formal_inv  as IsFormal 
, CONVERT(char(11), f.created_date, 101) DateCreated 
, cs.long_name as UnitName 
, CONVERT(char(11), ISNULL(t.ReceiveDate, f.created_date), 101) ReceiveDate 
--, ISNULL(pcd.actionDate, pcdI.actionDate) as CompletionDate
--, pcdI.actionDate
--,pcd.notes

----****************************Need to change this it will just shows todaydate--****************************
, CURRENT_TIMESTAMP as CompletionDate

--, DATEDIFF(d, ISNULL(pcd.actionDate, pcdI.actionDate), GETDATE()) as Days 

--****************************Need to change this it will just shows 0*****************************
, DATEDIFF(d, ISNULL(t.ReceiveDate, f.created_date), GETDATE()) as Days 

, ISNULL(l.id,0) lockId 

--, ( 

/******	ISNULL(pas.priority, 0) + DATEDIFF(d, ISNULL(t.ReceiveDate, f.created_date), GETDATE()) 

--  ) as PassPriority******/													-- added for 125 

, CASE  
	WHEN pas.priority >= 60000 THEN 'P1a' 
	WHEN pas.priority BETWEEN 50000 and 59999 THEN 'P1b' 
	WHEN pas.priority BETWEEN 40000 and 49999 THEN 'P2' 
	ELSE '' 
  END AS PriorityRank	 

  FROM [ALOD].[dbo].[core_WorkStatus] w, 

  --join core_StatusCodes s on s.statusId	 = w.statusId  

  --join core_WorkStatus_Options o on o.ws_id = w.ws_id 

  --left join 
  Form348_SC f --on f.status = w.ws_id 

  JOIN core_workflow wf			ON wf.workflowId = f.workflow 

  JOIN COMMAND_STRUCT cs ON cs.cs_id		= f.member_unit_id 

  left join core_workflowLocks l	ON l.refId		= f.SC_Id and l.module = 2 

  LEFT JOIN ( 
	SELECT	max(startDate) ReceiveDate,  ws_id,	refId  
	FROM core_WorkStatus_Tracking  
	GROUP BY ws_id, refId 
	) t								
	ON t.refId = f.SC_Id AND t.ws_id = f.status 

   LEFT JOIN core_lkupPAS pas		ON pas.pas = cs.PAS_CODE 

 where f.status in (
	select ws_id from core_WorkStatus
	where isConsult = 1 )


	order by WorkStatusId
--[dbo].[form348_sp_InConsultCaseSearch_all]
--[dbo].[form348_sp_InConsultCaseSearch_all]
GO

