
Create PROCEDURE [dbo].[core_witness_Update]
(
	@id int,
	@name varchar(50),
	@address varchar(50)
)
AS
	SET NOCOUNT on;

Update core_Witness 
SET name = @name,
    address = @address 
WHERE     (id = @id)
GO

