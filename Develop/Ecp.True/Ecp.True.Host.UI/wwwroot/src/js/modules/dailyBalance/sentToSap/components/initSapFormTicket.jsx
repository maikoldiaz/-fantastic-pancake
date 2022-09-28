import React from 'react';
import { connect } from 'react-redux';
import { Field, Fields, FieldArray, reduxForm, change, untouch, reset } from 'redux-form';
import { required, length } from 'redux-form-validators';
import { utilities } from '../../../../common/services/utilities';
import { dateService } from '../../../../common/services/dateService';
import { inputSelect, inputDatePicker, InputAutocompleteChipFilter, InputDateRange } from '../../../../common/components/formControl/formControl.jsx';
import { hideNotification, getCategoryElements, showError, showLoader, hideLoader } from '../../../../common/actions';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { constants } from '../../../../common/services/constants';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import SectionFooter from '../../../../common/components/footer/sectionFooter.jsx';
import { sapFormTicketValidator } from '../services/sapFormTicketValidationService';
import {
    getOfficialPeriods,
    resetOfficialPeriods,
    clearSearchNodes,
    requestSearchNodes,
    resetSapFromData,
    setSapTicketInfo,
    receiveSapTicketValidations
} from '../actions';

export class InitSapFormTicket extends React.Component {
    constructor() {
        super();
        this.onSubmit = this.onSubmit.bind(this);
        this.officialPeriodValidate = this.officialPeriodValidate.bind(this);
        this.requestSearchNodes = this.requestSearchNodes.bind(this);

        this.state = {
            cleanChangeToggler: false
        };
    }

    async onSubmit(formValues) {
        await sapFormTicketValidator.validateAsync(this.props, formValues);

        const isOperationalScenario = formValues.scenario.value === constants.ScenarioTypeId.operational;

        const startDate = isOperationalScenario
            ? dateService.parseToDate(formValues.initialDate)
            : dateService.parseToDate(formValues.periods.start);

        const endDate = isOperationalScenario
            ? dateService.parseToDate(formValues.finalDate)
            : dateService.parseToDate(formValues.periods.end);

        const nodes = formValues.nodes
            .filter(node => node.nodeId !== 0)
            .map(node => ({ nodeId: node.nodeId }));

        const ticket = {
            ticketTypeId: constants.TicketType.LogisticMovements,
            startDate,
            endDate,
            categoryElementId: formValues.segment.elementId,
            categoryElementName: formValues.segment.name,
            scenarioTypeId: formValues.scenario.value,
            ownerId: formValues.owner.elementId,
            ticketNodes: nodes
        };

        this.props.hideError();
        this.props.setSapTicketInfo(ticket);
        this.props.onNext(this.props.config.wizardName);
    }

    officialPeriodValidate(noMonths) {
        if (noMonths.selectedYear && noMonths.selectedMonths.length === 0) {
            this.props.showError(resourceProvider.read('officialDeltaNoPeriodForOneSegment'), resourceProvider.read('officialDeltaCannotContinue'));
        }

        if (noMonths.selectedMonths.length > 0) {
            this.props.hideError();
        }
    }

    isOfficialScenario() {
        return this.props.scenario ? this.props.scenario.value === constants.ScenarioTypeId.official : false;
    }

    getSegmentList() {
        return this.isOfficialScenario()
            ? this.props.categoryElements.filter(obj => obj.isActive && obj.categoryId === 2)
            : this.props.operationalSegments;
    }

    getOwnerList() {
        return this.props.categoryElements
            .filter(a => a.elementId === 27 || a.elementId === 30)
            .reverse();
    }

    getScenarioList() {
        return Object
            .entries(constants.ScenarioTypeId)
            .filter(x => x[1] !== 3)
            .map(x => ({ name: resourceProvider.read(x[0]), value: x[1] }));
    }

    requestSearchNodes(searchText) {
        if (utilities.isNullOrWhitespace(searchText)) {
            this.props.clearSearchedNodes();
        } else {
            this.props.requestSearchNodes(this.props.segment, searchText);
        }
    }

