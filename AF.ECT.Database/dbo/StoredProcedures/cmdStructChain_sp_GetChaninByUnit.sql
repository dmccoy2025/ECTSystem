

CREATE PROCEDURE [dbo].[cmdStructChain_sp_GetChaninByUnit]
(
	@cs_id int,
	@viewType int
)

AS
	SET NOCOUNT ON;


--SELECT @userPasCode = PAS_CODE FROM VW_USERS WHERE userId = @userId
--SELECT @cs_id = CS_ID FROM COMMAND_STRUCT WHERE PAS_CODE=@userPasCode -- @userCS = 20151543
/*
DECLARE  @csc_id INT
SELECT @csc_id = CSC_ID FROM COMMAND_STRUCT_CHAIN WHERE CS_ID=@cs_id AND view_type = @viewType -- @userCSC_ID = 29935825


SELECT DISTINCT  b.cs_id , b.long_name + ' (' + b.pas_code + ')'as unit
FROM 
	COMMAND_STRUCT_CHAIN a
JOIN
	COMMAND_STRUCT b ON b.cs_id = a.cs_id

WHERE   (a.CSC_ID_PARENT= @csc_id OR a.CS_ID = @cs_id) 
		AND a.view_type= @viewType	
		AND a.Cs_ID IS NOT NULL
		AND b.pas_code IS NOT NULL
ORDER BY unit

*/
    SELECT DISTINCT  b.cs_id , b.long_name + ' (' + b.pas_code + ')'as unit
    FROM COMMAND_STRUCT b 
    JOIN ( 
            SELECT child_id FROM Command_Struct_Tree WHERE  parent_id=@cs_id and  view_type=@viewType
         )t
	ON b.cs_id = t.child_id
GO

