import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm, change, SubmissionError, formValueSelector, untouch, reset } from 'redux-form';
import { required } from 'redux-form-validators';
import { inputSelect, inputDatePicker, inputAutocomplete, RadioButtonGroup } from '../../../../common/components/formControl/formControl.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import {
    cutOffReportFilterOnSelectCategory, requestSearchNodes, cutOffReportResetFields,
    cutOffReportFilterOnSelectElement, saveCutOffReportFilter, clearSearchNodes,
    requestFinalTicket, requestNonOperationalSegmentOwnership, requestOperationalDataWithoutCutOff,
    refreshStatus, clearStatus, requestOperationalDataWithoutCutoffStatus, navigateToReportsGrid, clearSelectedNode
} from '../actions';
import { navigationService } from '../../../../common/services/navigationService';
import { dateService } from '../../../../common/services/dateService';
import { utilities } from '../../../../common/services/utilities';
import {
    showError, showWarning, hideNotification, getCategories,
    getCategoryElements, showLoader, hideLoader, openMessageModal
} from '../../../../common/actions';
import { optionService } from '../../../../common/services/optionService';
import { constants } from '../../../../common/services/constants';
import { systemConfigService } from '../../../../common/services/systemConfigService';
import SectionFooter from '../../../../common/components/footer/sectionFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';
// import { asyncValidate } from '../asyncValidate';

export class CutoffReportFilter extends React.Component {
    constructor() {
        super();

        this.onSubmit = this.onSubmit.bind(this);
        this.onToggleReport = this.onToggleReport.bind(this);
        this.onSelected = this.onSelected.bind(this);
        this.getTicket = this.getTicket.bind(this);
        this.validate = this.validate.bind(this);
        this.getCategories = this.getCategories.bind(this);
        this.getElements = this.getElements.bind(this);
        this.showModal = this.showModal.bind(this);
        this.getReportOptions = this.getReportOptions.bind(this);
    }

    isCutOffModule() {
        return utilities.equalsIgnoreCase(navigationService.getModulePath(), constants.ReportType.BeforeCutOff);
    }

    onToggleReport(reportType) {
        if (this.isCutOffModule() && reportType === constants.Report.WithoutCutoff) {
            this.props.showWarning(resourceProvider.read('operationalReportLoadingTitle'), resourceProvider.read('operationalReportLoadingMessage'));
        } else if (!this.isCutOffModule()) {
            this.props.showWarning(resourceProvider.read('operativeBalanceReportLoadingTitle'), resourceProvider.read('operativeBalanceReportLoadingMessage'));
        } else {
            this.props.hideError();
        }

        this.props.resetField('initialDate');
        this.props.resetField('finalDate');

        if (this.props.type === constants.ReportType.OperativeBalance) {
            this.props.clearSearchNodes();
            this.props.clearNode();
            this.props.resetNode();
        }

        this.getTicket(reportType);
    }

    getTicket(reportType, value) {
        const element = value || this.props.selectedElement;
        if (element && reportType !== constants.Report.WithoutCutoff && this.isCutOffModule()) {
            const ticketTypeId = reportType === constants.Report.WithOwner ? constants.CalculationType.Ownership : constants.CalculationType.Cutoff;
            this.props.requestFinalTicket(element, ticketTypeId);
        }
    }

    onSelected(value, category) {
        this.props.onSelected(value, category);

        if (category) {
            this.props.resetField('element');
        }

        this.props.resetField('initialDate');
        this.props.resetField('finalDate');
        this.props.clearSearchNodes();

        if (!category) {
            this.getTicket(this.props.reportType, value);
        }
    }

