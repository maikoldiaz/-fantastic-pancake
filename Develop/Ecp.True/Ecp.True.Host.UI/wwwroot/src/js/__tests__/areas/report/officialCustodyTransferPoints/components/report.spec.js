import React from 'react';
import Enzyme, { mount, shallow } from 'enzyme';
import EnzymeAdapter from 'enzyme-adapter-react-16';
import OfficialCustodyTransferPointsReport from '../../../../../modules/report/officialCustodyTransferPoints/components/report.jsx';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
Enzyme.configure({ adapter: new EnzymeAdapter() });
function mountWithRealStore() {
    const enzymeWrapper = mount(<Provider store={store}{...props} >
        <OfficialCustodyTransferPointsReport />
    </Provider>);
    return { enzymeWrapper };
}

describe('OfficialCustodyTransferPointsReport', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
    });

    it('should mount component returned by toPbiReport test',
        async () => {

            const wrapper = shallow(<OfficialCustodyTransferPointsReport />)
            expect(wrapper).toHaveLength(1);
        });
});
