
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	The stored procedure used for the Total Count by Process
--				report. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/17/2015
-- Work Item:		TFS Task 386
-- Description:		Cleaned up the stored procedure. Changed the references
--					to the MPF to referencing the LOD-PM. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	8/1/2016
-- Description:		Updated to use the status codes instead of the work statuses
--					for the sum calculations in the @parents table.
-- ============================================================================
CREATE proc [dbo].[report_OpenStatusCount]
	@cs_id int,
	@VIEW int,
	@beginDate datetime,
	@endDate datetime 
AS
BEGIN
	DECLARE @csc_id int, @long_name varchar(200), @pas_code varchar(4)
 	
 	SET @csc_id = (SELECT csc_id FROM Command_Struct_Chain WHERE CS_ID = @cs_id AND view_type = @VIEW)
 	
 	SELECT	@long_name = long_name, @pas_code = pas_code
 	FROM	Command_Struct 
 	WHERE	CS_ID = @cs_id 
     

	DECLARE @parents TABLE
	(
		 cs_id int
		,pas char(4)
		,name varchar(200)
		,medical int
		,unit int
		,wingja int
		,wingcc int
		,lodPM int
		,formalInvestigation int
		,formalActionWingJA int
		,formalActionWingCC int
		,LodBoard int 
		,FormalHQ int
		,ApprovingAuthority int
		,formalInvestigationDirected int
		,Total int
	)

	-- Create the command chain
	INSERT	INTO @parents (cs_id, name, pas) 
	VALUES	(@cs_id,@long_name,@pas_code)

	INSERT	INTO	@parents (cs_id, name, pas) 
			SELECT	s.CS_ID, s.LONG_NAME, s.PAS_CODE
			FROM	Command_Struct_Chain a
					JOIN Command_Struct s ON s.CS_ID = a.CS_ID
			WHERE	(CSC_ID_PARENT = @csc_id)AND view_type  = @VIEW

	-- Create the temp table to store count
	DECLARE @counts TABLE
	(
		sc_Id INT,
		ws_id int,
		num int
	)

	DECLARE @curId int, @parentPas char(4)

	DECLARE c CURSOR FOR SELECT cs_id, pas FROM @parents
	
	OPEN c
	
	FETCH next FROM c INTO @curId, @parentPas

	WHILE @@FETCH_Status = 0
	BEGIN
		DELETE FROM @counts;
		
		INSERT	INTO @counts ([sc_Id], [ws_id], [num])
				SELECT	a.statusId,
						ws_id, 
						(
							SELECT	COUNT(*) AS TotalCount
							FROM	Form348 AS b
							WHERE	(b.status = a.ws_id  )
									AND (b.deleted = 0) 
									AND b.created_date BETWEEN @beginDate AND @endDate
									AND b.member_unit_id IN 
									(
										SELECT	child_id 
										FROM	Command_Struct_Tree t
										WHERE	parent_id = @curId AND view_type = @view
									)
						) AS TotalCount
				FROM	vw_workstatus AS a 
				WHERE	a.IsFinal=0 
						AND moduleId=2		-- All Line of Duty work statuses
				GROUP	BY a.statusId, a.ws_id

		--SELECT * FROM @counts;

		--UPDATE @parents SET medical = (SELECT Sum(num) FROM @counts WHERE ws_id in(1,2))  WHERE cs_id = @curId --Medoff 2 and medtech-1
		--UPDATE @parents SET unit =  (SELECT Sum(num) FROM @counts WHERE ws_id = 3) WHERE cs_id = @curId --unit cc
		--UPDATE @parents SET wingja = (SELECT Sum(num) FROM @counts WHERE ws_id = 4)  WHERE cs_id = @curId--wing ja
		--UPDATE @parents SET wingcc =  (SELECT Sum(num) FROM @counts WHERE ws_id = 5)  WHERE cs_id = @curId--wing cc
		--UPDATE @parents SET lodPM =  (SELECT Sum(num) FROM @counts WHERE ws_id = 7)  WHERE cs_id = @curId--lodPM
		--UPDATE @parents SET formalInvestigation =  (SELECT Sum(num) FROM @counts WHERE ws_id = 11)  WHERE cs_id = @curId--IO
		--UPDATE @parents SET formalActionWingJA =  (SELECT Sum(num) FROM @counts WHERE ws_id = 14)  WHERE cs_id = @curId--formalActionWingJA
		--UPDATE @parents SET formalActionWingCC =  (SELECT Sum(num) FROM @counts WHERE ws_id = 15)  WHERE cs_id = @curId--formalAction appoinitng authority
		--UPDATE @parents SET LodBoard = (SELECT Sum(num) FROM @counts WHERE ws_id in(8,10,16 ) )  WHERE cs_id = @curId--board,mbdmed,bdlegal,bdsenior  
		--UPDATE @parents SET FormalHQ =  (SELECT Sum(num) FROM @counts WHERE  ws_id in(18,19,20))  WHERE cs_id = @curId--frmlbd,frmlbdmed,frmlbdlegal,frmlbdsenir, frmldirectedtobd
		--UPDATE @parents SET ApprovingAuthority =  (SELECT Sum(num) FROM @counts WHERE ws_id in(17, 22) ) WHERE cs_id = @curId--aa and frml aa
		--UPDATE @parents SET formalInvestigationDirected =  (SELECT Sum(num) FROM @counts WHERE ws_id in( 23) ) WHERE cs_id = @curId--aa and frml aa

		UPDATE @parents SET medical =						(SELECT Sum(num) FROM @counts WHERE sc_Id in(1,3))			WHERE cs_id = @curId --Medoff and medtech
		UPDATE @parents SET unit =							(SELECT Sum(num) FROM @counts WHERE sc_Id = 4)				WHERE cs_id = @curId --unit cc
		UPDATE @parents SET wingja =						(SELECT Sum(num) FROM @counts WHERE sc_Id = 5)				WHERE cs_id = @curId--wing ja
		UPDATE @parents SET wingcc =						(SELECT Sum(num) FROM @counts WHERE sc_Id = 6)				WHERE cs_id = @curId--wing cc
		UPDATE @parents SET lodPM =							(SELECT Sum(num) FROM @counts WHERE sc_Id = 7)				WHERE cs_id = @curId--lodPM
		UPDATE @parents SET formalInvestigation =			(SELECT Sum(num) FROM @counts WHERE sc_Id = 8)				WHERE cs_id = @curId--IO
		UPDATE @parents SET formalActionWingJA =			(SELECT Sum(num) FROM @counts WHERE sc_Id = 9)				WHERE cs_id = @curId--formalActionWingJA
		UPDATE @parents SET formalActionWingCC =			(SELECT Sum(num) FROM @counts WHERE sc_Id = 10)				WHERE cs_id = @curId--formalAction appoinitng authority
		UPDATE @parents SET LodBoard =						(SELECT Sum(num) FROM @counts WHERE sc_Id in(11,12,13,169))	WHERE cs_id = @curId--board,mbdmed,bdlegal,bdsenior  
		UPDATE @parents SET FormalHQ =						(SELECT Sum(num) FROM @counts WHERE sc_Id in(20,21,22,170))	WHERE cs_id = @curId--frmlbd,frmlbdmed,frmlbdlegal,frmlbdsenir, frmldirectedtobd
		UPDATE @parents SET ApprovingAuthority =			(SELECT Sum(num) FROM @counts WHERE sc_Id in(15,24))		WHERE cs_id = @curId--aa and frml aa
		UPDATE @parents SET formalInvestigationDirected =	(SELECT Sum(num) FROM @counts WHERE sc_Id in(25))			WHERE cs_id = @curId--aa and frml aa
				
				 
		FETCH next FROM c INTO @curId, @parentPas
	END

	CLOSE c
	DEALLOCATE c

	SELECT	cs_id, pas, @VIEW viewType, name			 
			,isnull(medical,0) as medical,isnull(unit,0) unit,isnull(wingja,0) wingja,isnull(wingcc,0) wingcc,isnull(lodPM,0) lodPM
			,isnull(formalInvestigation,0)formalInvestigation,isnull(formalActionWingJA,0)formalActionWingJA
			,isnull(formalActionWingCC,0) formalActionWingCC,isnull(LodBoard,0) +isnull(FormalHQ,0) LodBoard
		--	,isnull(formalActionWingCC,0) formalActionWingCC,isnull(LodBoard,0) LodBoard,isnull(FormalHQ,0) FormalHQ
			,isnull(ApprovingAuthority ,0)ApprovingAuthority, isnull(formalInvestigationDirected ,0)formalInvestigationDirected
			,isnull(medical,0)+isnull(unit,0)+isnull(wingja,0)+isnull(wingcc,0)+isnull(lodPM,0)
			+isnull(formalInvestigation,0)+isnull(formalActionWingJA,0)
			+isnull(formalActionWingCC,0)+isnull(LodBoard,0)+isnull(FormalHQ,0)
			+isnull(ApprovingAuthority,0)+isnull(formalInvestigationDirected ,0) AS Total  
	FROM	@parents
END
GO