    validate(formValues) {
        const initialDate = dateService.parseToDate(formValues.initialDate);
        const finalDate = dateService.parseToDate(formValues.finalDate);
        const today = dateService.today();
        const difference = dateService.getDiff(formValues.finalDate, formValues.initialDate, 'd');
        const dateRange = systemConfigService.getDefaultUnbalanceReportValidDays();

        if (initialDate > finalDate) {
            const error = resourceProvider.read('DATES_INCONSISTENT');
            this.props.showError(error);
            throw new SubmissionError({
                _error: error
            });
        }

        if (finalDate >= today) {
            const error = resourceProvider.read('ENDDATE_BEFORENOWVALIDATION');
            this.props.showError(error);
            throw new SubmissionError({
                _error: error
            });
        }

        if (difference >= dateRange) {
            const error = resourceProvider.readFormat('MAXIMUM_DAYS_FOR_PERIOD_RANGEVALIDATION', [dateRange]);
            this.props.showError(error);
            throw new SubmissionError({
                _error: error
            });
        }

        if (utilities.isNullOrUndefined(formValues.node.name)) {
            throw new SubmissionError({
                node: resourceProvider.read('required')
            });
        }
    }

    onSubmit(formValues) {
        this.validate(formValues);

        const startDate = dateService.parseToDate(formValues.initialDate);
        const endDate = dateService.parseToDate(formValues.finalDate);

        // To navigate to before cutoff for all nodes
        if (formValues.node.name === constants.Todos && utilities.equalsIgnoreCase(formValues.reportType, constants.Report.WithOwner) && this.isCutOffModule()) {
            formValues.reportType = constants.Report.BalanceOperativeWithPropertyReport;
        }

        // To save execution status for operative balance with ownership
        if (utilities.equalsIgnoreCase(formValues.reportType, constants.Report.WithOwner) && !this.isCutOffModule()) {
            formValues.reportType = constants.Report.NonSonWithOwnerReport;
        }

        const filters = Object.assign({}, formValues, {
            categoryName: formValues.category.name,
            elementName: formValues.element.name,
            nodeName: formValues.node.name,
            nodeId: formValues.node.nodeId,
            reportType: formValues.reportType,
            initialDate: dateService.convertToColombian(formValues.initialDate),
            finalDate: dateService.convertToColombian(formValues.finalDate)
        });

        // On the fly reports, trigger execution and save filters
        // navigation to power BI will not happen
        if (formValues.reportType === constants.Report.WithoutCutoff || formValues.reportType === constants.Report.NonSonWithOwnerReport) {
            const reportTypeId = this.isCutOffModule() ? constants.ReportTypeName.BeforeCutOff : constants.ReportTypeName.OperativeBalance;
            const scenarioId = this.isCutOffModule() ? 'OPERATIONAL' : 'OFFICER';
            const name = formValues.reportType === constants.Report.WithoutCutoff ? 'WithoutCutoff' : 'NonSonWithOwnerReport';

            const data = {
                categoryId: formValues.category.categoryId,
                elementId: formValues.element.elementId,
                nodeId: formValues.node.nodeId,
                startDate,
                endDate,
                reportTypeId,
                scenarioId,
                name
            };

            this.props.saveExecution(data, !utilities.equalsIgnoreCase(formValues.reportType, constants.Report.NonSonWithOwnerReport));
            this.props.saveCutOffReportFilter(filters);
            return;
        }


        // analysis server reports, save filters and navigate to power BI
        this.props.saveCutOffReportFilter(filters);
        navigationService.navigateTo('manage/view');
        return;
    }

    getDateProps() {
        const dateProps = {};
        const currentTicket = this.props.ticket;

        if (this.props.reportType === constants.Report.WithoutCutoff || !this.isCutOffModule()) {
            dateProps.maxDate = dateService.subtract(dateService.now(), 1, 'd').toDate();
        } else if (this.props.selectedElement && currentTicket && this.props.reportType !== constants.Report.WithoutCutoff) {
            const ticketEndDate = currentTicket.endDate;
            const date = dateService.format(ticketEndDate);
            const difference = dateService.getDiff(dateService.nowAsString(), date, 'd');
            dateProps.maxDate = dateService.subtract(dateService.now(), difference, 'd').toDate();
        } else {
            dateProps.minDate = dateService.now().toDate();
            dateProps.maxDate = dateService.subtract(dateService.now(), 1, 'd').toDate();
        }

        return dateProps;
    }

