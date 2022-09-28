/*-- ============================================================================================================================
-- Author:          InterGrupo
-- Created Date:	Jun-06-2021
-- Updated Date:    Jun-17-2021 Add ownership date parameter
-- Updated Date:    Jun-17-2021 Add percentage validation
-- <Description>:   This function returns the purchase value that is sent to FICO. </Description>
-- Select [Admin].[udf_GetContractValue](497)(489)
-- ==============================================================================================================================*/
CREATE FUNCTION [Admin].[udf_GetContractValue](@contractId INT, @Date DATE)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @boughtByFico FLOAT
		   ,@valueToBuy FLOAT
		   ,@valueToBuyFinal FLOAT
		   ,@movementTypeId INT
		   ,@isVolume BIT
		   ,@startDate DATETIME
		   ,@days INT;

	SELECT @valueToBuy = CASE
		WHEN CE.Name LIKE '%[%]%' OR UPPER(CE.Name) = 'PORCENTAJE' THEN Volume
		WHEN [Frequency] = 'Diario'  THEN Volume
		WHEN [Frequency] = 'Quincenal' THEN Volume/15
		WHEN [Frequency] = 'Mensual' THEN Volume/DAY(EOMONTH(@Date)) END
		,@movementTypeId =  IIF(C.MovementTypeId = 191, 50, C.MovementTypeId)
		,@isVolume = CASE WHEN CE.Name LIKE '%[%]%' OR UPPER(CE.Name) = 'PORCENTAJE' THEN 0 ELSE 1 END
		,@startDate = C.StartDate
		FROM Admin.Contract C
	    INNER JOIN Admin.CategoryElement CE ON CE.ElementId = C.MeasurementUnit
		WHERE ContractId = @contractId;

	SELECT @boughtByFico = SUM(Volume)
		FROM Admin.MovementContract MC 
		JOIN Offchain.Movement M ON MC.MovementContractId = M.MovementContractId
		WHERE MONTH(M.OperationalDate) = MONTH(@Date)
		AND  YEAR(M.OperationalDate) = YEAR(@Date)
		AND MC.MovementTypeId = @movementTypeId
		AND MC.ContractId = @contractId;
	
	SET @days = IIF(MONTH(@startDate) = MONTH(@Date) AND  YEAR(@startDate) = YEAR(@Date),DAY(@Date)-DAY(@startDate),(DAY(@Date) - 1));
	SET @valueToBuyFinal = @valueToBuy + @valueToBuy * @days - IIF(@boughtByFico IS NULL,0,@boughtByFico);
	
	RETURN IIF(@isVolume = 1, IIF(@valueToBuyFinal < 0, 0, @valueToBuyFinal), @valueToBuy);
END;

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This function returns the purchase value that is sent to FICO.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'FUNCTION',
    @level1name = N'udf_GetContractValue',
    @level2type = NULL,
    @level2name = NULL