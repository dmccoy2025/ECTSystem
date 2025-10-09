--exec Test_form348_sp_Search 

CREATE PROCEDURE [dbo].[GetPassCodeList]
		  @type varchar(10)=null
		, @pascode varchar(4)=null
		, @rptView tinyInt=null
		 
		

	AS

		If ( @type='CHILD' )
		BEGIN 
				SELECT cst.child_pas FROM Command_Struct_Tree cst
				LEFT JOIN Command_Struct cs on cs.cs_id =cst.child_id
				WHERE cst.parent_pas=@pascode and cst.view_type=@rptView  and  cs.Inactive =0 
				 
		END	  
		If ( @type='PARENT' )
		BEGIN 
				SELECT cst.parent_pas FROM Command_Struct_Tree  cst 
			    LEFT JOIN Command_Struct cs on cs.cs_id =cst.parent_id
			    WHERE cst.child_pas=@pascode and cst.view_type=@rptView  and  cs.Inactive =0 
		END
GO

