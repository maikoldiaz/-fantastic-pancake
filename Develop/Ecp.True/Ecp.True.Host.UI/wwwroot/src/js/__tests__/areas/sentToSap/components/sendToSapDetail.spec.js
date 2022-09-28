import setup from '../../setup';
import React from 'react';
import { Provider } from 'react-redux';
import { createStore, combineReducers } from 'redux';
import { mount, shallow } from 'enzyme';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { navigationService } from '../../../../common/services/navigationService';
import SendToSapDetail, { SendToSapDetail as SendToSapDetailComponent } from '../../../../modules/dailyBalance/sentToSap/components/sendToSapDetail';
import Grid from '../../../../common/components/grid/grid.jsx';
import { routerActions } from '../../../../common/router/routerActions';

const movements = [
    {
        "movementId": "637515992349374143",
        "movementTransactionId": 281031,
        "state": "Finalizado",
        "description": "",
        "movementType": "Pérdida no identificada",
        "sourceCenter": "PR SAN ROQUE",
        "sourceStorage": null,
        "sourceProduct": "CRUDO MEZCLA",
        "destinationCenter": "PR PROVINCIA",
        "destinationStorage": null,
        "destinationProduct": "CRUDO MEZCLA",
        "ownershipVolume": 5500,
        "units": "Bbl",
        "operationalDate": "2021-03-12T00:00:00Z",
        "costCenter": null,
        "gmCode": null,
        "documentNumber": null,
        "position": null,
        "order": 1,
        "accountingDate": "2021-04-28T11:26:06.91Z",
        "segment": "SegJuliPrueba",
        "scenario": "Operativo",
        "owner": "ECOPETROL",
        "createdBy": null,
        "createdDate": null,
        "lastModifiedBy": null,
        "lastModifiedDate": null
    },
    {
        "movementId": "637515992349392889",
        "movementTransactionId": 281032,
        "state": "Fallido",
        "description": null,
        "movementType": "Pérdida no identificada",
        "sourceCenter": "PR CAMPO RICO",
        "sourceStorage": null,
        "sourceProduct": "CRUDO MEZCLA",
        "destinationCenter": "PR CARROTANQUES",
        "destinationStorage": null,
        "destinationProduct": "CRUDO MEZCLA",
        "ownershipVolume": 2000,
        "units": "Bbl",
        "operationalDate": "2021-03-14T00:00:00Z",
        "costCenter": null,
        "gmCode": null,
        "documentNumber": null,
        "position": null,
        "order": 1,
        "accountingDate": "2021-04-28T11:26:06.91Z",
        "segment": "SegJuliPrueba",
        "scenario": "Operativo",
        "owner": "ECOPETROL",
        "createdBy": null,
        "createdDate": null,
        "lastModifiedBy": null,
        "lastModifiedDate": null
    }
]

function mountWithRealStore() {
    const dataGrid = {
        sendToSap: {
            ticket: {
                segment: 'Transporte',
                ticketStartDate: '2021-04-27T00:00:00Z',
                ticketFinalDate: '2021-04-28T00:00:00Z',
                ownerName: 'Ecopetrol',
                scenarioName: 'Operativo'
            }
        },
        grid: {
            movements: {
                config: { name: 'movements', idField: 'movementTransactionId', selectable: true, odata: false, startEmpty: false, apiUrl: 'domain.com' },
                selection: [{ movementTransactionId: 1 }],
                items: movements
            }
        }
    };

    const grid = { movements: dataGrid.grid.movements };

    const reducers = {
        grid: jest.fn(() => grid),
        sendToSap: jest.fn(() => dataGrid.sendToSap),
    };

    const store = createStore(combineReducers(reducers));
    const dispatchSpy = jest.spyOn(store, 'dispatch');

    const enzymeWrapper = mount(<Provider store={store}>
        <SendToSapDetail name="movements" />
    </Provider>);
    return { store, enzymeWrapper, dispatchSpy };
}

describe('SendToSapDetail', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        navigationService.getParamByName = jest.fn(key => key);
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(19);
    });

    it('should call router action', () => {
        const { dispatchSpy } = mountWithRealStore();
        routerActions.fireAction('forwardToSap');
        expect(dispatchSpy).toHaveBeenCalledWith(expect.objectContaining({ type: 'REQUEST_FORWARD_MOVEMENTS' }));
    });

    it('should update selection', () => {
        const props = {
            selection: [],
            enableDisableForward: jest.fn(),
            getTicket: jest.fn()
        };

        const wrapper = shallow(<SendToSapDetailComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { selection: ['test'] }));
        expect(props.enableDisableForward.mock.calls).toHaveLength(2);
    });

    it('should update get movements', () => {
        const props = {
            selection: [],
            enableDisableForward: jest.fn(),
            getTicket: jest.fn(),
            getLogisticsMovements: jest.fn()
        };

        const wrapper = shallow(<SendToSapDetailComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { selection: ['test'], forwardToggler: false }));
        expect(props.enableDisableForward.mock.calls).toHaveLength(2);
    });
});