    getReportTypes() {
        const reportTypes = this.isCutOffModule() ? optionService.getReportTypes() : optionService.getOperativeBalanceReportTypes();
        return reportTypes.map(option => {
            return { label: resourceProvider.read(option.label), value: option.value };
        });
    }

    getCategories() {
        return this.props.allCategories.filter(x => x.categoryId === constants.Category.Segment || x.categoryId === constants.Category.System);
    }

    getElements() {
        if (utilities.isNullOrUndefined(this.props.selectedCategory)) {
            return [];
        }

        // When system is selected, filter all active system elements
        if (this.props.selectedCategory.categoryId === constants.Category.System) {
            return this.props.categoryElements.filter(x => x.categoryId === constants.Category.System && x.isActive);
        }

        // For cutoff module, return operational segments
        if (this.isCutOffModule()) {
            return this.props.operationalSegments;
        }

        // Non operational segments
        return this.props.categoryElements.filter(x => x.categoryId === constants.Category.Segment && x.isActive && !x.isOperationalSegment);
    }

    getItems() {
        const nodes = [...(this.props.searchedNodes || [])];
        if (this.props.reportType === constants.Report.WithOwner && !this.isCutOffModule()) {
            return nodes;
        }
        return [{ name: constants.Todos }, ...nodes];
    }

    showModal(titleKey, messageKey) {
        this.props.showModal(resourceProvider.read(messageKey), {
            canCancel: true,
            cancelActionTitle: 'toClose',
            acceptActionTitle: 'goToGeneratedReports',
            title: resourceProvider.read(titleKey),
            acceptActionAndClose: navigateToReportsGrid(),
            cancelAction: [reset(this.props.type), clearSearchNodes(), clearSelectedNode(true)],
            closeAction: [reset(this.props.type), clearSearchNodes(), clearSelectedNode(true)]
        });
    }

    getReportOptions() {
        return (
            <div className="ep-control-group m-b-3">
                <Field id="r_reportFilter_type" className="ep-radio-toggler--list" component={RadioButtonGroup}
                    options={this.getReportTypes()} isSame={true} canUpdate={true} defaultValue={this.props.initialValues.reportType}
                    disabled={false} name="reportType" onChange={this.onToggleReport} />
            </div>
        );
    }

