import React from 'react';
import { connect } from 'react-redux';
import { reduxForm, Fields } from 'redux-form';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { InputDateRange } from '../../../../common/components/formControl/formControl.jsx';
import { required } from 'redux-form-validators';
import { utilities } from '../../../../common/services/utilities.js';
import { dateService } from '../../../../common/services/dateService.js';
import { hideNotification, showError } from '../../../../common/actions.js';
import { setLogisticsInfo, getLogisticsValidationData, getLogisticsOfficialPeriods, setIsPeriodValid } from './../../../transportBalance/logistics/actions';
import { systemConfigService } from '../../../../common/services/systemConfigService';
import { constants } from './../../../../common/services/constants';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';

class OfficialLogisticsPeriod extends React.Component {
    constructor() {
        super();
        this.onSubmit = this.onSubmit.bind(this);
        this.onYearSelect = this.onYearSelect.bind(this);
    }

    onSubmit(formValues) {
        const startDate = dateService.parseFieldToISOString(formValues.periods.start);
        const endDate = dateService.parseFieldToISOString(formValues.periods.end);

        const logisticsInfo = Object.assign({}, formValues, {
            startDate,
            endDate,
            segmentId: this.props.logisticsInfo.segmentId,
            segmentName: this.props.logisticsInfo.segmentName,
            owner: this.props.logisticsInfo.owner,
            node: this.props.logisticsInfo.node
        });

        const ticket = {
            startDate: startDate,
            endDate: endDate,
            categoryElementId: this.props.logisticsInfo.segmentId,
            nodeId: this.props.logisticsInfo.node.nodeId,
            categoryElementName: this.props.logisticsInfo.segmentName,
            ownerId: this.props.logisticsInfo.owner[0].elementId
        };

        this.props.getLogisticsValidationData(ticket);

        this.props.setLogisticsInfo(logisticsInfo);
        this.props.hideError();
    }

    onYearSelect(val) {
        if (val.selectedYear && val.selectedMonths.length === 0) {
            this.props.showError(resourceProvider.read('segmentWithoutDeltaCalculationForYearError'), resourceProvider.read('segmentWithoutDeltaCalculationForYearTitle'));
            this.props.setIsPeriodValid(false);
        }

        if (val.selectedMonths.length > 0) {
            this.props.hideError();
            this.props.setIsPeriodValid(true);
        }
    }

    render() {
        const dateRange = !utilities.isNullOrUndefined(this.props.dateRange) ? this.props.dateRange.officialPeriods : [];
        const defaultYear = !utilities.isNullOrUndefined(this.props.dateRange) ? this.props.dateRange.defaultYear : null;
        return (
            <form id="frm_logisticsPeriod" className="ep-form" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                <section className="ep-modal__content">
                    <h2 className="ep-control-group-title">{resourceProvider.read('selectProcessingPeriod')}</h2>
                    <div className="ep-control-group">
                        <Fields id="dr_logisticsPeriod_period" component={InputDateRange} names={['periods', 'year']}
                            dateRange={dateRange} onYearSelect={this.onYearSelect} defaultYear={defaultYear}
                            validate={{ periods: [required({ msg: { presence: resourceProvider.read('required') } })] }} />
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('logisticsPeriod', { acceptText: 'next', disableAccept: !this.props.isPeriodValid, onAccept: this.props.onWizardNext })} />
            </form>
        );
    }

    componentDidMount() {
        this.props.getLogisticsOfficialPeriods(this.props.logisticsInfo.segment.elementId, systemConfigService.getDefaultOfficialSivTicketDateRange());
    }

    componentDidUpdate(prevProps) {
        if (prevProps.validationDataToggler !== this.props.validationDataToggler) {
            this.props.onNext(this.props.config.wizardName);
        }
    }

    componentWillUnmount() {
        this.props.setIsPeriodValid(true);
    }
}

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    const name = constants.LogisticsType[ownProps.componentType];
    return {
        hideError: () => {
            dispatch(hideNotification());
        },
        showError: (message, title) => {
            dispatch(showError(message, true, title));
        },
        setLogisticsInfo: logisticsInfo => {
            dispatch(setLogisticsInfo(logisticsInfo, name));
        },
        getLogisticsValidationData: ticket => {
            dispatch(getLogisticsValidationData(ticket, name, true));
        },
        setIsPeriodValid: isPeriodValid => {
            dispatch(setIsPeriodValid(isPeriodValid, name));
        },
        getLogisticsOfficialPeriods: (segmentId, years) => {
            dispatch(getLogisticsOfficialPeriods(segmentId, years, name));
        }
    };
};

/* istanbul ignore next */
const mapStateToProps = (state, ownProps) => {
    const name = constants.LogisticsType[ownProps.componentType];
    return {
        logisticsInfo: state.logistics[name].logisticsInfo,
        validationDataToggler: state.logistics[name].validationDataToggler,
        dateRange: state.logistics[name].periods,
        isPeriodValid: state.logistics[name].isPeriodValid
    };
};

/* istanbul ignore next */
const OfficialLogisticsPeriodForm = reduxForm({ form: 'officialLogisticsPeriod', enableReinitialize: true })(OfficialLogisticsPeriod);
export default connect(mapStateToProps, mapDispatchToProps)(OfficialLogisticsPeriodForm);
