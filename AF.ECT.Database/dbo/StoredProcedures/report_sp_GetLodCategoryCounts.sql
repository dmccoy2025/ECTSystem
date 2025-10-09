
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	7/29/2016
-- Description:		Updated to no longer use a hard coded value for the LOD
--					cancel status.
-- ============================================================================
CREATE PROCEDURE [dbo].[report_sp_GetLodCategoryCounts]
	@cs_id int,
	@VIEW int,
	@beginDate datetime,
	@endDate datetime,
	@isComplete tinyint

AS
BEGIN
	--DECLARE @cs_id int, @VIEW int
	--SET @cs_id = 18
	--SET @VIEW = 2

	DECLARE @csc_id INt, @chain varchar(20)
	DECLARE  @long_name varchar(200) ,@pas_code varchar(4)
 
	SELECT @long_name =long_name,@pas_code=pas_code FROM Command_Struct   WHERE CS_ID = @cs_id 
	SET @csc_id = (SELECT csc_id FROM Command_Struct_Chain WHERE CS_ID = @cs_id AND  view_type = @VIEW )

	DECLARE @parents TABLE
	(
		cs_id int,
		pas char(4),
		name varchar(200),
		disease int,
		injury int,
		illness int,
		death int
	)
 
	DECLARE @counts TABLE
	(
		event_type varchar(20),
		num int
	)
	
	--Insert the top level unit 
	INSERT	INTO @parents (cs_id, name, pas)
			Values( @cs_id,@long_name ,@pas_code) 

	INSERT	INTO	@parents (cs_id, name, pas)
			SELECT	s.CS_ID, s.LONG_NAME, s.PAS_CODE
			FROM	Command_Struct_Chain a
					JOIN Command_Struct s ON s.CS_ID = a.CS_ID
			WHERE	(CSC_ID_PARENT = @csc_id)AND view_type = @VIEW 
	 	
	DECLARE @curId int, @parentPas char(4)

	DECLARE c CURSOR FOR SELECT cs_id, pas FROM @parents

	OPEN c
	FETCH next FROM c INTO @curId, @parentPas

	WHILE @@FETCH_Status = 0
	BEGIN
		DELETE FROM @counts;
	 
		INSERT	INTO	@counts
				SELECT	m.event_nature_type, count(*)
				FROM	Form348 a
						LEFT JOIN vw_workstatus vw ON  vw.ws_id=a.status
						JOIN Form348_Medical m ON m.lodid = a.lodId
				WHERE	coalesce(a.completed_by_unit, a.member_unit_id)		 
						IN
						(
							SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@curId and view_type=@view 
						) 
						AND
						(
							CASE 
								WHEN @isComplete=1 THEN    1 
								WHEN @isComplete=0 THEN    0 
								ELSE  vw.IsFinal
							END =vw.IsFinal 
						)
						AND
						(
							/* This code is to get the records falling on the begin  and end dates also to be included.DATEDIFF(dd, 0, a.created_date ) will get the date only protion */
							DATEDIFF(dd, 0, a.created_date ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate ) 
						)
						AND 
						(	             
							( m.event_nature_type = 'Disease' and   vw.isCancel = 0 )
							Or
							m.event_nature_type <>  'Disease'  
						)    
						AND a.deleted =0      
				GROUP	BY m.event_nature_type

		--SELECT * FROM @counts;

		UPDATE @parents SET disease = (SELECT num FROM @counts WHERE event_type = 'Disease') WHERE cs_id = @curId
		UPDATE @parents SET injury = (SELECT num FROM @counts WHERE event_type = 'Injury') WHERE cs_id = @curId
		UPDATE @parents SET illness = (SELECT num FROM @counts WHERE event_type = 'Illness') WHERE cs_id = @curId
		UPDATE @parents SET death = (SELECT num FROM @counts WHERE event_type = 'Death') WHERE cs_id = @curId

		FETCH next FROM c INTO @curId, @parentPas
	END

	CLOSE c
	DEALLOCATE c

	SELECT	cs_id, pas, @VIEW viewType, name
			,isnull(disease, 0) Disease
			,isnull(injury, 0) Injury
			,isnull(illness, 0) Illness
			,isnull(death, 0) Death
			,isnull(disease, 0)+ isnull(injury, 0)+isnull(illness, 0)+isnull(death, 0) as Total
	FROM	@parents
END
GO

