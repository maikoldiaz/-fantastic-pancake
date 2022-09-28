import * as actions from '../../../modules/administration/annulation/actions';
import { annulations } from '../../../modules/administration/annulation/reducers';
import { constants } from '../../../common/services/constants';

describe('Reducers for Reversals', () => {
    const initialState = {
        fieldChange: { fieldChangeToggler: false },
        source: { movement: 0, node: 0, product: 0 },
        annulation: { movement: 0, node: 0, product: 0 }
    };

    it('should handle action INIT_ANNULATION_TYPES',
        function () {
            const action = {
                type: actions.INIT_ANNULATION_TYPES
            };
            const newState = Object.assign({}, initialState, {
                source: { movement: 0, node: 0, product: 0 },
                annulation: { movement: 0, node: 0, product: 0 },
                initialValues: { isActive: true }
            });
            expect(annulations(initialState, action)).toEqual(newState);
        });

    it('should handle action UPDATE_ANNULATION_TYPES for movement',
        function () {
            const name = constants.Annulations.Sections.Source;
            const value = { elementId: 1 };
            const field = constants.Annulations.Fields.Movement;
            const action = {
                type: actions.UPDATE_ANNULATION_TYPES,
                name,
                value,
                field
            };
            const updateFieldValue = action.value.elementId;
            const sectionState = initialState[action.name];

            const newState = Object.assign({}, initialState, {
                [action.name]: Object.assign({}, sectionState, {
                    [action.field]: updateFieldValue
                })
            });
            expect(annulations(initialState, action)).toEqual(newState);
        });

    it('should handle action UPDATE_ANNULATION_TYPES for node with origin type not none',
        function () {
            const name = constants.Annulations.Sections.Source;
            const value = { originTypeId: 1 };
            const field = constants.Annulations.Fields.Node;
            const action = {
                type: actions.UPDATE_ANNULATION_TYPES,
                name,
                value,
                field
            };
            const updateFieldValue = 0;
            const sectionState = initialState[action.name];

            const newState = Object.assign({}, initialState, {
                [action.name]: Object.assign({}, sectionState, {
                    [action.field]: updateFieldValue
                })
            });
            expect(annulations(initialState, action)).toEqual(newState);
        });

    it('should handle action UPDATE_ANNULATION_TYPES for node with origin type none',
        function () {
            const name = constants.Annulations.Sections.Source;
            const value = { originTypeId: 3 };
            const field = constants.Annulations.Fields.Node;
            const action = {
                type: actions.UPDATE_ANNULATION_TYPES,
                name,
                value,
                field
            };
            const updateFieldValue = 3;
            const sectionState = initialState[action.name];

            const newState = Object.assign({}, initialState, {
                [action.name]: Object.assign({}, sectionState, {
                    [action.field]: updateFieldValue
                })
            });
            expect(annulations(initialState, action)).toEqual(newState);
        });

    it('should handle action RESET_ANNULATION_TYPES',
        function () {
            const name = constants.Annulations.Sections.Source;
            const field = constants.Annulations.Fields.Movement;
            const action = {
                type: actions.RESET_ANNULATION_TYPES,
                name,
                field
            };
            const sectionState = initialState[action.name];

            const newState = Object.assign({}, initialState, {
                [action.name]: Object.assign({}, sectionState, {
                    [action.field]: 0
                })
            });
            expect(annulations(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_SAVE_ANNULATION',
        function () {
            const action = {
                type: actions.RECEIVE_SAVE_ANNULATION
            };

            const newState = Object.assign({}, initialState, {
                saveSuccess: !initialState.saveSuccess
            });
            expect(annulations(initialState, action)).toEqual(newState);
        });

    it('should handle action INIT_ANNULATION',
        function () {
            const initialValues = {
                source: {
                    movement: { elementId: 1, name: 'test' },
                    product: { originTypeId: 1, name: 'test' },
                    node: { originTypeId: 3, name: 'test' }
                },
                annulation: {
                    movement: { elementId: 2, name: 'test' },
                    product: { originTypeId: 3, name: 'test' },
                    node: { originTypeId: 1, name: 'test' }
                },
                annulationId: 1,
                isActive: true,
                rowVersion: 'test'
            };
            const action = {
                type: actions.INIT_ANNULATION,
                initialValues
            };

            const newState = Object.assign({}, initialState, {
                initialValues: action.initialValues,
                source: {
                    movement: 2,
                    product: constants.OriginTypes.None,
                    node: 0
                },
                annulation: {
                    movement: 1,
                    product: 0,
                    node: constants.OriginTypes.None
                }
            });
            expect(annulations(initialState, action)).toEqual(newState);
        });

    it('should handle form field change',
        function () {
            const action = {
                type: '@@redux-form/CHANGE',
                meta: {
                    form: 'annulation',
                    field: 'source.movement',
                    payload: { elementId: 1 }
                }
            };
            const newState = Object.assign({}, initialState, {
                fieldChange: {
                    fieldChangeToggler: !initialState.fieldChange.fieldChangeToggler,
                    currentModifiedField: action.meta.field,
                    currentModifiedValue: action.payload
                }
            });
            expect(annulations(initialState, action)).toEqual(newState);
        });
    it('should ignore form field change in other forms',
        function () {
            const action = {
                type: '@@redux-form/CHANGE',
                meta: {
                    form: 'dummyForm',
                    field: 'source.movement',
                    payload: { elementId: 1 }
                }
            };
            expect(annulations(initialState, action)).toEqual(initialState);
        });
    it('should handle default case',
        function () {
            const action = {
                type: 'test'
            };
            expect(annulations(initialState, action)).toEqual(initialState);
        });
});
