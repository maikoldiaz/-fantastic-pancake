import React from 'react';
import setup from '../../setup';
import { shallow } from 'enzyme';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import { OfficialDeltaInProgressMessage } from '../../../../modules/dailyBalance/officialDelta/components/progressMessage.jsx';

describe('officialPointsCommentMessage', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const wrapper = shallow(<OfficialDeltaInProgressMessage />);
        expect(wrapper).toHaveLength(1);
    });
});
