/*==============================================================================================================================
--Author:        Microsoft
--Created Date : May-14-2020
--<Description>: This table is used to fetch the data for Products related to the Nodes, to be displayed in the Node Configuration Report. </Description>
================================================================================================================================*/

CREATE TABLE [Admin].NodeProductInfo
(
    --Columns
   [ProductName]                    NVARCHAR(150)   NULL,
   [ProductOrder]                   NVARCHAR(20)    NULL,
   [OwnershipStrategy]              NVARCHAR(100)   NULL,
   [OwnerName]                      NVARCHAR(150)   NULL,
   [OwnershipPercentage]            DECIMAL(9,6)    NULL,
   [UncertaintyNodeProduct]         DECIMAL(9,6)    NULL,
   [Category]                       NVARCHAR(150)   NOT NULL,
   [Element]                        NVARCHAR(150)   NOT NULL,
   [NodeName]                       NVARCHAR(150)   NULL,
   [NodeId]                         INT             NULL,
   [PI]                             INT             NULL,
   [Interfase]                      INT             NULL,
   [PNI]                            INT             NULL,
   [Tolerancia]                     INT             NULL,
   [Inventario]                     INT             NULL,
   [ExecutionId]                    NVARCHAR(250)   NOT NULL,
   [LastLoadedDate]                 DATETIME        NULL    DEFAULT Admin.udf_GetTrueDate(),

   --Indexes
   INDEX NIX_NodeProductInfo_CatCatEleNode NONCLUSTERED (Category,Element,NodeName)
);
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeProductInfo',
    @level2type = N'COLUMN',
    @level2name = N'ProductName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of product from storage location product ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeProductInfo',
    @level2type = N'COLUMN',
    @level2name = N'ProductOrder'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The rule name from node product rule',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeProductInfo',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipStrategy'
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the owner (Category element of owner category)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeProductInfo',
    @level2type = N'COLUMN',
    @level2name = N'OwnerName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The ownership percentage of storage location product owner',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeProductInfo',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The uncertaintity node product of storage location product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeProductInfo',
    @level2type = N'COLUMN',
    @level2name = N'UncertaintyNodeProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the category',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeProductInfo',
    @level2type = N'COLUMN',
    @level2name = N'Category'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeProductInfo',
    @level2type = N'COLUMN',
    @level2name = N'Element'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeProductInfo',
    @level2type = N'COLUMN',
    @level2name = N'NodeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeProductInfo',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'1 if SUM(PI) > 1, otherwise SUM(PI)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeProductInfo',
    @level2type = N'COLUMN',
    @level2name = N'PI'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'1 if SUM(Interfase) > 1, otherwise SUM(Interfase)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeProductInfo',
    @level2type = N'COLUMN',
    @level2name = N'Interfase'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'1 if SUM(PNI) > 1, otherwise SUM(PNI)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeProductInfo',
    @level2type = N'COLUMN',
    @level2name = N'PNI'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'1 if SUM(Tolerancia) > 1, otherwise SUM(Tolerancia)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeProductInfo',
    @level2type = N'COLUMN',
    @level2name = N'Tolerancia'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'1 if SUM(Inventorio) > 1, otherwise SUM(Inventorio)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeProductInfo',
    @level2type = N'COLUMN',
    @level2name = N'Inventario'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The execution id unique to session received from UI',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeProductInfo',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The date when this row was inserted',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeProductInfo',
    @level2type = N'COLUMN',
    @level2name = N'LastLoadedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table is used to fetch the data for Products related to the Nodes, to be displayed in the Node Configuration Report.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeProductInfo',
    @level2type = NULL,
    @level2name = NULL