    render() {
        return (
            <section className="ep-content">
                <div className="ep-content__body">
                    <div className="ep-section ep-section--panel">
                        <form className="full-height" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                            <div className="ep-section__body ep-section__body--h">
                                <section className="ep-section__content-w300 m-t-6">
                                    <h2 className="ep-control-group-title">{resourceProvider.read('selectCriteria')}</h2>
                                    <div className="ep-control-group">
                                        <label id="lbl_reportFilter_category" className="ep-label" htmlFor="dd_reportFilter_category_sel">{resourceProvider.read('category')}</label>
                                        <Field id="dd_reportFilter_category" component={inputSelect} name="category" inputId="dd_reportFilter_category_sel"
                                            options={this.getCategories()} getOptionLabel={x => x.name} getOptionValue={x => x.categoryId}
                                            onChange={e => this.onSelected(e, true)} validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                                    </div>
                                    <div className="ep-control-group">
                                        <label id="lbl_reportFilter_element" className="ep-label" htmlFor="dd_reportFilter_element_sel">{resourceProvider.read('element')}</label>
                                        <Field id="dd_reportFilter_element" component={inputSelect} name="element" inputId="dd_reportFilter_element_sel"
                                            options={this.getElements()} onChange={e => this.onSelected(e, false)} getOptionLabel={x => x.name}
                                            getOptionValue={x => x.elementId} disabled={utilities.isNullOrUndefined(this.props.selectedCategory)}
                                            validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                                    </div>
                                    {!this.isCutOffModule() && this.getReportOptions()}
                                    <div className="ep-control-group">
                                        <label id="lbl_reportFilter_node" className="ep-label">{resourceProvider.read('node')}</label>
                                        <Field type="text" id="txt_reportFilter_node" component={inputAutocomplete} name="node"
                                            onSelect={this.props.onSelectNode} shouldChangeValueOnSelect={true} clear={this.props.clearSelectedNode}
                                            onChange={e => this.props.requestSearchNodes(this.props.selectedElement, e)}
                                            shouldItemRender={(item, value) => item.name === constants.Todos || item.name.toLowerCase().indexOf(value.toLowerCase()) > -1}
                                            renderItem={(item, isHighlighted) =>
                                                (<div key={item.name} style={{ padding: '10px 12px', background: isHighlighted ? '#eee' : '#fff' }}>
                                                    {item.name}
                                                </div>)
                                            }
                                            items={this.getItems()} getItemValue={n => n.name}
                                            validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                                    </div>
                                    {this.isCutOffModule() && this.getReportOptions()}
                                    <div className="ep-control-group">
                                        <label id="lbl_reportFilter_initialDate" className="ep-label" htmlFor="dt_reportFilter_initialDate">{resourceProvider.read('initialDate')}</label>
                                        <Field id="dt_reportFilter_initialDate"
                                            component={inputDatePicker} name="initialDate"
                                            validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                                    </div>
                                    <div className="ep-control-group">
                                        <label id="lbl_reportFilter_finalDate" className="ep-label" htmlFor="dt_reportFilter_finalDate">{resourceProvider.read('finalDate')}</label>
                                        <Field id="dt_reportFilter_finalDate"
                                            component={inputDatePicker} name="finalDate" {...this.getDateProps()}
                                            validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                                    </div>
                                    <SectionFooter floatRight={false} config={footerConfigService.getAcceptConfig('reportFilter',
                                        { acceptClassName: 'ep-btn', acceptText: 'seeReport' })} />
                                </section>
                            </div>
                        </form>
                    </div>
                </div>
            </section>
        );
    }

    componentDidMount() {
        if (!this.isCutOffModule()) {
            this.props.showWarning(resourceProvider.read('operativeBalanceReportLoadingTitle'), resourceProvider.read('operativeBalanceReportLoadingMessage'));
        }

        this.props.getCategories();
        this.props.getCategoryElements();
        this.props.cutOffReportResetFields();
    }

    componentDidUpdate(prevProps) {
        if (prevProps.reportToggler !== this.props.reportToggler) {
            this.showModal('generateBalance', 'generateReportRequestReceived');
        }

        if (prevProps.errorSaveCutOffToggler !== this.props.errorSaveCutOffToggler) {
            this.showModal('existingOperativeBalance', 'reportAlreadyProcessing');
        }

        if (prevProps.navigateToggler !== this.props.navigateToggler) {
            const module = this.isCutOffModule() ? 'generatedreport/manage' : 'generatedsupplychainreport/manage';
            navigationService.navigateToModule(module);
        }

        if (prevProps.clearSelectedNodeToggler !== this.props.clearSelectedNodeToggler) {
            this.props.resetClear();
        }
    }

    componentWillUnmount() {
        this.props.clearStatus();
        this.props.hideError();
    }
}

