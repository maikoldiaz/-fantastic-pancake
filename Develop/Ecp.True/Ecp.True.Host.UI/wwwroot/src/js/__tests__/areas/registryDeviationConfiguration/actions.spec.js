import * as actions from '../../../modules/administration/registryDeviationConfiguration/actions';

describe('Actions for registryDeviationConfiguration', () => {
    it('should get the filtered segments', () => {
        const jsonItems = [{
            elementId: 1,
            name: 'segment-1'
        }];
        const action = actions.filterCategoryElements('filter-x');

        expect(action.type).toEqual(actions.FILTER_CATEGORY_ELEMENTS);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(jsonItems);
        expect(receiveAction.type).toEqual(actions.RECEIVE_FILTER_CATEGORY_ELEMENTS);
        expect(receiveAction.items).toEqual(jsonItems);
    });

    it('should update the segment deviation', () => {
        const updatedSegment = [{
            elementId: 1,
            name: 'segment-1',
            deviationPercentage: 1.00
        }];
        const action = actions.updateDeviation(updatedSegment);

        expect(action.type).toEqual(actions.REQUEST_UPDATE_DEVIATION);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.body).toEqual(updatedSegment);
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success('successful');
        expect(receiveAction.type).toEqual(actions.RECEIVE_UPDATE_DEVIATION);
        expect(receiveAction.status).toEqual('successful');
    });
});
