import React from 'react';
import { Field, Fields, reduxForm, change, untouch, SubmissionError } from 'redux-form';
import { required } from 'redux-form-validators';
import { connect } from 'react-redux';
import { resourceProvider } from '../../services/resourceProvider';
import {
    nodeFilterOnSelectCategory, nodeFilterRequestSearchNodes, nodeFilterOnSelectElement,
    nodeFilterClearSearchNodes, nodeFilterResetFields, requestTicketNodeStatus, requestEventContractReport,
    showError, showWarning, hideNotification, getCategories, getCategoryElements, setSelectedNode,
    requestNodeConfigurationReport, requestDateRanges, resetDateRange, toggleViewReportButton
} from '../../actions';
import { inputSelect, inputDatePicker, inputAutocomplete, RadioButtonGroup, InputDateRange } from '../../components/formControl/formControl.jsx';
import { dateService } from '../../../common/services/dateService';
import { utilities } from '../../../common/services/utilities';
import { navigationService } from '../../../common/services/navigationService';
import { systemConfigService } from '../../../common/services/systemConfigService';
import { footerConfigService } from '../../services/footerConfigService';
import SectionFooter from '../footer/sectionFooter.jsx';
import { constants } from '../../services/constants';

export class NodeFilter extends React.Component {
    constructor() {
        super();
        this.onSubmit = this.onSubmit.bind(this);
        this.getDateRange = this.getDateRange.bind(this);
        this.onSelectSegment = this.onSelectSegment.bind(this);
        this.requestSearchNodes = this.requestSearchNodes.bind(this);
        this.onSelectCategory = this.onSelectCategory.bind(this);
        this.resetDates = this.resetDates.bind(this);
        this.getReportTypes = this.getReportTypes.bind(this);
        this.onYearSelect = this.onYearSelect.bind(this);
    }

    onSelectSegment(selectedItem) {
        this.props.nodeFilterOnSelectElement(selectedItem);
        this.props.resetField('node');
        this.resetDates();
        this.props.nodeFilterClearSearchNodes();
        if (this.props.config.onSegmentChange) {
            this.props.config.onSegmentChange(selectedItem);
        }
        if (this.props.config.parentPage === 'nodeStatusReport') {
            this.props.config.getTicket(selectedItem);
        }
        if (this.props.config.parentPage === 'officialBalancePerNode') {
            this.getDateRange(selectedItem, true);
        }
        if (this.props.config.parentPage === 'officialBalanceLoaded') {
            this.getDateRange(selectedItem, false);
        }
    }

    onSelectCategory(selectedItem) {
        this.props.nodeFilterOnSelectCategory(selectedItem);
        this.props.resetField('element');
        this.resetDates();
        this.props.nodeFilterClearSearchNodes();
    }

    resetDates() {
        this.props.resetField('initialDate');
        this.props.resetField('finalDate');
    }

    getDateRange(element, isPerNodeReport) {
        this.props.resetAutoField('periods');
        this.props.resetDateRange();
        if (element) {
            this.props.getDateRange(element.elementId, isPerNodeReport);
        }
    }

    isNodeAndViewButtonDisabled() {
        return this.props.viewReportButtonStatusToggler && (
            this.props.config.parentPage === 'officialBalancePerNode' ||
            this.props.config.parentPage === 'officialBalanceLoaded');
    }

