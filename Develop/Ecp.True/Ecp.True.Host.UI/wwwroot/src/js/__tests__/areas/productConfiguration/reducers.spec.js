import * as actions from '../../../modules/administration/productConfiguration/actions';
import { products } from '../../../modules/administration/productConfiguration/reducers';

describe('reducer test for product configuration', () => {
    it('should handle init Product Action', () => {
        const intialState = {};
        const initialValues = {
            product: {
                isActive: undefined,
                productSapId: undefined,
                productSapName: undefined,
                rowVersion: undefined
            }        
        }; 
        const action = {
            type: actions.INIT_PRODUCT,
            initialValues: initialValues
        };
        const newState = Object.assign({}, {}, {
            initialValues: initialValues
        });
        expect(products(intialState, action)).toEqual(newState);
    });

    it('should handle Receive save product Action', () => {
        const state = {
            updateToggler: false,
            saveSuccess: true
        }; 
        const action = {
            type: actions.RECEIVE_SAVE_PRODUCT,
            status: true
        };
        const newState = Object.assign({}, state, {
            saveSuccess: action.status,
            updateToggler: true
        });
        expect(products(state, action)).toEqual(newState);
    });

    it('should handle Receive update product Action', () => {
        const state = {
            updateToggler: false,
            updateSuccess: true
        }; 
        const action = {
            type: actions.RECEIVE_UPDATE_PRODUCT,
            status: true
        };
        const newState = Object.assign({}, state, {
            updateSuccess: action.status,
            updateToggler: true
        });
        expect(products(state, action)).toEqual(newState);
    });

    
    it('should handle Receive delete product Action', () => {
        const state = {
            updateToggler: false,
            deleteSuccess: true
        }; 
        const action = {
            type: actions.RECEIVE_DELETE_PRODUCT,
            status: true
        };
        const newState = Object.assign({}, state, {
            deleteSuccess: action.status,
            updateToggler: true
        });
        expect(products(state, action)).toEqual(newState);
    });

    it('should handle clear flags Success product Action', () => {
        const state = {
            saveSuccess: undefined,
            updateSuccess: undefined,
            deleteSuccess: undefined
        }; 
        const action = {
            type: actions.CLEAR_SUCCESS
        };
        const newState = Object.assign({}, state, {
            saveSuccess: undefined,
            updateSuccess: undefined,
            deleteSuccess: undefined
        });
        expect(products(state, action)).toEqual(newState);
    });

});
