import { updateTypes, resetTypes } from './actions';
import { constants } from '../../../common/services/constants';
import { utilities } from '../../../common/services/utilities';

const dataService = (function () {
    const getOppositeSectionName = sectionName => {
        return utilities.equalsIgnoreCase(sectionName, constants.Annulations.Sections.Source) ?
            constants.Annulations.Sections.Annulation : constants.Annulations.Sections.Source;
    };
    return {
        getAnnulationActions: (sectionName, value) => {
            const sectionNameParts = sectionName.split('.');
            if (!utilities.isNullOrUndefined(value)) {
                return [updateTypes(getOppositeSectionName(sectionNameParts[0]), value, sectionNameParts[1])];
            }
            return [resetTypes(getOppositeSectionName(sectionNameParts[0]), sectionNameParts[1])];
        },
        getLabels: sectionName => {
            if (sectionName === constants.Annulations.Sections.Source) {
                return ['movementTypeTransferOperational', 'originLbl', 'sourceProductTransferOperational'];
            }
            return ['cancellationType', 'destinationLbl', 'destinationProductTransferPoints'];
        },
        buildAnnulationObject: (values, mode, initialValues) => {
            return {
                sourceMovementTypeId: values.source.movement.elementId,
                annulationMovementTypeId: values.annulation.movement.elementId,
                sourceNodeId: values.source.node.originTypeId,
                destinationNodeId: values.annulation.node.originTypeId,
                sourceProductId: values.source.product.originTypeId,
                destinationProductId: values.annulation.product.originTypeId,
                sapTransactionCodeId: values.sapTransactionCode ? values.sapTransactionCode.elementId : undefined,
                isActive: values.isActive,
                annulationId: mode === constants.Modes.Update ? initialValues.annulationId : 0,
                rowVersion: mode === constants.Modes.Update ? initialValues.rowVersion : null
            };
        },
        disableField: (mode, name) => {
            return (mode === constants.Modes.Update && name === constants.Annulations.Sections.Source);
        },
        buildInitialValues: row => {
            const result = {
                source: {
                    movement: { elementId: row.sourceCategoryElement.elementId, name: row.sourceCategoryElement.name },
                    product: { originTypeId: row.sourceProductOriginType.originTypeId, name: row.sourceProductOriginType.name },
                    node: { originTypeId: row.sourceNodeOriginType.originTypeId, name: row.sourceNodeOriginType.name }
                },
                annulation: {
                    movement: { elementId: row.annulationCategoryElement.elementId, name: row.annulationCategoryElement.name },
                    product: { originTypeId: row.destinationProductOriginType.originTypeId, name: row.destinationProductOriginType.name },
                    node: { originTypeId: row.destinationNodeOriginType.originTypeId, name: row.destinationNodeOriginType.name }
                },
                annulationId: row.annulationId,
                isActive: row.isActive,
                rowVersion: row.rowVersion
            };

            if (row.sapTransactionCode) {
                result.sapTransactionCode = { elementId: row.sapTransactionCode.elementId, name: row.sapTransactionCode.name };
            }

            return result;
        }
    };
}());

export { dataService };
