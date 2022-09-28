IF EXISTS (SELECT 'X' FROM sys.schemas WHERE name = 'Offchain')
BEGIN
    IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] WHERE Id='8b93f57a-8717-4871-a191-9ddcbf928388' AND Status = 1)
	BEGIN
        BEGIN TRY
			UPDATE Offchain.Movement SET RetryCount=0 WHERE BlockchainStatus=1
            UPDATE Offchain.Node SET RetryCount=0 WHERE BlockchainStatus=1

			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('8b93f57a-8717-4871-a191-9ddcbf928388', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('8b93f57a-8717-4871-a191-9ddcbf928388', 0);
		END CATCH
    END
END