SELECT 
	CreatedBy,
	EventType,
	DAY(LastModifiedDate) as [Day],
	MONTH(LastModifiedDate) as [Month],
	YEAR(LastModifiedDate) as [Year],
	COUNT(*) as MovementCounter
FROM Offchain.Movement 
WHERE LastModifiedDate BETWEEN '2021-09-01' AND '2022-02-28'
GROUP BY CreatedBy, EventType, DAY(LastModifiedDate), MONTH(LastModifiedDate), YEAR(LastModifiedDate)
ORDER BY YEAR(LastModifiedDate), MONTH(LastModifiedDate), DAY(LastModifiedDate)