import { actionProvider } from '../../../modules/administration/transferPoints/actionProvider';
import { change } from 'redux-form';
import {
    resetOnDestinationNodeChange,
    getNodeStorageLocations,
    getTransferProducts,
    getLogisticCenter,
    resetOnSourceNodeChange,
    getTransferDestinationNodes,
    setStorageLocation
} from '../../../modules/administration/transferPoints/logistics/actions';

describe('action provider tests',
    () => {
        it('should give set of actions on null destination node change',
            () => {
                const actions = [
                    change('createTransferPointLogistics', 'destinationProduct', null),
                    change('createTransferPointLogistics', 'destinationStorageLocation', null),
                    resetOnDestinationNodeChange()
                ];
                expect(actionProvider.getlogisticsActions('destinationNode', null)).toEqual(actions);
            });

        it('should give set of actions on not null destination node change',
            () => {
                const value = { destinationNode: { nodeId: 1, name: 'name' } };
                const actions = [
                    change('createTransferPointLogistics', 'destinationProduct', null),
                    change('createTransferPointLogistics', 'destinationStorageLocation', null),
                    resetOnDestinationNodeChange(),
                    getNodeStorageLocations(value.destinationNode.nodeId, false),
                    getTransferProducts(value.destinationNode.nodeId, false),
                    getLogisticCenter(value.destinationNode.name, false)
                ];
                expect(JSON.stringify(actionProvider.getlogisticsActions('destinationNode', value))).toEqual(JSON.stringify(actions));
            });

        it('should give set of actions on null source node change',
            () => {
                const actions = [
                    change('createTransferPointLogistics', 'destinationNode', null),
                    change('createTransferPointLogistics', 'sourceProduct', null),
                    change('createTransferPointLogistics', 'sourceStorageLocation', null),
                    change('createTransferPointLogistics', 'destinationProduct', null),
                    change('createTransferPointLogistics', 'destinationStorageLocation', null),
                    resetOnSourceNodeChange()
                ];
                expect(actionProvider.getlogisticsActions('sourceNode', null)).toEqual(actions);
            });

        it('should give set of actions on not null source node change',
            () => {
                const value = { sourceNodeId: 1, sourceNode: { name: 'name' } };
                const actions = [
                    change('createTransferPointLogistics', 'destinationNode', null),
                    change('createTransferPointLogistics', 'sourceProduct', null),
                    change('createTransferPointLogistics', 'sourceStorageLocation', null),
                    change('createTransferPointLogistics', 'destinationProduct', null),
                    change('createTransferPointLogistics', 'destinationStorageLocation', null),
                    resetOnSourceNodeChange(),
                    getTransferDestinationNodes(value.sourceNodeId),
                    getNodeStorageLocations(value.sourceNodeId, true),
                    getTransferProducts(value.sourceNodeId, true),
                    getLogisticCenter(value.sourceNode.name, true)
                ];
                expect(JSON.stringify(actionProvider.getlogisticsActions('sourceNode', value))).toEqual(JSON.stringify(actions));
            });

        it('should give set of actions on null destination storage location change',
            () => {
                const actions = [];
                expect(actionProvider.getlogisticsActions('destinationStorageLocation', null)).toEqual(actions);
            });

        it('should give set of actions on not null destination storage location change',
            () => {
                const value = { destinationStorageLocation: 'name' };
                const actions = [ setStorageLocation(value, false) ];
                expect(JSON.stringify(actionProvider.getlogisticsActions('destinationStorageLocation', value))).toEqual(JSON.stringify(actions));
            });

        it('should give set of actions on null source storage location change',
            () => {
                const actions = [];
                expect(actionProvider.getlogisticsActions('sourceStorageLocation', null)).toEqual(actions);
            });

        it('should give set of actions on not null source storage location change',
            () => {
                const value = { destinationStorageLocation: 'name' };
                const actions = [ setStorageLocation(value, true) ];
                expect(JSON.stringify(actionProvider.getlogisticsActions('sourceStorageLocation', value))).toEqual(JSON.stringify(actions));
            });
    });
