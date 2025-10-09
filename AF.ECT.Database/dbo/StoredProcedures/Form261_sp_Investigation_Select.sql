-- =============================================
-- Author:		Nandita Srivastava
-- Create date: June 5  2008
-- Description:	Selects the Lod Investigation Information 
-- =============================================

CREATE PROCEDURE [dbo].[Form261_sp_Investigation_Select]
		@lodid int
AS
BEGIN
	SET NOCOUNT ON;
		
		SELECT  
			--Form 261  Information 
				a.reportDate,
				a.investigationOf,
				a.status,
				a.inactiveDutyTraining,
				a.durationStartDate,
				a.durationFinishDate,
			--Findings Information 
				b.findingsDate,
				b.place,
				b.howSustained,
				b.medicalDiagnosis,
				b.presentForDuty,
				b.absentWithAuthority,
				b.intentionalMisconduct,
				b.mentallySound,
				b.remarks,
				b.finding
				 
		FROM Form261 a WITH (nolock)  

		LEFT JOIN Form261_Findings b on a.lodId=b.lodId

		WHERE a.lodid=@lodid 

END
GO

