/*==============================================================================================================================
--Author:        Microsoft
--Created Date : May-14-2020
--<Description>: This table is used to fetch the data for Node Connections related to the Nodes, to be displayed in the Node Configuration Report. </Description>
================================================================================================================================*/

CREATE TABLE [Admin].NodeConnectionInfo
(
    --Columns
   [ConnectionType]                 VARCHAR(20)     NULL,
   [NodeName]                       NVARCHAR(150)   NULL,
   [NodeConnectionName]             NVARCHAR(150)   NULL,
   [ProductName]                    NVARCHAR(150)   NULL,
   [ProductId]                      NVARCHAR(20)    NULL,
   [Priority]                       INT             NULL,
   [TransferPoint]                  VARCHAR(5)      NULL,
   [AlgorithmName]                  NVARCHAR(150)   NULL,
   [OwnershipStrategy]              NVARCHAR(100)   NULL,
   [UncertaintyConnectionProduct]   DECIMAL(9,6)   NULL,
   [OwnershipPercentage]            DECIMAL(9,6)   NULL,
   [OwnerName]                      NVARCHAR(150)   NULL,
   [Category]                       NVARCHAR(150)   NOT NULL,
   [Element]                        NVARCHAR(150)   NOT NULL,
   [NodeId]                         INT             NULL,
   [RNo]                            BIGINT          NULL,
   [ExecutionId]                    NVARCHAR(250)   NOT NULL,
   [LastLoadedDate]                 DATETIME        NULL    DEFAULT Admin.udf_GetTrueDate(),

   --Indexes
   INDEX NIX_NodeConnectionInfo_CatCatEleNode NONCLUSTERED (Category,Element,NodeName)
);
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The type of connection (Entrada,Salida)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionInfo',
    @level2type = N'COLUMN',
    @level2name = N'ConnectionType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionInfo',
    @level2type = N'COLUMN',
    @level2name = N'NodeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of node connection',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionInfo',
    @level2type = N'COLUMN',
    @level2name = N'NodeConnectionName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The product name',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionInfo',
    @level2type = N'COLUMN',
    @level2name = N'ProductName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionInfo',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the model',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionInfo',
    @level2type = N'COLUMN',
    @level2name = N'AlgorithmName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of Estrategia de propiedad for node connections',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionInfo',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipStrategy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The uncertaintity percentage of node connection product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionInfo',
    @level2type = N'COLUMN',
    @level2name = N'UncertaintyConnectionProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The ownership percentage of node connection product owner',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionInfo',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the owner of node connection product owner',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionInfo',
    @level2type = N'COLUMN',
    @level2name = N'OwnerName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the category',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionInfo',
    @level2type = N'COLUMN',
    @level2name = N'Category'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionInfo',
    @level2type = N'COLUMN',
    @level2name = N'Element'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionInfo',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The rank over ConnectionType, NodeConnectionName, ProductName',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionInfo',
    @level2type = N'COLUMN',
    @level2name = N'RNo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The execution id unique to session received from UI',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionInfo',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The date when this row was inserted',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionInfo',
    @level2type = N'COLUMN',
    @level2name = N'LastLoadedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table is used to fetch the data for Node Connections related to the Nodes, to be displayed in the Node Configuration Report.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionInfo',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The priority of connection or product between 1 and 10',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionInfo',
    @level2type = N'COLUMN',
    @level2name = N'Priority'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Si (Yes) if node connection is a transfer point, else No',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeConnectionInfo',
    @level2type = N'COLUMN',
    @level2name = N'TransferPoint'