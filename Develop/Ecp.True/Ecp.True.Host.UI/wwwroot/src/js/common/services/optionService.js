// cSpell:ignore aprobación
// cSpell:ignore conciliación
import { constants } from './constants';
import { resourceProvider } from './resourceProvider';

const optionService = (function () {
    const options = {
        actionTypes: [{ label: 'insert', value: 'Insertar' }, { label: 'update', value: 'Actualizar' }, { label: 'delete', value: 'Eliminar' }],
        excelActionTypes: [{ label: 'insert', value: 'Insertar' }, { label: 'update', value: 'Actualizar' }, { label: 'delete', value: 'Eliminar' }, { label: 'reInject', value: 'Re Inyectar' }],
        statusTypes: [{ label: 'finalized', value: 'Finalizado' }, { label: 'processing', value: 'Procesando' }],
        cutoffStateTypes: [{ label: 'finalized', value: 'Finalizado' }, { label: 'processing', value: 'Procesando' }, { label: 'failed', value: 'Fallido' }],
        ownershipStateTypes: [
            { label: 'finalized', value: 'Finalizado' }, { label: 'processing', value: 'Procesando' }, { label: 'failed', value: 'Fallido' },
            { label: 'conciliationfailed', value: 'Fallo conciliación' }],
        logisticsStateTypes: [
            { label: 'finalized', value: 'Finalizado' }, { label: 'processing', value: 'Procesando' },
            { label: 'failed', value: 'Fallido' }, { label: 'error', value: 'Error' }],
        logisticsMovementStateTypes: [
            { label: 'finalized', value: 'Finalizado' },
            { label: 'sent', value: 'Enviado' },
            { label: 'failed', value: 'Fallido' },
            { label: 'cancelled', value: 'Cancelado' },
            { label: 'forwarded', value: 'Reenviado' }],
        ownershipNodeStateTypes: [
            { label: 'ownershipoption', value: 'Propiedad' }, { label: 'processing', value: 'Procesando' }, { label: 'failed', value: 'Fallido' },
            { label: 'locked', value: 'Bloqueado' }, { label: 'unlocked', value: 'Desbloqueado' }, { label: 'publishing', value: 'Publicando' },
            { label: 'published', value: 'Publicado' }, { label: 'submitforapproval', value: 'Enviado a aprobación' }, { label: 'approved', value: 'Aprobado' },
            { label: 'rejected', value: 'Rechazado' }, { label: 'reopened', value: 'Reabierto' },
            { label: 'notconciliated', value: 'No Conciliado' }, { label: 'conciliated', value: 'Conciliado' }, { label: 'conciliationfailed', value: 'Fallo Conciliación' }],
        fileTypes: [],
        reportTypes: [{ label: 'operationalReport', value: constants.Report.WithoutCutoff },
            { label: 'withOperationalCutoff', value: constants.Report.WithoutOwner },
            { label: 'withOwnership', value: constants.Report.WithOwner }],
        operativeBalanceReportTypes: [{ label: 'operationalReport', value: constants.Report.WithoutCutoff },
            { label: 'withOwnership', value: constants.Report.WithOwner }],
        transactionsAuditReportTypes: [
            { label: 'movements', value: constants.Report.MovementAuditReport },
            { label: 'inventories', value: constants.Report.InventoryAuditReport }
        ],
        userRolesAndPermissionsReportTypes: [
            { label: 'userGroupAssignment', value: constants.ReportTypeName.UserGroupAssignmentReport },
            { label: 'userGroupAccess', value: constants.ReportTypeName.UserGroupAccessReport },
            { label: 'userGroupAndAssignedUserAccess', value: constants.ReportTypeName.UserGroupAndAssignedUserAccessReport }
        ],
        gridStatusTypes: [{ label: 'active', value: true }, { label: 'inActive', value: false }],
        DeltaStateTypes: [{ label: 'processing', value: 'Procesando' }, { label: 'failed', value: 'Fallido' }, { label: 'finalized', value: 'Finalizado' }],
        officialDeltaStateTypes: [{ label: 'processing', value: 'Procesando' }, { label: 'failed', value: 'Fallido' }, { label: 'delta', value: 'Deltas' }],
        officialDeltaPerNodeStateTypes: [{ label: 'processing', value: 'Procesando' }, { label: 'failed', value: 'Fallido' },
            { label: 'delta', value: 'Deltas' }, { label: 'submitforapproval', value: 'Enviado a aprobación' }, { label: 'approved', value: 'Aprobado' },
            { label: 'rejected', value: 'Rechazado' }, { label: 'reopened', value: 'Reabierto' }],
        OfficialLogisticsStateTypes: [
            { label: 'processing', value: 'Procesando' },
            { label: 'error', value: 'Error' },
            { label: 'finalized', value: 'Finalizado' }
        ],
        executionReportStatusTypes: [{ label: 'finalized', value: 'Finalizado' }, { label: 'processing', value: 'Procesando' }, { label: 'failed', value: 'Fallido' }],
        logicticMovementsStateTypes: [
            { label: 'processing', value: 'Procesando' },
            { label: 'visualization', value: 'Visualizacion' },
            { label: 'error', value: 'Error' },
            { label: 'sent', value: 'Enviado' },
            { label: 'finalized', value: 'Finalizado' },
            { label: 'failed', value: 'Fallido' }
        ],
        IntegrationType: () => {
            const arr = [];
            Object.entries(constants.IntegrationType).forEach(
                ([, value]) => arr.push({ label: resourceProvider.read(value.toLowerCase()), value: value })
            );
            return arr;
        },
        SourceTypeBySapAndFico: [
            { label: 'PURCHASE', value: 'PURCHASE' },
            { label: 'SELL', value: 'SELL' },
            { label: 'MOVEMENTS', value: 'MOVEMENTS' },
            { label: 'INVENTORY', value: 'INVENTORY' },
            { label: 'LOGISTIC', value: 'LOGISTIC' },
            { label: 'OWNERSHIP', value: 'OWNERSHIP' },
            { label: 'OPERATIVEDELTA', value: 'OPERATIVEDELTA' },
            { label: 'OFFICIALDELTA', value: 'OFFICIALDELTA' }
        ]
    };
    return {
        getActionTypes: function () {
            return options.actionTypes;
        },
        getExcelActionTypes: function () {
            return options.excelActionTypes;
        },
        getStatusTypes: function () {
            return options.statusTypes;
        },
        getFileTypes: function () {
            return options.fileTypes;
        },
        setFileTypes: function (type, values) {
            const systemTypeMapping = {};
            Object.keys(constants.SystemType).forEach(x => {
                systemTypeMapping[constants.SystemType[x]] = x;
            });
            options[type] = values.map(x => {
                return { label: x.name, value: systemTypeMapping[x.systemTypeId] };
            });
        },
        getCutoffStateTypes: function () {
            return options.cutoffStateTypes;
        },
        getOwnershipStateTypes: function () {
            return options.ownershipStateTypes;
        },
        getLogisticsStateTypes: function () {
            return options.logisticsStateTypes;
        },
        getDeltaStateTypes: function () {
            return options.DeltaStateTypes;
        },
        getLogisticsMovementStateTypes: function () {
            return options.logisticsMovementStateTypes;
        },
        getOfficialDeltaStateTypes: function () {
            return options.officialDeltaStateTypes;
        },
        officialDeltaPerNodeStateTypes: function () {
            return options.officialDeltaPerNodeStateTypes;
        },
        getOwnershipNodeStateTypes: function () {
            return options.ownershipNodeStateTypes;
        },
        getReportTypes: function () {
            return options.reportTypes;
        },
        getOperativeBalanceReportTypes: function () {
            return options.operativeBalanceReportTypes;
        },
        getOriginTypes: function (values) {
            let originTypes = [];
            originTypes = values.map(x => {
                return { label: x.name, value: x.name };
            });
            return originTypes;
        },
        getTransactionsAuditReportTypes: function () {
            return options.transactionsAuditReportTypes;
        },
        getUserRolesAndPermissionsReportTypes: function () {
            return options.userRolesAndPermissionsReportTypes;
        },
        getGridStatusTypes: function () {
            return options.gridStatusTypes;
        },
        getBlockchainTypes: function () {
            return Object.keys(constants.BlockChainPageType).map(a => {
                return { label: constants.BlockChainPageType[a], value: Number(a) };
            });
        },
        getOfficialLogisticsStateTypes: function () {
            return options.OfficialLogisticsStateTypes;
        },
        getExecutionStatusTypes: function () {
            return options.executionReportStatusTypes;
        },
        getLogisticMovementsStateTypes: function () {
            return options.logicticMovementsStateTypes;
        },
        getIntegrationType: function () {
            return options.IntegrationType();
        },
        getSourceTypeBySapAndFico: function () {
            const arr = [];
            options.SourceTypeBySapAndFico.forEach(
                ({ label, value }) => arr.push({ label: resourceProvider.read(label.toLowerCase()), value: value })
            );
            return arr;
        }
    };
}());

export { optionService };
