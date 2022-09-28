import React from 'react';
import { connect } from 'react-redux';
import { reduxForm, Field, change, SubmissionError } from 'redux-form';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import { required } from 'redux-form-validators';
import { inputSelect, RadioButtonGroup, inputAutocomplete } from './../../../../common/components/formControl/formControl.jsx';
import { utilities } from '../../../../common/services/utilities';
import { constants } from './../../../../common/services/constants';
import { dateService } from './../../../../common/services/dateService';
import { getCategoryElements, hideNotification, showError } from './../../../../common/actions';
import {
    clearSearchNodes, onSegmentSelect, requestSearchNodes,
    setLogisticsInfo, getLastOwnershipPerformedDateForSelectedSegment
} from '../actions';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';

export class LogisticsCriteria extends React.Component {
    constructor() {
        super();
        this.onSubmit = this.onSubmit.bind(this);
        this.requestSearchNodes = this.requestSearchNodes.bind(this);
        this.onSelectSegment = this.onSelectSegment.bind(this);
    }

    onSubmit(formValues) {
        this.props.hideError();

        if (utilities.isNullOrUndefined(formValues.node.name)) {
            const error = resourceProvider.read('invalidNode');
            this.props.showError(error);
            throw new SubmissionError({
                _error: error
            });
        }

        const logisticsInfo = Object.assign({}, formValues, {
            segmentId: formValues.segment.elementId,
            segmentName: formValues.segment.name,
            ticketTypeId: constants.TicketType.Logistics,
            owner: this.props.owners.filter(x => x.elementId === formValues.owner),
            node: formValues.node
        });

        if (this.props.componentType === constants.TicketType.Logistics && dateService.isMinDate(this.props.lastOwnershipDate)) {
            const error = resourceProvider.read('CHAIN_WITHOUT_OWNERSHIP_CALCULATION');
            this.props.showError(error);
            throw new SubmissionError({
                _error: error
            });
        }
        this.props.setLogisticsInfo(logisticsInfo);

        this.props.onNext(this.props.config.wizardName);
    }

    onSelectSegment(selectedItem) {
        this.props.hideError();
        this.props.clearSearchedNodes();
        if (utilities.isNullOrUndefined(selectedItem)) {
            return;
        }
        const segmentId = utilities.isNullOrWhitespace(selectedItem.elementId) ? [] : selectedItem.elementId;
        this.props.getLastOwnershipPerformedDateForSelectedSegment(segmentId);
        this.props.onSegmentSelect(selectedItem);
    }

    requestSearchNodes(searchText) {
        if (utilities.isNullOrWhitespace(searchText)) {
            this.props.clearSearchedNodes();
        } else {
            this.props.requestSearchNodes(this.props.selectedSegment, searchText);
        }
    }

    buildSearchedNodes(nodes) {
        const searchedNodes = !utilities.isNullOrUndefined(nodes) ? nodes : [];
        const defaultNode = {
            name: 'Todos'
        };

        return [defaultNode, ...searchedNodes];
    }

    getOwners(owners) {
        const array = owners.map(function (option) {
            return { value: option.elementId, label: utilities.toSentenceCase(option.name) };
        });
        return array;
    }

    render() {
        const searchedNodes = this.buildSearchedNodes(this.props.searchedNodes);

        return (
            <form id="frm_logisticsCriteria" className="ep-form" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                <section className="ep-modal__content">
                    <h2 className="ep-control-group-title">{resourceProvider.read('selectCriteria')}</h2>
                    <div className="ep-control-group">
                        <label className="ep-label" htmlFor="dd_logisticsCriteria_segment_sel">{resourceProvider.read('segment')}</label>
                        <Field id="dd_logisticsCriteria_segment" component={inputSelect} name="segment" inputId="dd_logisticsCriteria_segment_sel"
                            options={this.props.segments} getOptionLabel={x => x.name} getOptionValue={x => x.elementId}
                            onChange={this.onSelectSegment} validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                    </div>
                    <div className="ep-control-group">
                        <label className="ep-label">{resourceProvider.read('node')}</label>
                        <Field type="text" id="txt_logisticsCriteria_node" component={inputAutocomplete} name="node"
                            onSelect={this.props.onSelectNode} shouldChangeValueOnSelect={true}
                            placeholder={resourceProvider.read('select')}
                            onChange={this.requestSearchNodes}
                            shouldItemRender={(item, value) => item.name === 'Todos' || item.name.toLowerCase().indexOf(value.toLowerCase()) > -1}
                            renderItem={(item, isHighlighted) =>
                                (<div style={{ padding: '10px 12px', background: isHighlighted ? '#eee' : '#fff' }}>
                                    {item.name}
                                </div>)
                            }
                            items={searchedNodes} getItemValue={n => n.name}
                            validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                    </div>
                    <div className="ep-control-group">
                        <label className="ep-label">{resourceProvider.read('owner')}</label>
                        <Field name="owner" id="r_logisticsCriteria_owner" component={RadioButtonGroup} options={this.getOwners(this.props.owners)} isSame={true}
                            validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('logisticsCriteria',
                    { onAccept: this.props.onWizardNext, acceptText: 'next' })} />
            </form>
        );
    }

    componentDidMount() {
        this.props.getCategoryElements();
        this.props.hideError();
    }
}

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    const name = constants.LogisticsType[ownProps.componentType];
    return {
        getCategoryElements: () => {
            dispatch(getCategoryElements());
        },
        clearSearchedNodes: () => {
            dispatch(clearSearchNodes(name));
        },
        onSegmentSelect: selectedSegment => {
            dispatch(onSegmentSelect(selectedSegment, name));
        },
        requestSearchNodes: (selectedElement, searchText) => {
            if (!utilities.isNullOrUndefined(selectedElement)) {
                dispatch(requestSearchNodes(selectedElement.elementId, searchText, name));
            }
        },
        onSelectNode: node => {
            dispatch(change('logisticsCriteria', 'node', node));
        },
        setLogisticsInfo: logisticsInfo => {
            dispatch(setLogisticsInfo(logisticsInfo, name));
        },
        getLastOwnershipPerformedDateForSelectedSegment: selectedSegment => {
            dispatch(getLastOwnershipPerformedDateForSelectedSegment(selectedSegment, name));
        },
        showError: message => {
            dispatch(showError(message, true));
        },
        hideError: () => {
            dispatch(hideNotification());
        }
    };
};

/* istanbul ignore next */
const mapStateToProps = (state, ownProps) => {
    const name = constants.LogisticsType[ownProps.componentType];
    return {
        owners: state.shared.categoryElements.filter(a => a.elementId === 27 || a.elementId === 30).reverse(),
        segments: state.shared.groupedCategoryElements[constants.Category.Segment] && state.shared.groupedCategoryElements[constants.Category.Segment].filter(a => a.isActive),
        selectedSegment: state.logistics[name].selectedSegment,
        searchedNodes: state.logistics[name].searchedNodes,
        lastOwnershipDate: state.logistics[name].lastOwnershipDate
    };
};

const LogisticsCriteriaForm = reduxForm({ form: 'logisticsCriteria', enableReinitialize: true })(LogisticsCriteria);

export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(LogisticsCriteriaForm);
