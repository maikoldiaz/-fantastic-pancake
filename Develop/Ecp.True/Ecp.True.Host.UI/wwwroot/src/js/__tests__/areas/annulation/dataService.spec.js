import { dataService } from '../../../modules/administration/annulation/dataService';
import { constants } from '../../../common/services/constants';
import { updateTypes, resetTypes } from '../../../modules/administration/annulation/actions';

describe('action provider tests',
    () => {
        it('should update for source section change with not null value',
            () => {
                const sectionName = 'source.movement';
                const value = { elementId: 1 };
                const actions = [ updateTypes(constants.Annulations.Sections.Annulation, value, constants.Annulations.Fields.Movement) ];

                expect(dataService.getAnnulationActions(sectionName, value)).toEqual(actions);
            });
        it('should reset for source section change with null value',
            () => {
                const sectionName = 'source.movement';
                const value = null;
                const actions = [ resetTypes(constants.Annulations.Sections.Annulation, constants.Annulations.Fields.Movement) ];

                expect(dataService.getAnnulationActions(sectionName, value)).toEqual(actions);
            });

        it('should update for annulation section change with not null value',
            () => {
                const sectionName = 'annulation.movement';
                const value = { elementId: 1 };
                const actions = [ updateTypes(constants.Annulations.Sections.Source, value, constants.Annulations.Fields.Movement) ];

                expect(dataService.getAnnulationActions(sectionName, value)).toEqual(actions);
            });
        it('should reset for annulation section change with null value',
            () => {
                const sectionName = 'annulation.movement';
                const value = null;
                const actions = [ resetTypes(constants.Annulations.Sections.Source, constants.Annulations.Fields.Movement) ];

                expect(dataService.getAnnulationActions(sectionName, value)).toEqual(actions);
            });
        it('should give set of labels for source section',
            () => {
                const sectionName = constants.Annulations.Sections.Source;
                const labels = ['movementTypeTransferOperational', 'originLbl', 'sourceProductTransferOperational'];
                expect(dataService.getLabels(sectionName)).toEqual(labels);
            });

        it('should give set of labels for annulation section',
            () => {
                const sectionName = constants.Annulations.Sections.Annulation;
                const labels = ['cancellationType', 'destinationLbl', 'destinationProductTransferPoints'];
                expect(dataService.getLabels(sectionName)).toEqual(labels);
            });

        it('should build annulation object during create',
            () => {
                const values = {
                    source: {
                        movement: { elementId: 1 },
                        product: { originTypeId: 1 },
                        node: { originTypeId: 1 }
                    },
                    annulation: {
                        movement: { elementId: 1 },
                        product: { originTypeId: 1 },
                        node: { originTypeId: 1 }
                    },
                    isActive: true
                };
                const mode = constants.Modes.Create;
                const initialValues = { annulationId: 1, rowVersion: 'test' };
                const outputObject = {
                    sourceMovementTypeId: values.source.movement.elementId,
                    annulationMovementTypeId: values.annulation.movement.elementId,
                    sourceNodeId: values.source.node.originTypeId,
                    destinationNodeId: values.annulation.node.originTypeId,
                    sourceProductId: values.source.product.originTypeId,
                    destinationProductId: values.annulation.product.originTypeId,
                    isActive: values.isActive,
                    annulationId: mode === constants.Modes.Update ? initialValues.annulationId : 0,
                    rowVersion: mode === constants.Modes.Update ? initialValues.rowVersion : null
                };
                expect(dataService.buildAnnulationObject(values, mode, initialValues)).toEqual(outputObject);
            });

        it('should build annulation object during update',
            () => {
                const values = {
                    source: {
                        movement: { elementId: 1 },
                        product: { originTypeId: 1 },
                        node: { originTypeId: 1 }
                    },
                    annulation: {
                        movement: { elementId: 1 },
                        product: { originTypeId: 1 },
                        node: { originTypeId: 1 }
                    },
                    isActive: true
                };
                const mode = constants.Modes.Update;
                const initialValues = { annulationId: 1, rowVersion: 'test' };
                const outputObject = {
                    sourceMovementTypeId: values.source.movement.elementId,
                    annulationMovementTypeId: values.annulation.movement.elementId,
                    sourceNodeId: values.source.node.originTypeId,
                    destinationNodeId: values.annulation.node.originTypeId,
                    sourceProductId: values.source.product.originTypeId,
                    destinationProductId: values.annulation.product.originTypeId,
                    isActive: values.isActive,
                    annulationId: mode === constants.Modes.Update ? initialValues.annulationId : 0,
                    rowVersion: mode === constants.Modes.Update ? initialValues.rowVersion : null
                };
                expect(dataService.buildAnnulationObject(values, mode, initialValues)).toEqual(outputObject);
            });

        it('should disable source field during update',
            () => {
                const mode = constants.Modes.Update;
                const name = constants.Annulations.Sections.Source;
                expect(dataService.disableField(mode, name)).toEqual(true);
            });

        it('should build initial values object',
            () => {
                const row = {
                    sourceCategoryElement: { elementId: 1, name: 'test' },
                    annulationCategoryElement: { elementId: 2, name: 'test' },
                    sourceProductOriginType: { originTypeId: 1, name: 'test' },
                    destinationProductOriginType: { originTypeId: 1, name: 'test' },
                    sourceNodeOriginType: { originTypeId: 1, name: 'test' },
                    destinationNodeOriginType: { originTypeId: 1, name: 'test' },
                    annulationId: 1,
                    rowVersion: 'test',
                    isActive: true
                };

                const outputObject = {
                    source: {
                        movement: { elementId: 1, name: 'test' },
                        product: { originTypeId: 1, name: 'test' },
                        node: { originTypeId: 1, name: 'test' }
                    },
                    annulation: {
                        movement: { elementId: 2, name: 'test' },
                        product: { originTypeId: 1, name: 'test' },
                        node: { originTypeId: 1, name: 'test' }
                    },
                    annulationId: row.annulationId,
                    isActive: row.isActive,
                    rowVersion: row.rowVersion
                };

                expect(dataService.buildInitialValues(row)).toEqual(outputObject);
            });
    });

