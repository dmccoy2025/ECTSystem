

CREATE PROCEDURE [dbo].[dev_sp_GetUnits]

AS

SELECT a.cs_id AS id, CONVERT(varchar, order_num) + ' - '  + long_name + '(' + pas_code + ')' AS name 
FROM dev_units a
JOIN Command_Struct c ON c.CS_ID = a.cs_id
WHERE a.unit_type = 'W' and c.Inactive =0
ORDER BY order_num
GO

