import React from 'react';
import setup from '../../setup';
import { shallow } from 'enzyme';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import { OfficialPointsErrorMessage } from '../../../../modules/transportBalance/cutOff/components/officialPointsErrorMessage.jsx';

describe('officialPointsErrorMessage', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const props = {
            officialPoint: {
                movementId: '1',
                movementTypeName: 'type',
                operationalDate: '2020-06-15T00:00:00:000',
                errorMessage: 'message',
                errors: []
            }
        };
        const wrapper = shallow(<OfficialPointsErrorMessage {...props}/>);
        expect(wrapper).toHaveLength(1);
    });

    it('should find labels', () => {
        const props = {
            officialPoint: {
                movementId: '1',
                movementTypeName: 'type',
                operationalDate: '2020-06-15T00:00:00:000',
                errorMessage: 'message',
                errors: []
            }
        };
        const wrapper = shallow(<OfficialPointsErrorMessage {...props} />);
        expect(wrapper).toHaveLength(1);
        expect(wrapper.find('#lbl_officialPointsCommentMessage_movementId')).toHaveLength(1);
        expect(wrapper.find('#lbl_officialPointsCommentMessage_movementType')).toHaveLength(1);
        expect(wrapper.find('#lbl_officialPointsCommentMessage_operationalDate')).toHaveLength(1);
        expect(wrapper.find('#lbl_officialPointsCommentMessage_error')).toHaveLength(1);
    });

    it('should show errors list', () => {
        const props = {
            officialPoint: {
                movementId: '1',
                movementTypeName: 'type',
                operationalDate: '2020-06-15T00:00:00:000',
                errorMessage: '',
                errors: [{ errorCode: '0001', errorDescription: 'description' }]
            }
        };
        const wrapper = shallow(<OfficialPointsErrorMessage {...props}/>);
        expect(wrapper).toHaveLength(1);
        expect(wrapper.find('#lbl_officialPointsCommentMessage_errorlength')).toHaveLength(1);
        expect(wrapper.find('#lbl_officialPointsCommentMessage_errorlength').text()).toEqual('1');
    });
});
