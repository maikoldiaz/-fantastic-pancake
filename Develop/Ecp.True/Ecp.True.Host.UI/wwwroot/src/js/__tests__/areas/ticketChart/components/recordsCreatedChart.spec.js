import setup from './../../setup.js';
import React from 'react';
import { mount } from 'enzyme';
import RecordsCreatedChart from './../../../../modules/transportBalance/cutOff/components/recordsCreatedChart';

const data = [
    {
        name: 'category 1',
        value: 100
    },
    {
        name: 'category 2',
        value: 200
    }
];
function mountWithRealStore() {
    const enzymeWrapper = mount(<RecordsCreatedChart data={data} />);
    return { enzymeWrapper };
}

describe('records Created Chart', () => {
    it('should mount with data', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });
});
