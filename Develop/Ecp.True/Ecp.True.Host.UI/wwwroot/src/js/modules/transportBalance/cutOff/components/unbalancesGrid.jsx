import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../common/components/grid/grid.jsx';
import { gridUtils } from '../../../../common/components/grid/gridUtils';
import { dataGrid } from '../../../../common/components/grid/gridComponent';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { openModal, openMessageModal, showError, wizardSetStep } from '../../../../common/actions';
import { setUnbalances, requestUnbalances, requestSaveTicket, initCutOff, incrementCutOff, intAddComment } from '../actions';
import { navigationService } from '../../../../common/services/navigationService';
import { apiService } from '../../../../common/services/apiService';
import { dateService } from '../../../../common/services/dateService';
import NumberFormatter from '../../../../common/components/formControl/numberFormatter.jsx';
import { utilities } from '../../../../common/services/utilities';
import { batchService } from '../services/batchService';
import SectionFooter from '../../../../common/components/footer/sectionFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import { selectAllGridData, removeGridData, clearSelection } from '../../../../common/components/grid/actions';

export class UnbalancesGrid extends React.Component {
    constructor() {
        super();
        this.getUnbalances = this.getUnbalances.bind(this);
    }

    getValue(row) {
        const text = row.original.unbalancePercentageText;
        if (text === 'Error') {
            return <span className="float-r" style={{ color: 'red', fontWeight: 'bold' }}>{text}</span>;
        }

        return (<NumberFormatter
            className="float-r"
            value={utilities.parseFloat(text)}
            isNumericString={true}
            displayType="text" />);
    }

    getColumns() {
        const columns = [];

        columns.push(gridUtils.buildTextColumn('nodeName', this.props, null, 'node'));
        columns.push(gridUtils.buildTextColumn('productName', this.props, null, 'product'));
        columns.push(gridUtils.buildDecimalColumn('unbalance', this.props, 'unbalance', { type: 'number' }));
        columns.push(gridUtils.buildTextColumn('unitName', this.props, null, 'units'));
        columns.push(gridUtils.buildTextColumn('unbalancePercentageText', this.props, r => this.getValue(r), 'unbalancePercentage'));
        columns.push(gridUtils.buildDecimalColumn('acceptableBalance', this.props, 'acceptableControl', { type: 'number' }));

        return columns;
    }

    getConfirmationMessage() {
        return resourceProvider.read('operationalCutConfirmationMessage')
            .replace('{0}', dateService.capitalize(this.props.operationalCut.ticket.startDate, 'DD MMMM YYYY'))
            .replace('{1}', dateService.capitalize(this.props.operationalCut.ticket.endDate, 'DD MMMM YYYY'))
            .replace('{2}', this.props.operationalCut.ticket.categoryElementName);
    }

    getUnbalances(apiUrl) {
        if (this.props.step === 2) {
            this.props.requestUnbalances(apiUrl, this.props.operationalCut.ticket, this.props.officialMovements, this.props.name, this.props.operationalCut.firstTimeNodes);
            this.props.incrementCutOff();
        }
    }

    render() {
        return (
            <div className="ep-section">
                <header className="ep-section__header">
                    <h1 className="ep-section__title m-t-3">
                        {resourceProvider.read('operationalCutNodeMessage')
                            .replace('{0}', dateService.capitalize(this.props.operationalCut.ticket.startDate))
                            .replace('{1}', dateService.capitalize(this.props.operationalCut.ticket.endDate))
                            .replace('{2}', this.props.operationalCut.ticket.categoryElementName)
                        }
                    </h1>
                    <span className="float-r">
                        {this.props.items.length > 0 && <span className="fw-sb m-r-4"><i className="fas fa-info-circle m-r-2 fs-20" />{resourceProvider.read('manageImbalances')}</span>}
                        <button disabled={this.props.selection.length === 0}
                            id="btn_consistencyCheck_addNote" type="button" className="ep-btn"
                            onClick={() => this.props.onAddNote('unBalancesDetail',
                                resourceProvider.read('hasSelected'),
                                resourceProvider.read('addCommentsOperationalCutNodePostText'),
                                this.props.selection.length)}><i className="far fa-sticky-note m-r-1" /><span className="ep-btn__txt">{resourceProvider.read('addNote')}</span></button>
                    </span>
                </header>
                <div className="ep-section__body ep-section__body--hf p-a-0">
                    <Grid name={this.props.name} columns={this.getColumns()} requestGridData={this.getUnbalances} className="ep-table--row-sm ep-table--wizard" />
                </div>
                <SectionFooter floatRight={true} config={footerConfigService.getCommonConfig('unbalancesGrid',
                    {
                        onAccept: this.props.onConfirmCutoff, cancelActions: [initCutOff, wizardSetStep('operationalCut', 1), selectAllGridData(false, 'unbalances')],
                        disableAccept: (this.props.items.length > 0 || this.props.disableConfirmCutoff),
                        acceptText: 'next', acceptClassName: 'ep-btn', acceptType: 'button'
                    })} />
            </div>
        );
    }

