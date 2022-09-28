import setup from '../../../setup';
import { createStore, combineReducers } from 'redux';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { Provider } from 'react-redux';
import { httpService } from '../../../../../common/services/httpService';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import Grid from '../../../../../common/components/grid/grid.jsx';
import ProductsGrid, {ProductsGrid as ProductsGridComponent } from '../../../../../modules/administration/productConfiguration/components/product/productsGrid'; 

function mountWithRealStore() {


    const data = {
        pageActions: {
            hiddenActions: [],
            disabledActions: []
        },
        products: {
            "updateToggler": true,
            "initialValues": {
                "product": {
                    "productSapId": "10000002049",
                    "productSapName": "CRUDOS IMPORTADOS",
                    "isActive": false,
                    "rowVersion": "AAAAAAADuyI="
                }
            }
        },

        grid: {
            products: {
                "config": {
                    "name": "products",
                    "apiUrl": "https://localhost:44315/v1/odata/products?$select=productId,name,isActive,rowVersion",
                    "idField": "productId",
                    "sortable": {
                        "defaultSort": "productId"
                    },
                    "section": true
                },
                "selection": [],
                "selectAll": false,
                "items": [
                    {
                        "productId": "97863",
                        "name": "nuhbf",
                        "isActive": false,
                        "rowVersion": "AAAAAAADuyE="
                    },
                    {
                        "productId": "741852",
                        "name": "NAFTA CLARA",
                        "isActive": false,
                        "rowVersion": "AAAAAAADvQE="
                    },
                    {
                        "productId": "65435",
                        "name": "fghhfg",
                        "isActive": true,
                        "rowVersion": "AAAAAAADobM="
                    },
                    {
                        "productId": "5893",
                        "name": "asdgh",
                        "isActive": true,
                        "rowVersion": "AAAAAAADux4="
                    },
                    {
                        "productId": "580053",
                        "name": "ujgdf",
                        "isActive": true,
                        "rowVersion": "AAAAAAADux8="
                    },
                    {
                        "productId": "5678",
                        "name": "sdfg",
                        "isActive": true,
                        "rowVersion": "AAAAAAADung="
                    },
                    {
                        "productId": "4858",
                        "name": "sdfasf",
                        "isActive": true,
                        "rowVersion": "AAAAAAADunk="
                    },
                    {
                        "productId": "40000009200",
                        "name": "GAS BULLERENGUE",
                        "isActive": true,
                        "rowVersion": "AAAAAAADnqA="
                    },
                    {
                        "productId": "40000009191",
                        "name": "GAS CAMPO LOMA LARGA",
                        "isActive": true,
                        "rowVersion": "AAAAAAADnp8="
                    },
                    {
                        "productId": "40000009182",
                        "name": "GAS LOMA LARGA",
                        "isActive": true,
                        "rowVersion": "AAAAAAADnp4="
                    }
                ],
                "pageItems": [],
                "totalItems": 899,
                "filterValues": {},
                "pageFilters": {},
                "receiveDataToggler": false,
                "refreshToggler": true
            }
        }


    };

    const reducers = {
        pageActions: jest.fn(() => data.pageActions),
        grid: jest.fn(() => data.grid),
        products: jest.fn(() => data.products),
    };

    const initialProps = {};

    const store = createStore(combineReducers(reducers));

    const props = Object.assign({}, initialProps, { hideEdit: true, hideEdit: true });
    const enzymeWrapper = mount(<Provider store={store} >
        <ProductsGrid name="products" />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('products', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);

    });

    it('should pass the grid name for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).at(0).prop('name')).toEqual('products');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(4);
    }); 
    it('should open confirm modal on delete', () => {
        const props = {
            openConfirmModal: jest.fn(),
        };

        const wrapper = shallow(<ProductsGridComponent {...props} />);
        wrapper.instance().onDelete();
        expect(props.openConfirmModal.mock.calls).toHaveLength(1);
    });
});