    onSubmit(formValues) {
        if (!this.props.config.node.hidden && utilities.isNullOrUndefined(formValues.node.name)) {
            throw new SubmissionError({
                node: resourceProvider.read('required')
            });
        }

        if (this.props.config.parentPage === 'balanceControlChart' ||
            this.props.config.parentPage === 'eventContractReport' ||
            this.props.config.parentPage === 'nodeStatusReport' ||
            this.props.config.parentPage === 'transactionsAuditReport' ||
            this.props.config.parentPage === 'settingsAuditReport') {
            const initialDate = dateService.parseToDate(formValues.initialDate);
            const finalDate = dateService.parseToDate(formValues.finalDate);
            const today = dateService.today();

            if (initialDate > finalDate) {
                const error = resourceProvider.read('DATES_INCONSISTENT');
                this.props.showError(error);
                throw new SubmissionError({
                    _error: error
                });
            }

            if (finalDate >= today && !this.props.config.finalDate.allowAfterNow) {
                const error = resourceProvider.read('ENDDATE_BEFORENOWVALIDATION');
                this.props.showError(error);
                throw new SubmissionError({
                    _error: error
                });
            }
            if (this.props.config.validateDateRange) {
                const error = this.props.config.validateDateRange(initialDate, finalDate);
                if (error) {
                    this.props.showError(error);
                    throw new SubmissionError({
                        _error: error
                    });
                }
            }

            if (this.props.config.getReportRequest) {
                const data = this.props.config.getReportRequest(formValues);
                if (this.props.config.parentPage === 'nodeStatusReport') {
                    this.props.requestTicketNodeStatus(data);
                } else if (this.props.config.parentPage === 'eventContractReport') {
                    this.props.requestEventContractReport(data);
                }
            }
        }

        if (this.props.config.parentPage === 'nodeConfigurationReport') {
            const data = this.props.config.getReportRequest(formValues);
            this.props.requestNodeConfigurationReport(data);
        }

        this.props.config.onSubmitFilter(formValues);
    }

    requestSearchNodes(searchText) {
        if (utilities.isNullOrWhitespace(searchText) || this.props.selectedElement.length === 0) {
            this.props.nodeFilterClearSearchNodes();
        } else {
            this.props.nodeFilterRequestSearchNodes(this.props.selectedElement, searchText);
        }
    }

    buildSearchedNodes(nodes) {
        const searchedNodes = !utilities.isNullOrUndefined(nodes) ? nodes : [];
        const defaultNode = {
            name: constants.Todos
        };

        if (this.props.config.parentPage === 'balanceControlChart' ||
            this.props.config.parentPage === 'officialBalancePerNode' ||
            this.props.config.parentPage === 'officialBalanceLoaded') {
            return [...searchedNodes];
        }
        return [defaultNode, ...searchedNodes];
    }

    getReportTypes() {
        return this.props.config.getReportTypes();
    }

    onYearSelect(data) {
        const status = data.selectedYear && data.selectedMonths.length === 0;

        this.props.toggleViewReportButton(status);
        if (status && this.props.config.parentPage === 'officialBalancePerNode') {
            this.props.showError(resourceProvider.read('segmentWithoutDeltaCalculationForYearError'), resourceProvider.read('segmentWithoutDeltaCalculationForYearTitle'));
        } else if (status && this.props.config.parentPage === 'officialBalanceLoaded') {
            this.props.showError(resourceProvider.read('segmentWithoutOfficialInformationForYearError'), resourceProvider.read('segmentWithoutDeltaCalculationForYearTitle'));
        } else {
            this.props.hideError();
        }
    }