    componentDidUpdate(prevProps) {
        if (this.props.commentToggler !== prevProps.commentToggler) {
            this.props.onSubmit(this.props.selection, this.props.comment, this.props.name);
            this.props.clearSelection('unbalances');
        }
        if (this.props.operationalCut.confirmCutoffToggler !== prevProps.operationalCut.confirmCutoffToggler) {
            if (!batchService.start(this.props.operationalCut)) {
                this.props.saveOperationalCutOff(this.props.operationalCut, batchService.getSessionId());
            }
        }

        if (this.props.operationalCut.batchToggler !== prevProps.operationalCut.batchToggler) {
            if (!batchService.processNext()) {
                this.props.saveOperationalCutOff(this.props.operationalCut, batchService.getSessionId());
            }
        }

        if (this.props.operationalCut.batchFailureToggler !== prevProps.operationalCut.batchFailureToggler) {
            this.props.showError(resourceProvider.read('cutoffAlreadyRunning'));
        }

        if ((prevProps.failureToggler !== this.props.failureToggler) && (!utilities.isNullOrUndefined(this.props.failureToggler))) {
            this.props.confirm(this.props.saveCutOffFailedErrorMessage, resourceProvider.read('error'), false);
            this.props.cancelCutOff(false);
            navigationService.navigateToModule('cutoff/manage');
        }

        if (this.props.operationalCut.receiveToggler !== prevProps.operationalCut.receiveToggler) {
            this.props.cancelCutOff(false);
            navigationService.navigateToModule('cutoff/manage');
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    const props = {
        selection: state.grid.unbalances ? state.grid.unbalances.selection : [],
        comment: state.cutoff.operationalCut.unBalancesDetail ? state.cutoff.operationalCut.unBalancesDetail.comment : null,
        items: state.grid.unbalances ? state.grid.unbalances.items : [],
        operationalCut: state.cutoff.operationalCut,
        step: state.cutoff.operationalCut.step,
        failureToggler: utilities.isNullOrUndefined(state.cutoff.operationalCut.saveCutOffFailureToggler) ? null : state.cutoff.operationalCut.saveCutOffFailureToggler,
        saveCutOffFailed: utilities.isNullOrUndefined(state.cutoff.operationalCut.saveCutOffFailed) ? false : state.cutoff.operationalCut.saveCutOffFailed,
        saveCutOffFailedErrorMessage: state.cutoff.operationalCut.saveCutOffFailedErrorMessage,
        officialMovements: state.cutoff.operationalCut.officialMovements,
        disableConfirmCutoff: state.cutoff.operationalCut.disableConfirmCutoff
    };

    if (state.cutoff.operationalCut.unBalancesDetail) {
        props.commentToggler = state.cutoff.operationalCut.unBalancesDetail.commentToggler;
    }

    return props;
};

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    return {
        cancelCutOff: navigate => {
            dispatch(initCutOff());
            if (navigate) {
                ownProps.goToStep('operationalCut', 1);
            }
            dispatch(selectAllGridData(false, 'unbalances'));
        },
        onAddNote: (name, preText, postText, count) => {
            dispatch(openModal('addComments', '', 'addCommentsOperationalCutNode'));
            dispatch(intAddComment(name, preText, postText, count));
        },
        onSubmit: (items, comment, name) => {
            dispatch(removeGridData(name, items));
            const unbalances = items.map(x => Object.assign({}, x, { comment, node: null, product: null }));
            dispatch(setUnbalances(unbalances));
        },
        requestUnbalances: (apiUrl, ticket, officialMovements, name, firstTimeNodes) => {
            dispatch(requestUnbalances(apiUrl, ticket, officialMovements, name, firstTimeNodes));
        },
        saveOperationalCutOff: (operationalCutOff, sessionId) => {
            dispatch(requestSaveTicket(operationalCutOff, sessionId));
        },
        incrementCutOff: () => {
            dispatch(incrementCutOff());
        },
        onConfirmCutoff: () => {
            dispatch(openModal('cutOffConfirmation'));
        },
        confirm: (message, title, canCancel) => {
            dispatch(openMessageModal(message, {
                title: title,
                canCancel: canCancel
            }));
        },
        showError: message => {
            dispatch(showError(message));
        },
        clearSelection: name => {
            dispatch(clearSelection(name));
        }
    };
};

const consistencyCheckGridConfig = () => {
    return {
        name: 'unbalances',
        idField: 'unbalanceId',
        selectable: true,
        odata: false,
        apiUrl: apiService.operationalCutOff.getUnbalances(),
        resume: true
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(dataGrid(UnbalancesGrid, consistencyCheckGridConfig));
