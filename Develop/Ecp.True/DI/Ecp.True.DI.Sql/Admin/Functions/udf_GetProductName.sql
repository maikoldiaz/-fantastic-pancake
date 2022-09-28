/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date:	Jul-14-2020 
-- <Description>:   This is a function to Return Product Name Based On ProductId </Description>
-- ==============================================================================================================================*/
CREATE FUNCTION [Admin].[udf_GetProductName] 
(
    @ProductId               NVARCHAR(20)
)
RETURNS NVARCHAR(300)
AS
BEGIN

    DECLARE @ProductName NVARCHAR(300)

    SELECT TOP 1 @ProductName = [Name]
	FROM  [Admin].Product
    WHERE ProductId = @ProductId
    RETURN @ProductName
END
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
                            @value = N'This is a function to Return Product Name Based On ProductId',
                            @level0type = N'SCHEMA',
                            @level0name = N'Admin',
                            @level1type = N'FUNCTION',
                            @level1name = N'udf_GetProductName',
                            @level2type = NULL,
                            @level2name = NULL