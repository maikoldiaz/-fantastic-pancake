import React from 'react';
import { connect } from 'react-redux';
import { nodeFilterConfigService } from '../../../../common/services/nodeFilterConfigService';
import NodeFilter from '../../../../common/components/filters/nodeFilter.jsx';
import { utilities } from '../../../../common/services/utilities';
import { saveOfficialPendingBalanceFilter, onSelectedElement, resetPendingBalanceFilter } from '../actions';
import { navigationService } from '../../../../common/services/navigationService';
import { constants } from '../../../../common/services/constants';
import { dateService } from '../../../../common/services/dateService';

export class OfficialPendingBalanceFilter extends React.Component {
    constructor() {
        super();
        this.onSubmitFilter = this.onSubmitFilter.bind(this);
    }

    onSubmitFilter(formValues) {
        const initialDate = dateService.parse(dateService.today()).subtract(5, 'hours').subtract(1, 'years').startOf('month');
        const finalDate = dateService.parse(dateService.today()).subtract(5, 'hours').subtract(1, 'months').endOf('month');
        const filters = {
            element: {
                name: formValues.element.name,
                elementId: formValues.element.elementId
            },
            reportType: constants.Report.OfficialPendingBalanceReport,
            initialDate,
            finalDate
        };
        this.props.saveFilter(filters);
        navigationService.navigateTo('view');
    }

    filterElements(categoryElements) {
        return categoryElements.filter(x => x.categoryId === constants.Category.Segment && x.isActive);
    }

    getConfig() {
        return nodeFilterConfigService.getPendingBalance(this.filterElements, this.onSubmitFilter);
    }

    render() {
        return (
            <NodeFilter config={this.getConfig()} {...this.props} selectCriteriaKey="selectCriteriaForOfficialBalance" />
        );
    }

    componentDidMount() {
        this.props.resetFilter();
    }
}
/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        saveFilter: filters => {
            dispatch(saveOfficialPendingBalanceFilter(filters));
        },
        onSelectedElement: element => {
            dispatch(onSelectedElement(element));
        },
        resetFilter: () => {
            dispatch(resetPendingBalanceFilter());
        }
    };
};

export default connect(null, mapDispatchToProps, utilities.merge)(OfficialPendingBalanceFilter);
