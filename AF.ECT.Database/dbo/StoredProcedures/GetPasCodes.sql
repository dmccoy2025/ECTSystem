--exec GetPasCodes 'CHILD' , 1016050, 6

CREATE PROCEDURE [dbo].[GetPasCodes]
		  @type varchar(10)=null
		, @cs_id int=null
		, @rptView tinyInt=null
		 
 
AS

If ( @type='CHILD' )
BEGIN 
	select a.child_id as Value, c.LONG_NAME + ' (' + a.child_pas + ')' as 'Name'
	from Command_Struct_Tree a
	join Command_Struct c on c.CS_ID = child_id
	where parent_id = @cs_id and view_type = @rptView
	order by c.LONG_NAME;
		--SELECT cst.child_id as Value,   cs.long_name +' ('+cst.child_pas+')' as Name FROM Command_Struct_Tree cst
		--LEFT JOIN Command_Struct cs on cs.cs_id =cst.child_id
		-- WHERE cst.parent_id=@cs_id and cst.view_type=@rptView   and cs.Inactive =0 
		-- order by cs.long_name 
END	  
If ( @type='PARENT' )
BEGIN 
	select a.parent_id as Value, c.LONG_NAME + ' (' + a.parent_pas + ')' as 'Name'
	from Command_Struct_Tree a
	join Command_Struct c on c.CS_ID = parent_id
	where parent_id = @cs_id and view_type = @rptView
	order by c.LONG_NAME;

		--SELECT cst.parent_id as Value,'('+cst.parent_pas+') ' + cs.long_name as Name FROM Command_Struct_Tree cst
		--LEFT JOIN Command_Struct cs on cs.cs_id =cst.parent_id
		-- WHERE cst.child_id=@cs_id and cst.view_type=@rptView and cs.Inactive =0

END
GO

