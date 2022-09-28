import setup from '../../../setup.js';
import React from 'react';
import { shallow } from 'enzyme';
import { navigationService } from '../../../../../common/services/navigationService';
import { officialNodeStatusReportFilterBuilder } from '../../../../../modules/report/officialNodesStatus/filterBuilder';
import { OfficialNodesStatusReport as OfficialNodesStatusReportComponent } from '../../../../../modules/report/officialNodesStatus/components/report.jsx';

describe('OfficialNodesStatusReport', () => {
    beforeAll(() => {
        navigationService.navigateTo = jest.fn();
        officialNodeStatusReportFilterBuilder.build = jest.fn();
    });

    it('should mount successfully', () => {
        const props = {
            filters: {}
        };

        const wrapper = shallow(<OfficialNodesStatusReportComponent {...props} />);
        expect(wrapper).toHaveLength(1);
    });

    it('should call to the manage view', () => {
        const props = {
            filters: null
        };

        shallow(<OfficialNodesStatusReportComponent {...props} />);
        expect(navigationService.navigateTo.mock.calls).toHaveLength(1);
    });
});
