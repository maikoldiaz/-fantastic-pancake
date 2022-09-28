import setup from '../../setup';
import React from 'react';
import { shallow } from 'enzyme';
import InputSegment
  from '../../../../modules/administration/registryDeviationConfiguration/components/inputSegment';

describe('Input segment - registry deviation configuration', () => {
  it('should render four segments', () => {
    const segments =  [
      {
        name: 'segment-1',
        deviationPercentage: 2.00
      },
      {
        name: 'segment-2',
        deviationPercentage: 2.00
      },
      {
        name: 'segment-3',
        deviationPercentage: 2.00
      }
    ];
    segments.get = jest.fn((i) => segments[i]);

    const props = {
      fields: segments,
      maxDeviationPercentage: 3.00,
      minDeviationPercentage: 0
    };

    const enzymeWrapper = shallow(<InputSegment {...props} />);
    const segmentLabels = enzymeWrapper.find('.ep-label');
    expect(segmentLabels).toHaveLength(3);
  });
});