    render() {
        const config = this.props.config;
        const searchedNodes = this.buildSearchedNodes(this.props.searchedNodes);
        const disableNodeAndViewReportButton = this.isNodeAndViewButtonDisabled();
        let categories = this.props.allCategories;
        if (config.category.filterCategoryItems) {
            categories = config.category.filterCategoryItems(this.props.allCategories);
        }

        let elements = this.props.categoryElements.filter(x => !utilities.isNullOrUndefined(this.props.selectedCategory) && x.categoryId === this.props.selectedCategory.categoryId);
        if (config.categoryElement.filterCategoryElementsItem) {
            elements = config.categoryElement.filterCategoryElementsItem(this.props.categoryElements, this.props.selectedCategory);
        }

        let startDateProps = {};
        let endDateProps = {};
        if (this.props.selectedElement) {
            startDateProps = config.getStartDateProps ? config.getStartDateProps(this.props.selectedElement) : {};
            endDateProps = config.getEndDateProps ? config.getEndDateProps(this.props.selectedElement) : {};
        } else if (config.setDateProps) {
            startDateProps = config.getStartDateProps || {};
            endDateProps = config.getEndDateProps || {};
        }

        const nodeName = this.props.config.parentPage === 'officialBalanceLoaded' ||
            this.props.config.parentPage === 'officialBalancePerNode' ? this.props.config.parentPage : 'node';

        const requiredMessage = config.requiredMessage ? config.requiredMessage : resourceProvider.read('required');
        return (
            <section className="ep-content">
                <div className="ep-content__body">
                    <div className="ep-section ep-section--panel">
                        <form className="full-height" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                            <div className="ep-section__body ep-section__body--h">
                                <section className="ep-section__content-w500 m-t-6">
                                    <h2 className="ep-control-group-title">{resourceProvider.read(this.props.selectCriteriaKey || 'selectCriteria')}</h2>

                                    {!config.category.hidden &&
                                        <div className="ep-control-group">
                                            <label id="lbl_nodeFilter_category" className="ep-label" htmlFor="dd_nodeFilter_category_sel">{config.category.label}</label>
                                            <Field id="dd_nodeFilter_category" component={inputSelect} name="category" inputId="dd_nodeFilter_category_sel"
                                                options={categories} getOptionLabel={x => x.name} getOptionValue={x => x.categoryId}
                                                onChange={this.onSelectCategory}
                                                validate={[required({ msg: { presence: requiredMessage } })]} />
                                        </div>}

                                    {!config.categoryElement.hidden &&
                                        <div className="ep-control-group">
                                            <label id="lbl_nodeFilter_element" className="ep-label" htmlFor="dd_nodeFilter_element_sel">{config.categoryElement.label}</label>
                                            <Field id="dd_nodeFilter_element" component={inputSelect} name="element" inputId="dd_nodeFilter_element_sel"
                                                options={elements}
                                                onChange={this.onSelectSegment}
                                                getOptionLabel={x => x.name} getOptionValue={x => x.elementId}
                                                disabled={utilities.isNullOrUndefined(this.props.selectedCategory)}
                                                validate={[required({ msg: { presence: requiredMessage } })]} />
                                        </div>}
                                    {!config.node.hidden &&
                                        <div className="ep-control-group">
                                            <label id="lbl_nodeFilter_node" className="ep-label">{resourceProvider.read('node')}</label>
                                            <Field type="text" id="txt_nodeFilter_node" component={inputAutocomplete} name={nodeName}
                                                onSelect={this.props.onSelectNode} shouldChangeValueOnSelect={true}
                                                onChange={this.requestSearchNodes} clear={this.props.clearSelectedNode}
                                                shouldItemRender={(item, value) => item.name === constants.Todos || item.name.toLowerCase().indexOf(value.toLowerCase()) > -1}
                                                renderItem={(item, isHighlighted) =>
                                                    (<div key={item.name} style={{ padding: '10px 12px', background: isHighlighted ? '#eee' : '#fff' }}>
                                                        {item.name}
                                                    </div>)
                                                }
                                                disabled={disableNodeAndViewReportButton}
                                                items={searchedNodes} getItemValue={n => n.name}
                                                validate={[required({ msg: { presence: requiredMessage } })]} />
                                        </div>}
                                    {!config.reportType.hidden &&
                                        <div className="ep-control-group m-b-3">
                                            <Field id="r_reportFilter_type" className="ep-radio-toggler--list" component={RadioButtonGroup} options={this.getReportTypes()} isSame={true}
                                                defaultValue={this.props.initialValues.reportType} canUpdate={true} disabled={false} name="reportType" onChange={this.onToggleReport} />
                                        </div>
                                    }
                                    {!config.dateRange.hidden &&
                                        <div className="ep-control-group">
                                            <Fields id="txt_nodeFilter_periods" component={InputDateRange} names={['periods', 'year']}
                                                dateLbl={resourceProvider.read('period')}
                                                yearLbl={resourceProvider.read('year')}
                                                onYearSelect={this.onYearSelect}
                                                dateRange={this.props.dateRange}
                                                defaultYear={this.props.defaultYear}
                                                validate={{
                                                    periods: [required({ msg: { presence: requiredMessage } })]
                                                }
                                                }
                                            />
                                        </div>
                                    }
                                    {!config.initialDate.hidden &&
                                        <div className="ep-control-group">
                                            <label id="lbl_nodeFilter_initialDate" className="ep-label" htmlFor="dt_nodeFilter_initialDate">{resourceProvider.read('initialDate')}</label>
                                            <Field id="dt_nodeFilter_initialDate"
                                                component={inputDatePicker} name="initialDate" {...startDateProps}
                                                validate={[required({ msg: { presence: requiredMessage } })]} />
                                        </div>}

                                    {!config.finalDate.hidden &&
                                        <div className="ep-control-group">
                                            <label id="lbl_nodeFilter_finalDate" className="ep-label" htmlFor="dt_nodeFilter_finalDate">{resourceProvider.read('finalDate')}</label>
                                            <Field id="dt_nodeFilter_finalDate"
                                                component={inputDatePicker} name="finalDate" {...endDateProps}
                                                validate={[required({ msg: { presence: requiredMessage } })]} />
                                        </div>}

                                    <SectionFooter floatRight={false} config={footerConfigService.getAcceptConfig('nodeFilter',
                                        {
                                            disableAccept: disableNodeAndViewReportButton,
                                            acceptText: config.submitText, acceptClassName: 'ep-btn'
                                        })} />
                                </section>
                            </div>
                        </form>
                    </div>
                </div>
            </section >
        );
    }

