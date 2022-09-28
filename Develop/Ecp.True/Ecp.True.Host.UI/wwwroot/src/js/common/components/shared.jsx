import React from 'react';
import { connect } from 'react-redux';
import { utilities } from '../services/utilities';
import { requestCategoryElements, requestLogisticCenters, requestStorageLocations, requestCategories, requestSystemTypes, requestVariableTypes, requestOriginTypes } from '../actions.js';

export class Shared extends React.Component {
    render() {
        return null;
    }

    componentDidUpdate(prevProps) {
        if (prevProps.categoryElementsToggler !== this.props.categoryElementsToggler) {
            this.props.requestCategoryElements();
        }
        if (prevProps.allCategoriesToggler !== this.props.allCategoriesToggler) {
            this.props.requestCategories();
        }
        if (prevProps.logisticCentersToggler !== this.props.logisticCentersToggler) {
            this.props.requestLogisticCenters();
        }
        if (prevProps.storageLocationsToggler !== this.props.storageLocationsToggler) {
            this.props.requestStorageLocations();
        }
        if (prevProps.systemTypesToggler !== this.props.systemTypesToggler) {
            this.props.requestSystemTypes();
        }
        if (prevProps.variableTypesToggler !== this.props.variableTypesToggler) {
            this.props.requestVariableTypes();
        }
        if (prevProps.originTypesToggler !== this.props.originTypesToggler) {
            this.props.requestOriginTypes();
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        categoryElementsToggler: state.shared.categoryElementsToggler,
        allCategoriesToggler: state.shared.allCategoriesToggler,
        logisticCentersToggler: state.shared.logisticCentersToggler,
        storageLocationsToggler: state.shared.storageLocationsToggler,
        systemTypesToggler: state.shared.systemTypesToggler,
        variableTypesToggler: state.shared.variableTypesToggler,
        originTypesToggler: state.shared.originTypesToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        requestCategoryElements: () => {
            dispatch(requestCategoryElements());
        },
        requestCategories: () => {
            dispatch(requestCategories());
        },
        requestLogisticCenters: () => {
            dispatch(requestLogisticCenters());
        },
        requestStorageLocations: () => {
            dispatch(requestStorageLocations());
        },
        requestSystemTypes: () => {
            dispatch(requestSystemTypes());
        },
        requestVariableTypes: () => {
            dispatch(requestVariableTypes());
        },
        requestOriginTypes: () => {
            dispatch(requestOriginTypes());
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(Shared);
