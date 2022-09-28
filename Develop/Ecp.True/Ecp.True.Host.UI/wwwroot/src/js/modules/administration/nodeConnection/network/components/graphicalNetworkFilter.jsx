import React from 'react';
import { connect } from 'react-redux';
import { getGraphicalNetwork, saveGraphicalFilter } from '../actions';
import NodeFilter from '../../../../../common/components/filters/nodeFilter.jsx';
import { navigationService } from '../../../../../common/services/navigationService';
import { nodeFilterConfigService } from '../../../../../common/services/nodeFilterConfigService';
import { utilities } from '../../../../../common/services/utilities';

export class GraphicalNetworkFilter extends React.Component {
    constructor() {
        super();
        this.filterCategories = this.filterCategories.bind(this);
        this.onSubmitFilter = this.onSubmitFilter.bind(this);
    }

    filterCategories(allCategories) {
        return allCategories.filter(x => x.categoryId === 2 || x.categoryId === 8);
    }

    onSubmitFilter(formValues) {
        const filters = Object.assign({}, formValues, {
            elementId: formValues.element.elementId,
            nodeId: formValues.node.name === 'Todos' ? 0 : formValues.node.nodeId
        });
        this.props.saveGraphicalFilter({ elementId: filters.elementId, nodeId: filters.nodeId });
        this.props.getGraphicalNetwork(filters.elementId, filters.nodeId);
        navigationService.navigateTo('manage');
    }

    render() {
        return (
            <NodeFilter config={nodeFilterConfigService.getGraphicalNodeConfig(this.filterCategories, this.onSubmitFilter)} />
        );
    }
}

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        getGraphicalNetwork: (elementId, nodeId) => {
            dispatch(getGraphicalNetwork(elementId, nodeId));
        },
        saveGraphicalFilter: filters => {
            dispatch(saveGraphicalFilter(filters));
        }
    };
};


export default connect(null, mapDispatchToProps, utilities.merge)(GraphicalNetworkFilter);
