/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date:	Jan-21-2020
-- Updated Date:	Mar-20-2020
-- <Description>:   This is a helper function to convert input string to pascal casing and primarily used in PowerBi reports. </Description>
-- ==============================================================================================================================*/

CREATE FUNCTION [Admin].[udf_PascalCase] 
(
    @InputString NVARCHAR(MAX) 
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
DECLARE @Index INT
DECLARE @Char NCHAR(1)
DECLARE @OutputString NVARCHAR(MAX)
SET @OutputString = LOWER(@InputString)
SET @Index = 1
WHILE @Index <= LEN(@InputString)
BEGIN
IF (SUBSTRING(@OutputString, @Index, 1) LIKE '[a-zA-Z]')
BEGIN
	SET @OutputString = STUFF(@OutputString,@Index,1,UPPER(SUBSTRING(@OutputString, @Index, 1)))
	SET @Index = @Index + 1
	BREAK
END
ELSE
BEGIN
	SET @Index = @Index + 1
END
END
WHILE @Index <= LEN(@InputString)
BEGIN
SET @Char = SUBSTRING(@InputString, @Index, 1)
IF @Char IN (' ', ';', ':', '!', '?', ',', '.', '_', '-', '/', '&','''','(')
IF @Index + 1 <= LEN(@InputString)
BEGIN
WHILE @Index + 1 <= LEN(@InputString)
BEGIN
IF (SUBSTRING(@OutputString, @Index + 1, 1) LIKE '[a-zA-Z]')
BEGIN
	SET @OutputString = STUFF(@OutputString,@Index + 1,1,UPPER(SUBSTRING(@OutputString, @Index + 1, 1)))
	SET @Index = @Index + 1 
	BREAK
END
ELSE
BEGIN
	SET @Index = @Index + 1 
END
END
END
SET @Index = @Index + 1
END
RETURN ISNULL(@OutputString,'')
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This is a helper function to convert input string to pascal casing and primarily used in PowerBi reports.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'FUNCTION',
    @level1name = N'udf_PascalCase',
    @level2type = NULL,
    @level2name = NULL