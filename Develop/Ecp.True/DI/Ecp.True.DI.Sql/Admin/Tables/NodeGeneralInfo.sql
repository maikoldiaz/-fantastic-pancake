/*==============================================================================================================================
--Author:        Microsoft
--Created Date : May-14-2020
--<Description>: This table is used to fetch the general Node information, to be displayed in the Node Configuration Report. </Description>
================================================================================================================================*/

CREATE TABLE [Admin].NodeGeneralInfo
(
    --Columns
   [NodeName]                        NVARCHAR(150)   NULL,
   [NodeOrder]                       NVARCHAR(50)    NULL,
   [NodeId]                          INT             NULL,
   [NodeOwnershipStrategy]           NVARCHAR(100)   NULL,
   [NodeControlLimit]                NVARCHAR(100)	 NULL,
   [NodeAcceptableBalancePercentage] NVARCHAR(100)   NULL,
   [Category]                        NVARCHAR(150)   NOT NULL,
   [Element]                         NVARCHAR(150)   NOT NULL,
   [ExecutionId]                     NVARCHAR(250)   NOT NULL,
   [LastLoadedDate]                  DATETIME        NULL    DEFAULT Admin.udf_GetTrueDate(),

   --Indexes
   INDEX NIX_NodeGeneralInfo_CatCatEleNode NONCLUSTERED (Category,Element,NodeName)
);
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeGeneralInfo',
    @level2type = N'COLUMN',
    @level2name = N'NodeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The order of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeGeneralInfo',
    @level2type = N'COLUMN',
    @level2name = N'NodeOrder'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeGeneralInfo',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of Estrategia de propiedad for nodes',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeGeneralInfo',
    @level2type = N'COLUMN',
    @level2name = N'NodeOwnershipStrategy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The control limit of node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeGeneralInfo',
    @level2type = N'COLUMN',
    @level2name = N'NodeControlLimit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The acceptable balance percentage of node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeGeneralInfo',
    @level2type = N'COLUMN',
    @level2name = N'NodeAcceptableBalancePercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the category',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeGeneralInfo',
    @level2type = N'COLUMN',
    @level2name = N'Category'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeGeneralInfo',
    @level2type = N'COLUMN',
    @level2name = N'Element'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The execution id unique to session received from UI',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeGeneralInfo',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The date when this row was inserted',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeGeneralInfo',
    @level2type = N'COLUMN',
    @level2name = N'LastLoadedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table is used to fetch the general Node information, to be displayed in the Node Configuration Report.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeGeneralInfo',
    @level2type = NULL,
    @level2name = NULL