/* istanbul ignore next */
const mapStateToProps = (state, ownProps) => {
    const reportType = formValueSelector(ownProps.type)(state, 'reportType');
    return {
        form: ownProps.type,
        categoryElements: state.shared.categoryElements,
        operationalSegments: state.shared.operationalSegments,
        allCategories: state.shared.allCategories,
        selectedCategory: state.report.cutOffReport.selectedCategory,
        selectedElement: state.report.cutOffReport.selectedElement,
        searchedNodes: state.report.cutOffReport.searchedNodes,
        ticket: state.report.cutOffReport.ticket,
        segmentToggler: state.report.cutOffReport.segmentToggler,
        reportToggler: state.report.cutOffReport.reportToggler,
        executionId: state.report.cutOffReport.executionId,
        status: state.report.cutOffReport.status,
        receiveStatusToggler: state.report.cutOffReport.receiveStatusToggler,
        refreshStatusToggler: state.report.cutOffReport.refreshStatusToggler,
        navigateToggler: state.report.cutOffReport.navigateToggler,
        clearSelectedNode: state.report.cutOffReport.clearSelectedNode,
        clearSelectedNodeToggler: state.report.cutOffReport.clearSelectedNodeToggler,
        initialValues: {
            reportType: ownProps.type === constants.ReportType.OperativeBalance ?
                constants.Report.WithoutCutoff :
                constants.Report.WithOwner
        },
        reportType,
        errorSaveCutOffToggler: state.report.cutOffReport.errorSaveCutOffToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    return {
        getCategories: () => {
            dispatch(getCategories());
        },
        getCategoryElements: () => {
            dispatch(getCategoryElements());
        },
        onSelected: (value, category) => {
            if (category === true) {
                dispatch(cutOffReportFilterOnSelectCategory(value));
            } else {
                dispatch(cutOffReportFilterOnSelectElement(value));
            }
        },
        requestSearchNodes: (selectedElement, searchText) => {
            if (!selectedElement || selectedElement.length === 0 || utilities.isNullOrWhitespace(searchText)) {
                dispatch(clearSearchNodes());
            } else {
                dispatch(requestSearchNodes(selectedElement.elementId, searchText));
            }
        },
        cutOffReportResetFields: () => {
            dispatch(cutOffReportResetFields());
        },
        clearSearchNodes: () => {
            dispatch(clearSearchNodes());
        },
        onSelectNode: node => {
            dispatch(change(ownProps.type, 'node', node));
        },
        saveCutOffReportFilter: formValues => {
            dispatch(saveCutOffReportFilter(formValues));
        },
        showError: message => {
            dispatch(showError(message));
        },
        hideError: () => {
            dispatch(hideNotification());
        },
        requestFinalTicket: (element, ticketTypeId) => {
            dispatch(requestFinalTicket(element.categoryId, element.elementId, ticketTypeId));
        },
        resetField: fieldName => {
            dispatch(change(ownProps.type, fieldName, null));
            dispatch(untouch(ownProps.type, fieldName));
        },
        showWarning: (message, title) => {
            dispatch(showWarning(message, false, title));
        },
        requestOperationalDataWithoutCutoffStatus: executionId => {
            dispatch(requestOperationalDataWithoutCutoffStatus(executionId));
        },
        saveExecution: (data, isCutOff) => {
            if (isCutOff) {
                dispatch(requestOperationalDataWithoutCutOff(data));
            } else {
                dispatch(requestNonOperationalSegmentOwnership(data));
            }
        },
        refreshStatus: () => {
            dispatch(refreshStatus());
        },
        clearStatus: () => {
            dispatch(clearStatus());
        },
        showLoader: () => {
            dispatch(showLoader());
        },
        hideLoader: () => {
            dispatch(hideLoader());
        },
        showModal: (message, opts) => {
            dispatch(openMessageModal(message, opts));
        },
        resetClear: () => {
            dispatch(clearSelectedNode(false));
        },
        clearNode: () => {
            dispatch(clearSelectedNode(true));
        },
        resetNode: () => {
            dispatch(change(ownProps.type, 'node', {}));
            dispatch(untouch(ownProps.type, 'node'));
        }
    };
};

/* istanbul ignore next */
const CutoffReportFilterForm = reduxForm()(CutoffReportFilter);

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(CutoffReportFilterForm);
