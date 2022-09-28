import setup from '../../setup';
import React from 'react';
import { shallow } from 'enzyme';
import { OwnershipHeader } from '../../../../modules/transportBalance/nodeOwnership/components/ownershipHeader.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { dateService } from '../../../../common/services/dateService';

describe('ownershipHeader', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        dateService.format = jest.fn(() => '01/01/2020');
    });
    it('should mount successfully', () => {
        const props = { nodeDetails: { ticket: { categoryElement: { name: 'test' }, startDate: '01/01/2020', endDate: '01/01/2020' }, node: { name: 'test' } } };
        const enzymeWrapper = shallow(<OwnershipHeader {...props} />);
        expect(enzymeWrapper).toHaveLength(1);
    });
});