    resetOfficialPeriod() {
        if (this.isOfficialScenario()) {
            this.props.resetOfficialTicket();
            this.props.resetField('periods');
            this.props.resetField('year');
            if (this.props.segment) {
                this.props.requestLastOfficialTicket(
                    this.props.segment.elementId,
                    this.props.initSapFormTicket.officialPeriodsYearsRange
                );
            }
        }
    }

    resetFilterNode() {
        const resetNodes = this.props.selectedNodes.filter(node => node.nodeId === 0);

        this.props.clearSearchedNodes();
        if (this.props.selectedNodes.length > 0) {
            this.props.resetField('nodes', resetNodes);
        }
    }

    render() {
        const scenarioList = this.getScenarioList();
        const segmentList = this.getSegmentList();
        const ownerList = this.getOwnerList();

        return (
            <form className="full-height" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                <div className="ep-section__body ep-section__body--auto">
                    <div className="row">
                        <div className="col-md-4 p-x-8 p-t-6 col-md-offset-2">
                            <div className="ep-control-group">
                                <label id="lbl_sapProcess_scenario" className="ep-label" htmlFor="dd_sapProcess_scenario_sel">{resourceProvider.read('stage')}</label>
                                <Field id="dd_sapProcess_scenario" component={inputSelect} name="scenario" options={scenarioList}
                                    getOptionLabel={x => x.name} getOptionValue={x => x.value} inputId="dd_sapProcess_scenario_sel"
                                    validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                            </div>
                            <div className="ep-control-group">
                                <label id="lbl_sapProcess_segment" className="ep-label" htmlFor="dd_sapProcess_segment_sel">{resourceProvider.read('segment')}</label>
                                <Field id="dd_sapProcess_segment" component={inputSelect} name="segment" options={segmentList}
                                    getOptionLabel={x => x.name} getOptionValue={x => x.elementId} inputId="dd_sapProcess_segment_sel"
                                    validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                            </div>
                            {this.isOfficialScenario() && <div className="ep-control-group">
                                <Fields id="dd_sapProcess_period" component={InputDateRange} names={['periods', 'year']}
                                    dateRange={this.props.officialPeriodsTicket.officialPeriods} onYearSelect={this.officialPeriodValidate} defaultYear={this.props.officialPeriodsTicket.defaultYear}
                                    dateLbl={resourceProvider.read('period')}
                                    validate={{ periods: [required({ msg: { presence: resourceProvider.read('required') } })] }} />
                            </div>}
                            {!this.isOfficialScenario() && <React.Fragment>
                                <div className="ep-control-group">
                                    <label id="lbl_sapProcess_initialDate" className="ep-label" htmlFor="dt_sapProcess_initialDate">{resourceProvider.read('initialDate')}</label>
                                    <Field id="dt_sapProcess_initialDate" component={inputDatePicker} name="initialDate"
                                        maxDate={dateService.subtract(dateService.now(), 1, 'd').toDate()}
                                        validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                                </div>
                                <div className="ep-control-group">
                                    <label id="lbl_sapProcess_finalDate" className="ep-label" htmlFor="dt_sapProcess_finalDate">{resourceProvider.read('finalDate')}</label>
                                    <Field id="dt_sapProcess_finalDate" component={inputDatePicker} name="finalDate"
                                        isDisabled={!this.props.initialDate}
                                        maxDate={dateService.subtract(dateService.now(), 1, 'd').toDate()}
                                        validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                                </div>
                            </React.Fragment>}
                            <div className="ep-control-group">
                                <label id="lbl_sapProcess_owner" className="ep-label" htmlFor="dd_sapProcess_owner_sel">{resourceProvider.read('owner')}</label>
                                <Field id="dd_sapProcess_owner" component={inputSelect} name="owner" options={ownerList}
                                    getOptionLabel={x => x.name} getOptionValue={x => x.elementId} inputId="dd_sapProcess_owner_sel"
                                    validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                            </div>
                        </div>
                        <div className="col-md-4 p-x-0 ep-card-box">
                            <div className="ep-control-group p-t-6 p-x-8">
                                <label id="lbl_sapProcess_node" className="ep-label">{resourceProvider.read('node')}</label>
                                <FieldArray id="txt_sapProcess_node" component={InputAutocompleteChipFilter} name="nodes" idName="nodeId"
                                    searchedItems={this.props.searchedNodes} searchItems={this.requestSearchNodes} hasAllOption={true}
                                    validate={[
                                        required({ msg: { presence: resourceProvider.read('required') } }),
                                        length({ msg: { tooShort: resourceProvider.read('required') }, min: 1 })
                                    ]} />
                            </div>
                        </div>
                    </div>
                    <SectionFooter floatRight={true} config={footerConfigService.getCommonConfig('sapProcess', {
                        onCancel: this.props.resetFields,
                        disableAccept: (this.props.invalid),
                        acceptText: 'next',
                        acceptClassName: 'ep-btn'
                    })} />
                </div>
            </form>
        );
    }

