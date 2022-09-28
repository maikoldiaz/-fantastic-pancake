import setup from '../../../setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { reducer as formReducer } from 'redux-form';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import AddManualMovements, { AddManualMovements as AddManualMovementsComponent } from '../../../../../modules/report/officialDeltaNode/components/addManualMovements.jsx';
import { navigationService } from '../../../../../common/services/navigationService';


const formValues = {
    element: {
        elementId: 0,
        name: 'name element'
    },
    node: {
        nodeId: 0,
        name: 'name node'
    },
    periods: { start: '2020-08-01', end: '2020-08-31', officialPeriods: [] }
};

describe('Report OfficialDeltaNode AddManualMovements', () => {
    beforeAll(() => {
    });

    it('should mount successfully', () => {
        const manualMovements = [];
        const props = {
            init: jest.fn(),
            initialDateShort: new Date(),
            finalDateShort: new Date(),
            getRequestDeltaNodeMovements: jest.fn(() => manualMovements),
            report: { officialDeltaNode: { manualMovements: manualMovements } },
            manualMovements: manualMovements
        }
        const enzymeWrapper = shallow(<AddManualMovementsComponent {...props} />);
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should call componentDidMount successfully', () => {
        const manualMovements = [];
        const props = {
            init: jest.fn(),
            initialDateShort: new Date(),
            finalDateShort: new Date(),
            getRequestDeltaNodeMovements: jest.fn(() => manualMovements),
            report: { officialDeltaNode: { manualMovements: manualMovements } },
            manualMovements: manualMovements
        }
        const enzymeWrapper = shallow(<AddManualMovementsComponent {...props} />);
        expect(props.getRequestDeltaNodeMovements.mock.calls).toHaveLength(1);
    });

    it('should be render modal successfully', () => {
        const manualMovements = [];
        const props = {
            init: jest.fn(),
            initialDateShort: new Date(),
            finalDateShort: new Date(),
            getRequestDeltaNodeMovements: jest.fn(() => manualMovements),
            report: { officialDeltaNode: { manualMovements: manualMovements } },
            manualMovements: manualMovements
        }
        const enzymeWrapper = shallow(<AddManualMovementsComponent {...props} />);
        expect(enzymeWrapper.exists('#AddManualMovements_Modal')).toBe(true);
    });

    it('should close modal when form is saved successfully', () => {
        const manualMovements = [];
        const props = {
            init: jest.fn(),
            initialDateShort: new Date(),
            finalDateShort: new Date(),
            getRequestDeltaNodeMovements: jest.fn(() => manualMovements),
            report: { officialDeltaNode: { manualMovements: manualMovements } },
            manualMovements: manualMovements,
            closeModal: jest.fn(),
            isSaveForm: null
        }
        const enzymeWrapper = shallow(<AddManualMovementsComponent {...props} />);
        enzymeWrapper.setProps(Object.assign({}, props, { isSaveForm: true }));
        expect(props.closeModal.mock.calls).toHaveLength(1);
    })

    it('should render table successfully when partial data is completed', () => {
        const manualMovements = [{ movementType: { name: "Hello" }, movementSource: { sourceNode: '', sourceProduct: '' }, movementDestination: { destinationNode: '', destinationProduct: '' }, owners: [{ ownerElement: { name: '' } }] }];
        const props = {
            init: jest.fn(),
            initialDateShort: new Date(),
            finalDateShort: new Date(),
            getRequestDeltaNodeMovements: jest.fn(() => manualMovements),
            report: { officialDeltaNode: { manualMovements: manualMovements } },
            manualMovements: manualMovements,
            closeModal: jest.fn(),
            isSaveForm: false
        }
        const enzymeWrapper = shallow(<AddManualMovementsComponent {...props} />);
        expect(props.getRequestDeltaNodeMovements.mock.calls).toHaveLength(1);
    })

    it('should render table successfully when partial data is missing', () => {
        const manualMovements = [{ movementType: { name: "Hello" }, movementSource: null, movementDestination: null, owners: [] }];
        const props = {
            init: jest.fn(),
            initialDateShort: new Date(),
            finalDateShort: new Date(),
            getRequestDeltaNodeMovements: jest.fn(() => manualMovements),
            report: { officialDeltaNode: { manualMovements: manualMovements } },
            manualMovements: manualMovements,
            closeModal: jest.fn(),
            isSaveForm: false
        }
        const enzymeWrapper = shallow(<AddManualMovementsComponent {...props} />);
        expect(props.getRequestDeltaNodeMovements.mock.calls).toHaveLength(1);
    })
});

