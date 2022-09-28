import React from 'react';
import { connect } from 'react-redux';
import { change, untouch } from 'redux-form';
import { nodeFilterConfigService } from '../../../../common/services/nodeFilterConfigService';
import NodeFilter from '../../../../common/components/filters/nodeFilter.jsx';
import { utilities } from '../../../../common/services/utilities';
import { resetDateRange } from '../../../../common/actions';
import { onSelectedElement, resetBalancePerNodeFilter, saveOfficialDeltaFilter } from '../actions';
import { navigationService } from '../../../../common/services/navigationService';
import { constants } from '../../../../common/services/constants';
import { dateService } from '../../../../common/services/dateService';

export class OfficialDeltaFilter extends React.Component {
    constructor() {
        super();
        this.onSubmitFilter = this.onSubmitFilter.bind(this);
    }

    onSubmitFilter(formValues) {
        if (Object.keys(formValues.periods).length !== 0) {
            const initialDate = dateService.convertToColombian(formValues.periods.start);
            const finalDate = dateService.convertToColombian(formValues.periods.end);
            const filters = {
                elementId: formValues.element.elementId,
                elementName: formValues.element.name,
                nodeName: formValues.node.name,
                nodeId: formValues.node.nodeId,
                reportType: constants.Report.OfficialBalancePerNodeReport,
                initialDate,
                finalDate
            };
            this.props.saveFilter(filters);
            navigationService.navigateTo('manage/view');
        }
    }

    filterElements(categoryElements) {
        return categoryElements.filter(x => x.categoryId === constants.Category.Segment && x.isActive);
    }

    getConfig() {
        return nodeFilterConfigService.getOfficialNodeBalance(this.filterElements, this.onSubmitFilter);
    }

    render() {
        return (
            <NodeFilter config={this.getConfig()} {...this.props} selectCriteriaKey="selectCriteriaForOfficialBalance" />
        );
    }

    componentDidMount() {
        this.props.resetFilter();
        this.props.resetDateRange();
        this.props.resetField('periods');
    }
}

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        saveFilter: filters => {
            dispatch(saveOfficialDeltaFilter(filters));
        },
        onSelectedElement: element => {
            dispatch(onSelectedElement(element));
        },
        resetFilter: () => {
            dispatch(resetBalancePerNodeFilter());
        },
        resetField: fieldName => {
            dispatch(change('nodeFilter', fieldName, []));
            dispatch(untouch('nodeFilter', fieldName));
        },
        resetDateRange: () => {
            dispatch(resetDateRange());
        }
    };
};

/* istanbul ignore next */
export default connect(null, mapDispatchToProps, utilities.merge)(OfficialDeltaFilter);