    componentDidMount() {
        this.props.getCategoryElements();
    }

    componentDidUpdate(prevProps) {
        if (prevProps.scenarioChangeToggler !== this.props.scenarioChangeToggler) {
            if (this.props.segment) {
                if (!this.getSegmentList().some(segment => segment.elementId === this.props.segment.elementId)) {
                    this.props.resetField('segment');
                } else {
                    this.resetOfficialPeriod();
                }
            }
        }
        if (prevProps.segmentChangeToggler !== this.props.segmentChangeToggler) {
            this.resetOfficialPeriod();
            this.resetFilterNode();
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    let selectedNodes = [];
    if (state.form.initSapFormTicket) {
        selectedNodes = state.form.initSapFormTicket.values
            ? (state.form.initSapFormTicket.values.nodes || [])
            : [];
    }

    return {
        operationalSegments: state.shared.operationalSegments,
        categoryElements: state.shared.categoryElements,
        scenario: state.sendToSap.scenario,
        segment: state.sendToSap.segment,
        officialPeriodsTicket: state.sendToSap.officialPeriods,
        scenarioChangeToggler: state.sendToSap.scenarioChangeToggler,
        segmentChangeToggler: state.sendToSap.segmentChangeToggler,
        searchedNodes: state.sendToSap.searchedNodes,
        selectedNodes
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        setSapTicketInfo: ticket => {
            dispatch(setSapTicketInfo(ticket));
        },
        getCategoryElements: () => {
            dispatch(getCategoryElements());
        },
        resetField: (fieldName, value) => {
            dispatch(change('initSapFormTicket', fieldName, value));
            dispatch(untouch('initSapFormTicket', fieldName));
        },
        requestLastOfficialTicket: (segmentId, years) => {
            dispatch(getOfficialPeriods(segmentId, years));
        },
        resetOfficialTicket: () => {
            dispatch(resetOfficialPeriods());
        },
        clearSearchedNodes: () => {
            dispatch(clearSearchNodes());
        },
        requestSearchNodes: (selectedElement, searchText) => {
            if (!utilities.isNullOrUndefined(selectedElement)) {
                dispatch(requestSearchNodes(selectedElement.elementId, searchText));
            }
        },
        saveSapTicketValidations: validations => {
            dispatch(receiveSapTicketValidations(validations));
        },
        hideError: () => {
            dispatch(hideNotification());
        },
        hideLoader: () => {
            dispatch(hideLoader());
        },
        showLoader: () => {
            dispatch(showLoader());
        },
        showError: (message, title) => {
            dispatch(showError(message, false, title));
        },
        resetFields: () => {
            dispatch(reset('initSapFormTicket'));
            dispatch(resetSapFromData());
            dispatch(hideNotification());
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(reduxForm({ form: 'initSapFormTicket', enableReinitialize: true })(InitSapFormTicket));
