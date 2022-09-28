const constants = (function () {
    return {
        Types: {
            GUID: 'guid',
            DATETIME: 'datetime',
            DATE: 'date'
        },
        Config_Constants: {
            PROD: 'production',
            DEV: 'development'
        },
        ResponsiveBreakpoints: {
            DESKTOPMIN: 1367,
            MOBILEMIN: 360,
            MOBILEMAX: 767,
            TABLETMIN: 768,
            TABLETMAX: 1366
        },
        Device: {
            Desktop: 'Desktop',
            Mobile: 'Mobile',
            Tablet: 'Tablet',
            TabletPortrait: 'TabletPortrait',
            TabletLandscape: 'TabletLandscape'
        },
        RouterActions: {
            Type: {
                Button: 'Button',
                Dropdown: 'Dropdown',
                Link: 'Link',
                Component: 'Component'
            }
        },
        NotificationType: {
            Success: 'success',
            Error: 'error',
            Warning: 'warning',
            Info: 'info'
        },
        SyncStatus:
        {
            Info: 'info',
            Failed: 'error',
            Success: 'success',
            NotReady: 'warning'
        },
        Modes: {
            Create: 'create',
            Read: 'read',
            Update: 'update',
            Delete: 'delete'
        },
        Roles: {
            Administrator: 'Administrator',
            ProfessionalSegmentBalances: 'ProfessionalSegmentBalances',
            Query: 'Query',
            Approver: 'Approver',
            Programmer: 'Programmer',
            Auditor: 'Auditor',
            Chain: 'Chain',
            Anonymous: '*'
        },
        ChartColors: {
            a: '#74B44A',
            b: '#F7DF00',
            c: '#656565',
            d: '#E0F0FA',
            e: '#5995AF',
            f: '#1592E6',
            g: '#93BCBB',
            h: '#C3D2CB',
            i: '#00CB90',
            j: '#CCD325',
            k: '#F6B41A',
            l: '#C55A11',
            m: '#7030A0',
            n: '#3B3838',
            o: '#7A8B58',
            p: '#93A7BE',
            q: '#5F6867',
            r: '#003427'
        },
        Category: {
            NodeType: 1,
            Segment: 2,
            Operator: 3,
            UnitMeasurement: 6,
            Owner: 7,
            System: 8,
            MovementType: 9,
            OwnershipRules: 10,
            ProductType: 11,
            Nodes: 13,
            Products: 14,
            StorageLocations: 15,
            ReasonForChange: 16,
            TransferPoint: 18,
            CostCenter: 24,
            SapTransactionCode: 25
        },
        StatusType: {
            PROCESSED: 'Propiedad',
            PROCESSING: 'Procesando',
            FAILED: 'Fallido',
            FINALIZED: 'Finalizado',
            DELTA: 'Deltas',
            SENT: 'Enviado',
            VISUALIZATION: 'Visualizacion',
            ERROR: 'Error',
            CONCILIATIONFAILED: 'Fallo conciliación'
        },
        Status: {
            Processed: 'PROCESSED',
            Processing: 'PROCESSING',
            Failed: 'FAILED'
        },
        VariableType: {
            Interface: 1,
            Tolerance: 2,
            UnidentifiedLoss: 3,
            InitialInventory: 4,
            Input: 5,
            Output: 6,
            IdentifiedLoss: 7,
            FinalInventory: 8
        },
        OwnershipNodeStatusType: {
            OWNERSHIP: 'Propiedad',
            SENT: 'Enviado',
            PROCESSING: 'Procesando',
            FAILED: 'Fallido',
            LOCKED: 'Bloqueado',
            UNLOCKED: 'Desbloqueado',
            PUBLISHING: 'Publicando',
            PUBLISHED: 'Publicado',
            SUBMITFORAPPROVAL: 'Enviado a aprobación',
            APPROVED: 'Aprobado',
            REJECTED: 'Rechazado',
            REOPENED: 'Reabierto',
            DELTA: 'Deltas',
            CONCILIATED: 'Conciliado',
            NOTCONCILIATED: 'No Conciliado',
            CONCILIATIONFAILED: 'Fallo Conciliación'
        },
        OwnershipNodeStatus: {
            SENT: 'SENT',
            Processing: 'PROCESSING',
            OWNERSHIP: 'OWNERSHIP',
            FAILED: 'FAILED',
            LOCKED: 'LOCKED',
            UNLOCKED: 'UNLOCKED',
            PUBLISHING: 'PUBLISHING',
            PUBLISHED: 'PUBLISHED',
            SUBMITFORAPPROVAL: 'SUBMITFORAPPROVAL',
            APPROVED: 'APPROVED',
            REJECTED: 'REJECTED',
            REOPENED: 'REOPENED',
            RECONCILED: 'RECONCILED',
            NOTRECONCILED: 'NOTRECONCILED',
            CONCILIATIONFAILED: 'CONCILIATIONFAILED'
        },
        TicketType: {
            Cutoff: 1,
            Ownership: 2,
            Logistics: 3,
            Delta: 4,
            OfficialDelta: 5,
            OfficialLogistics: 6,
            LogisticMovements: 7
        },
        LogisticsType: {
            3: 'operational',
            6: 'official'
        },
        CalculationType: {
            Cutoff: 'Cutoff',
            Ownership: 'Ownership',
            Delta: 'Delta',
            OfficialDelta: 'OfficialDelta',
            OfficialLogistics: 'OfficialLogistics',
            LogisticMovements: 'LogisticMovements'
        },
        Errors: {
            Forbidden: 401,
            NoAccess: 403,
            NotFound: 404,
            ServerError: 500
        },
        Validations: {
            ChainWithInitialNodes: 'chainWithInitialNodes',
            ChainWithFinalNodes: 'chainWithFinalNodes',
            NodesWithOwnershipRules: 'nodesWithOwnershipRules',
            NodeOwnershipCouldBeFound: 'nodeOwnershipCouldBeFound',
            ChainWithAnalyticalOperationalInfo: 'chainWithAnaliticalOperationalInfo'
        },
        SystemType: {
            TRUE: 1,
            SINOPER: 2,
            EXCEL: 3,
            CONTRACT: 4,
            EVENTS: 5
        },
        ProcessType: {
            Movement: 'Movement',
            Inventory: 'Inventory',
            Events: 'Events',
            Contract: 'Contract'
        },
        CommentType: {
            Cutoff: 1,
            Error: 2
        },
        EventType: {
            Insert: 'Insert',
            Update: 'Update',
            Delete: 'Delete'
        },
        Report: {
            WithOwner: '10.10.17BalanceOperativoConPropiedadPorNodo17',
            WithoutOwner: '10.10.01BalanceOperativoSinPropiedad01',
            AnalyticsReport: '10.10.03EvaluacionModelosAnaliticosPorcentajePropiedad03',
            WithoutCutoff: '10.10.04BalanceOperativo04',
            BalanceControlChart: '10.10.05CartadeControl05',
            EventContractReport: '10.10.06ComprasyVentas06',
            NodeStatusReport: '10.10.07AprobacionesBalanceConPropiedadPorNodo07',
            NodeConfigurationReport: '10.10.08ConfiguracionDetalladaPorNodo08',
            MovementAuditReport: '10.10.10AuditoriaDeMovimientos10',
            InventoryAuditReport: '10.10.11AuditoriaDeInventarios11',
            SettingsAuditReport: '10.10.09AuditoriaDeConfiguraciones09',
            OfficialBalancePerNodeReport: '10.10.12BalanceOficialPorNodo12',
            OfficialInitialBalanceReport: '10.10.13BalanceOficialInicialCargado13',
            OfficialPendingBalanceReport: '10.10.15PendientesOficialesDePeriodosAnteriores15',
            OfficialCustodyTransferPointsReport: '10.10.14PuntosTransferenciaCustodia14',
            BalanceOperativeWithPropertyReport: '10.10.02BalanceOperativoConPropiedad02',
            NonSonWithOwnerReport: '10.10.16DeOtrosSegmentosConPropiedad16',
            SendToSapStatesReport: '10.10.18EstadosEnvioSap18',
            OfficialNodeStatusReport: '10.10.20NodeOficialStatus20',
            UserGroupAssignmentReport: '10.10.21UserRole21',
            UserGroupAccessReport: '10.10.19RoleConMenus19',
            UserGroupAndAssignedUserAccessReport: '10.10.22UserRoleMenu22'
        },
        Element: {
            ElementCreatedSuccessful: 'Elemento creado con éxito',
            ElementUpdatedSuccessful: 'Elemento modificado con éxito'
        },
        PowerAutomate: {
            Flows: 'flows',
            Approvals: 'approvalCenter'
        },
        DetailsPageType: {
            Grid: 'P',
            Details: 'D'
        },
        NodeConnectionState: {
            Unsaved: 'Unsaved',
            Active: 'Active',
            Inactive: 'Inactive',
            TransferPoint: 'TransferPoint',
            Invalid: 'Invalid'
        },
        NodeRelationship: {
            Logistics: 'logistics',
            Operative: 'operatives'
        },
        DecimalRange: {
            Min: 0.01,
            Max: 9999999999999999.99,
            MaxIntValue: 2147483647,
            ThousandSeparator: '.',
            DecimalSeparator: ',',
            Step: 0.01,
            greaterThanOrEqualTo: 0.01,
            MinNegative: -9999999999999999.99,
            defaultMin: 0
        },
        PercentageRange: {
            Min: 1,
            Max: 100,
            Step: 0.01,
            greaterThanOrEqualTo: 0.01
        },
        RuleType: {
            Node: 'node',
            NodeProduct: 'storageLocationProduct',
            NodeConnectionProduct: 'nodeConnectionProduct',
            ConnectionProduct: 'connectionProduct',
            NodeAttribute: 'NodeAttribute',
            StorageLocationProductVariable: 'storageLocationProductVariable'
        },
        GridNames: {
            Node: 'nodeOwnershipRules',
            NodeProduct: 'nodeProductRules',
            NodeConnectionProduct: 'nodeConnectionProductRules',
            ConnectionProduct: 'connectionProducts',
            NodeAttributeProduct: 'nodeProducts',
            NodeAttribute: 'nodeAttributes'
        },
        Timeouts: {
            Graphical: 300,
            Tickets: 30,
            FileUploads: 10,
            OwnershipNodes: 10,
            Synchronizer: 10000,
            TooltipDelay: 0.8,
            Reports: 10,
            Sap: 30
        },
        ForgeryTokenName: 'X-XSRF-TOKEN',
        DefaultColorCode: '#E0F0FA',
        MovementType: {
            Purchase: 49,
            Sale: 50,
            Compra: 'Compra',
            Venta: 'Venta',
            AceSalida: 'ACE Salida',
            AceEntrada: 'ACE Entrada'
        },
        DashedGridCell: '--',
        DateFormat: {
            FullDate: 'YYYY-MM-DDThh:mi:ss',
            ShortDate: 'DD-MMM-YY',
            LongDate: 'YYYY-MM-DD HH:mm:ss'
        },
        NodeSection: {
            In: 'in_',
            Out: 'out_'
        },
        Prefix: '+-',
        Suffix: '%',
        InputType: {
            Decimal: 'Decimal',
            Percentage: 'Percentage'
        },
        Annulations: {
            Fields: {
                Movement: 'movement',
                Node: 'node',
                Product: 'product'
            },
            Sections: {
                Source: 'source',
                Annulation: 'annulation'
            }
        },
        OriginTypes: {
            Origin: 1,
            Destination: 2,
            None: 3
        },
        DecimalPlaces: 2,
        InventoriesValidations: {
            InitialInventories: 1,
            NewNodes: 2
        },
        OwnershipMinPrecentage: 0.01,
        TicketTypeName: {
            Delta: 'Delta',
            OfficialDelta: 'OfficialDelta'
        },
        ReportType: {
            BeforeCutOff: 'cutOffReport',
            OfficialNodeBalance: 'officialBalancePerNode',
            OfficialInitialBalance: 'officialBalanceLoaded',
            OperativeBalance: 'operativeBalanceReport',
            SapBalance: 'sentToSapReport',
            officialNodeStatusReport: 'officialNodeStatusReport',
            UserRolesAndPermissions: 'userRolesAndPermissions'
        },
        ReportTypeName: {
            BeforeCutOff: 'BeforeCutOff',
            OfficialNodeBalance: 'OfficialNodeBalance',
            OfficialInitialBalance: 'OfficialInitialBalance',
            OperativeBalance: 'OperativeBalance',
            SapBalance: 'SapBalance',
            OfficialNodeStatusReport: 'officialNodeStatusReport',
            UserRolesAndPermissions: 'UserRolesAndPermissions',
            UserGroupAssignmentReport: 'UserGroupAssignmentReport',
            UserGroupAccessReport: 'UserGroupAccessReport',
            UserGroupAndAssignedUserAccessReport: 'UserGroupAndAssignedUserAccessReport'
        },
        BlockChainPageType: {
            1: 'Movimiento',
            2: 'Propietario de movimiento',
            3: 'Inventario',
            4: 'Propietario de inventario',
            5: 'Propiedad de movimiento',
            6: 'Propiedad de inventario',
            7: 'Desbalance',
            8: 'Nodo',
            9: 'Conexión entre nodos'
        },
        ScenarioType: {
            Operational: 'Operativo',
            Official: 'Oficial',
            Consolidated: 'Consolidated'
        },
        ScenarioTypeId: {
            operational: 1,
            official: 2,
            Consolidated: 3
        },
        ReopenDeltaNodeActionType: {
            DeltaNode: 1,
            DeltaNodeAndSuccessor: 2
        },
        OperationalCutOff: {
            TransferPoint: { DefaultComment: 'Oficializado por System' }
        },
        OfficialNodeBalance: 'OfficialNodeBalance',
        ReportStatusAvailable: 0,
        Todos: 'Todos',
        Footer: {
            Section: 'section',
            Modal: 'modal',
            Flyout: 'flyout'
        },
        FieldValidation: {
            Category: '^[A-Za-z0-9ÑÁÉÍÓÚñáéíóúü :_-]+$',
            Node: '^["A-Za-z0-9ÑÁÉÍÓÚñáéíóúü :_-]+$',
            StorageLocation: '^["A-Za-z0-9ÑÁÉÍÓÚñáéíóúü :_-]+$'
        },
        IntegrationType: {
            1: 'REQUEST',
            2: 'RESPONSE'
        },
        InfoCreationStatus: {
            Created: 'Created',
            Duplicated: 'Duplicated',
            Updated: 'Updated',
            Error: 'Error'
        }
    };
}());

export { constants };