    componentDidMount() {
        this.props.getCategories();
        this.props.getCategoryElements();
        this.props.nodeFilterResetFields();
    }

    componentDidUpdate(prevProps) {
        if (prevProps.reportToggler !== this.props.reportToggler && !this.props.preventOfficialBalanceRedirect) {
            navigationService.navigateTo('view');
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        categoryElements: state.shared.categoryElements,
        allCategories: state.shared.allCategories,
        selectedCategory: state.nodeFilter.selectedCategory,
        selectedElement: state.nodeFilter.selectedElement,
        searchedNodes: state.nodeFilter.searchedNodes,
        reportToggler: state.nodeFilter.reportToggler,
        dateRange: state.nodeFilter.dateRange,
        defaultYear: state.nodeFilter.defaultYear,
        dateRangeToggler: state.nodeFilter.dateRangeToggler,
        viewReportButtonStatusToggler: state.nodeFilter.viewReportButtonStatusToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        getCategories: () => {
            dispatch(getCategories());
        },
        getCategoryElements: () => {
            dispatch(getCategoryElements());
        },
        nodeFilterOnSelectCategory: selectedCategory => {
            dispatch(nodeFilterOnSelectCategory(selectedCategory));
        },
        nodeFilterOnSelectElement: selectedElement => {
            dispatch(nodeFilterOnSelectElement(selectedElement));
        },
        nodeFilterRequestSearchNodes: (selectedElement, searchText) => {
            if (!utilities.isNullOrUndefined(selectedElement)) {
                dispatch(nodeFilterRequestSearchNodes(selectedElement.elementId, searchText));
            }
        },
        nodeFilterClearSearchNodes: () => {
            dispatch(nodeFilterClearSearchNodes());
        },
        onSelectNode: node => {
            dispatch(change('nodeFilter', 'node', node));
            dispatch(setSelectedNode(node));
        },
        resetField: fieldName => {
            dispatch(change('nodeFilter', fieldName, null));
            dispatch(untouch('nodeFilter', fieldName));
        },
        showError: (message, title) => {
            dispatch(showError(message, false, title));
        },
        hideError: () => {
            dispatch(hideNotification());
        },
        showWarning: (message, title) => {
            dispatch(showWarning(message, false, title));
        },
        nodeFilterResetFields: () => {
            dispatch(nodeFilterResetFields());
        },
        requestTicketNodeStatus: data => {
            dispatch(requestTicketNodeStatus(data));
        },
        requestEventContractReport: data => {
            dispatch(requestEventContractReport(data));
        },
        requestNodeConfigurationReport: data => {
            dispatch(requestNodeConfigurationReport(data));
        },
        getDateRange: (elementId, isPerNodeReport) => {
            dispatch(requestDateRanges(elementId, systemConfigService.getDefaultOfficialDeltaReportYears(), isPerNodeReport));
        },
        resetAutoField: fieldName => {
            dispatch(change('nodeFilter', fieldName, []));
            dispatch(untouch('nodeFilter', fieldName));
        },
        resetDateRange: () => {
            dispatch(resetDateRange());
        },
        toggleViewReportButton: status => {
            dispatch(toggleViewReportButton(status));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(reduxForm({ form: 'nodeFilter', enableReinitialize: true })(NodeFilter));
