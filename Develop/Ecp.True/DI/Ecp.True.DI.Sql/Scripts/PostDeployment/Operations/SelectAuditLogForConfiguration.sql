SELECT 
	[User],
	LogType,
	DAY(LogDate) as [Day],
	MONTH(LogDate) as [Month],
	YEAR(LogDate) as [Year],
	COUNT(AuditLogId) as ActionCounter
FROM [Audit].AuditLog
WHERE LogDate BETWEEN '2021-09-01' AND '2022-02-28' AND Entity IN(
    'Admin.HomologationDataMapping',
    'Admin.StorageLocationProduct',
    'Admin.NodeConnectionProduct',
    'Admin.NodeConnectionProductOwner',
    'Admin.NodeConnection',
    'Admin.NodeTag',
    'Admin.StorageLocationProductOwner',
    'Admin.Node',
    'Admin.NodeStorageLocation',
    'Admin.Event',
    'Admin.StorageLocationProductVariable',
    'Admin.Contract',
    'Admin.CategoryElement',
    'Analytics.OperativeNodeRelationshipWithOwnership',
    'Analytics.OperativeNodeRelationship',
    'Admin.Category')
GROUP BY [User], LogType,DAY(LogDate), MONTH(LogDate), YEAR(LogDate)
ORDER BY YEAR(LogDate), MONTH(LogDate), DAY(LogDate), [User]