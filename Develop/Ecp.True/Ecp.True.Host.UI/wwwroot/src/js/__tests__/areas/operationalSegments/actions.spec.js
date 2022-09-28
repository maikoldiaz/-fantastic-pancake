import * as actions from '../../../modules/administration/operationalSegments/actions';
import { apiService } from '../../../common/services/apiService';

describe('actions for operational segments', () => {
    it('should update son segment success', () => {
        const UPDATE_SON_SEGMENTS_SUCCESS = 'UPDATE_SON_SEGMENTS_SUCCESS';
        const data = 'test data';

        const action = actions.updateSegmentsSuccess(data);

        expect(action.type).toEqual(UPDATE_SON_SEGMENTS_SUCCESS);
        expect(action.data).toEqual(data);
    })

    it('should update son segment failure', () => {
        const UPDATE_SON_SEGMENTS_FAILURE = 'UPDATE_SON_SEGMENTS_FAILURE';

        const action = actions.updateSegmentsFailure();

        expect(action.type).toEqual(UPDATE_SON_SEGMENTS_FAILURE);
    })

    it('should update segments', () => {
        const UPDATE_SEGMENTS = 'UPDATE_SON_SEGMENTS';
        const operational = [{
            id: 1,
            name: 'test',
            value: 0,
            selected: false,
            rowVersion: 'testRowVersion'
        }];
        const others = [{
            id: 2,
            name: 'test',
            value: 0,
            selected: false,
            rowVersion: 'testRowVersion'
        }];

        const action = actions.updateSegments(operational, others);

        expect(action.type).toEqual(UPDATE_SEGMENTS);
        expect(action.operational).toEqual(operational);
        expect(action.others).toEqual(others); 
    })

    it('should save segments', () => {
        const SAVE_SON_SEGMENTS = 'SAVE_SON_SEGMENTS';
        const segments = [];
        const action = actions.saveSegments(segments);

        expect(action.type).toEqual(SAVE_SON_SEGMENTS);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.category.updateCategoryElements());
        expect(action.fetchConfig.success).toBeDefined
    })
})