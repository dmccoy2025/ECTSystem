
-- =============================================
-- Author:		Nandita Srivastava
-- Create date: June 10  2008
-- Description:	Updates  the Lod Investigation Other Personnel Information 
-- =============================================

/* This procedure will insert Person data against a lodid in form261  table  */
CREATE PROCEDURE [dbo].[Form261_sp_Investigation_UpdateOtherPeronnel]
	
	@lodId int,
	@in_Personnels xml 

AS
 
	

 
		UPDATE   dbo.Form261 
			SET otherPersonnels=@in_Personnels WHERE lodId=@lodId
GO

