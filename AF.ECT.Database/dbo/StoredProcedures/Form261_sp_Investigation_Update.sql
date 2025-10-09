-- =============================================
-- Author:		Nandita Srivastava
-- Create date: Jan 29  2009
-- Description:	Updates the Lod Investigation Information 
-- =============================================

CREATE PROCEDURE [dbo].[Form261_sp_Investigation_Update]
		@lodId int,
	
		@reportDate datetime=null, 
		@investigationOf tinyint=null, 
		@status tinyint=null, 
		@inactiveDutyTraining nchar(10)=null, 
		@durationStartDate  datetime=null, 
		@durationFinishDate  datetime=null, 
		@findingsDate   datetime=null,
		@place varchar(50)=null,
		@howSustained varchar(100)=null,
		@medicalDiagnosis varchar(100)=null,
		@presentForDuty varchar(1)=null,
		@absentWithAuthority  varchar(1)=null,
		@intentionalMisconduct  varchar(1)=null,
		@mentallySound  varchar(1)=null,
		@remarks varchar(1000)=null,
		@finding   tinyint=null




AS
BEGIN
	SET NOCOUNT ON;

--if lod does not  already exist insert primary key 
IF not exists (SELECT lodid FROM Form261_Findings  WHERE lodid=@lodId)
	INSERT INTO Form261_Findings (lodId) VALUES (@lodId)
--Update record 
	UPDATE Form261_Findings  set 
	 	--Findings Information 
				 findingsDate=@findingsDate,
				 place=@place,
				 howSustained=@howSustained,
				 medicalDiagnosis=@medicalDiagnosis,
				 presentForDuty=@presentForDuty,
				 absentWithAuthority=@absentWithAuthority,
				 intentionalMisconduct=@intentionalMisconduct,
				 mentallySound=@mentallySound,
				 remarks=@remarks,
				 finding= @finding
				 
	 
		WHERE Form261_Findings.lodid=@lodid 

--Form 261  Information 
--if lod does not  already exist insert primary key 
IF not exists (SELECT lodid FROM Form261   WHERE lodid=@lodId)
	INSERT INTO Form261  (lodId) VALUES (@lodId)

		UPDATE Form261   set 
			   reportDate=@reportDate,
				investigationOf=@investigationOf,
				status=@status,
				inactiveDutyTraining=@inactiveDutyTraining,
				durationStartDate=@durationStartDate,
				durationFinishDate=@durationFinishDate
		WHERE Form261.lodid=@lodid 


END
GO

