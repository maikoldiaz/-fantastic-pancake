import setup from '../../setup';
import React from 'react';
import { shallow } from 'enzyme';
import { NodeOwnership } from './../../../../modules/transportBalance/nodeOwnership/components/nodeOwnership.jsx';
import SummaryGrid from './../../../../modules/transportBalance/nodeOwnership/components/summaryGrid.jsx';
import NodeDetails from './../../../../modules/transportBalance/nodeOwnership/components/nodeDetails.jsx';

describe('nodeOwnership', () => {
    it('should mount successfully', () => {
        const props = { ownershipNodeId: 1, getOwnershipNodeDetails: jest.fn() };
        const enzymeWrapper = shallow(<NodeOwnership {...props} />);
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should pass ownershipNodeId as prop to summary grid and node details page', () => {
        const props = { ownershipNodeId: 1, getOwnershipNodeDetails: jest.fn() };
        const enzymeWrapper = shallow(<NodeOwnership {...props} />);
        expect(enzymeWrapper.find(SummaryGrid).at(0).prop('ownershipNodeId')).toEqual(1);
        expect(enzymeWrapper.find(NodeDetails).at(0).prop('ownershipNodeId')).toEqual(1);
    });

    it('should get ownership node details on componentDidMount', () => {
        const props = { ownershipNodeId: 1, getOwnershipNodeDetails: jest.fn() };
        const enzymeWrapper = shallow(<NodeOwnership {...props} />);
        enzymeWrapper.instance().componentDidMount();
        expect(props.getOwnershipNodeDetails).toHaveBeenCalled();
    });
});
