import React from 'react';
import { connect } from 'react-redux';
import { nodeConfigurationReportSaveReportFilter } from '../actions';
import NodeFilter from '../../../../common/components/filters/nodeFilter.jsx';
import { navigationService } from '../../../../common/services/navigationService';
import { constants } from '../../../../common/services/constants';
import { utilities } from '../../../../common/services/utilities';
import { nodeFilterConfigService } from '../../../../common/services/nodeFilterConfigService';
import { systemConfigService } from '../../../../common/services/systemConfigService';

class NodeConfigurationReport extends React.Component {
    constructor() {
        super();
        this.filterCategories = this.filterCategories.bind(this);
        this.filterElements = this.filterElements.bind(this);
        this.onSubmitFilter = this.onSubmitFilter.bind(this);
    }

    filterCategories(allCategories) {
        return allCategories.filter(x => x.categoryId === 2 || x.categoryId === 8);
    }

    filterElements(categoryElements, selectedCategory) {
        return categoryElements.filter(x => !utilities.isNullOrUndefined(selectedCategory) && x.categoryId === selectedCategory.categoryId && x.isActive);
    }

    onSubmitFilter(formValues) {
        const filters = Object.assign({}, formValues, {
            categoryName: formValues.category.name,
            elementName: formValues.element.name,
            reportType: constants.Report.NodeConfigurationReport
        });
        this.props.nodeConfigurationReportSaveReportFilter(filters);
        navigationService.navigateTo('view');
    }

    getReportRequest(values) {
        const executionId = systemConfigService.getSessionId();
        const data = {
            category: values.category.name,
            element: values.element.name,
            executionId
        };
        return data;
    }

    getConfig() {
        const functions = {
            filterCategories: this.filterCategories,
            filterElements: this.filterElements,
            onSubmitFilter: this.onSubmitFilter,
            getReportRequest: this.getReportRequest
        };
        return nodeFilterConfigService.getNodeConfigurationConfig(functions);
    }

    render() {
        return (
            <NodeFilter config={this.getConfig()} />
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = () => {
    return {
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        nodeConfigurationReportSaveReportFilter: formValues => {
            dispatch(nodeConfigurationReportSaveReportFilter(formValues));
        }
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(NodeConfigurationReport);


