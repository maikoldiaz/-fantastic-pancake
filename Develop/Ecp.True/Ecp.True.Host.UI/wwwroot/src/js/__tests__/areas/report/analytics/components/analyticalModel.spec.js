import React from 'react';
import Enzyme, { mount, shallow } from 'enzyme';
import EnzymeAdapter from 'enzyme-adapter-react-16';
import AnalyticalModel from '../../../../../modules/report/analytics/components/analyticalModel.jsx';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
Enzyme.configure({ adapter: new EnzymeAdapter() });
function mountWithRealStore() {
    const enzymeWrapper = mount(<Provider store={store}{...props} >
        <AnalyticalModel />
    </Provider>);
    return { enzymeWrapper };
}

describe('AnalyticalModel', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
    });

    it('should mount component returned by toPbiReport test',
        async () => {

            const wrapper = shallow(<AnalyticalModel />)
            expect(wrapper).toHaveLength(1);
        });
});
