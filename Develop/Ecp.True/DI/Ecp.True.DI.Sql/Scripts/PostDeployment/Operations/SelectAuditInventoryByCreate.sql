SELECT 
	CreatedBy,
	EventType,
	DAY(CreatedDate) as [Day],
	MONTH(CreatedDate) as [Month],
	YEAR(CreatedDate) as [Year],
	COUNT(*) as InventoryCounter
FROM Offchain.InventoryProduct 
WHERE CreatedDate BETWEEN '2021-09-01' AND '2022-02-28'
GROUP BY CreatedBy, EventType, DAY(CreatedDate), MONTH(CreatedDate), YEAR(CreatedDate)
ORDER BY YEAR(CreatedDate), MONTH(CreatedDate), DAY(CreatedDate)