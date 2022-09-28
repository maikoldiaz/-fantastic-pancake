import {
    RECEIVE_ADD_CATEGORYELEMENT,
    RECEIVE_UPDATE_CATEGORYELEMENT,
    REQUEST_EDIT_INITIALIZE_CATEGORYELEMENT,
    RECEIVE_ICONS,
    OPEN_ICON_MODAL,
    REFRESH_STATUS,
    INITIALIZE_VALUE
} from './actions.js';

export const manageElement = (state = { iconModal: false }, action = {}) => {
    switch (action.type) {
    case RECEIVE_ADD_CATEGORYELEMENT:
    case RECEIVE_UPDATE_CATEGORYELEMENT:
        return Object.assign({},
            state,
            {
                status: action.status,
                refreshToggler: !state.refreshToggler
            });
    case REQUEST_EDIT_INITIALIZE_CATEGORYELEMENT:
        return Object.assign({},
            state,
            {
                categoryElement: action.categoryElement
            });
    case RECEIVE_ICONS:
        return Object.assign({},
            state,
            {
                icons: action.icons
            });
    case OPEN_ICON_MODAL:
        return Object.assign({},
            state,
            {
                iconModal: !state.iconModal
            });
    case REFRESH_STATUS:
        return Object.assign({},
            state,
            {
                status: {
                    message: ''
                }
            });
    case INITIALIZE_VALUE:
        return Object.assign({},
            state,
            {
                initialValues: {
                    category: {
                        name: action.categoryElement.category.name,
                        categoryId: action.categoryElement.categoryId
                    },
                    name: action.categoryElement.name,
                    description: action.categoryElement.description,
                    color: action.categoryElement.color,
                    icon: action.categoryElement.icon ? action.categoryElement.icon.name + '.svg' : null,
                    isActive: action.categoryElement.isActive,
                    rowVersion: action.categoryElement.rowVersion
                }
            });
    default:
        return state;
    }
};
