import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../common/components/grid/grid.jsx';
import { gridUtils } from '../../../../common/components/grid/gridUtils';
import { dataGrid } from '../../../../common/components/grid/gridComponent';
import { openModal, wizardNextStep } from '../../../../common/actions';
import { setPendingTransactionErrors, requestPendingTransactions, incrementCutOff, initCutOff, intAddComment } from '../actions';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { DateCell, TextWithToolTipCell } from '../../../../common/components/grid/gridCells.jsx';
import { apiService } from '../../../../common/services/apiService';
import { dateService } from '../../../../common/services/dateService';
import { utilities } from '../../../../common/services/utilities';
import SectionFooter from '../../../../common/components/footer/sectionFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import { removeGridData, clearSelection } from '../../../../common/components/grid/actions';

export class ErrorsGrid extends React.Component {
    constructor() {
        super();
        this.getErrors = this.getErrors.bind(this);
    }

    getColumns() {
        const columns = [];
        const date = rowProps => <DateCell {...this.props} {...rowProps} />;
        const errorMessageCell = rowProps => <TextWithToolTipCell {...this.props} {...rowProps} />;

        columns.push(gridUtils.buildTextColumn('errorMessage', this.props, errorMessageCell));
        columns.push(gridUtils.buildTextColumn('systemName', this.props, null, 'source'));
        columns.push(gridUtils.buildTextColumn('messageType', this.props, null, 'classification', { type: 'text' }));
        columns.push(gridUtils.buildTextColumn('actionType', this.props, null, 'actionType', { type: 'text' }));
        columns.push(gridUtils.buildTextColumn('sourceNode', this.props));
        columns.push(gridUtils.buildTextColumn('destinationNode', this.props));
        columns.push(gridUtils.buildTextColumn('sourceProduct', this.props));
        columns.push(gridUtils.buildTextColumn('destinationProduct', this.props));
        columns.push(gridUtils.buildDateColumn('pendingTransaction.startDate', this.props, date, 'date'));
        columns.push(gridUtils.buildDecimalColumn('pendingTransaction.volume', this.props, 'netQuantity', { type: 'number' }));
        columns.push(gridUtils.buildTextColumn('units', this.props));

        return columns;
    }

    getErrors(apiUrl) {
        if (this.props.step === 0) {
            this.props.getErrors(apiUrl, this.props.operationalCut.ticket, this.props.name);
            this.props.incrementCutOff();
        }
    }

    render() {
        return (
            <div className="ep-section">
                <header className="ep-section__header">
                    <h1 className="ep-section__title m-t-3">
                        {resourceProvider.read('operationalCutErrorGridMessage')
                            .replace('{0}', dateService.capitalize(this.props.operationalCut.ticket.startDate))
                            .replace('{1}', dateService.capitalize(this.props.operationalCut.ticket.endDate))
                            .replace('{2}', this.props.operationalCut.ticket.categoryElementName)
                        }
                    </h1>
                    <span className="float-r">
                        {this.props.items.length > 0 && <span className="fw-sb m-r-4"><i className="fas fa-info-circle m-r-2 fs-20" />{resourceProvider.read('manageMessage')}</span>}
                        <button disabled={this.props.selection.length === 0}
                            id="btn_errorsGrid_addNote" type="button" className="ep-btn"
                            onClick={() => this.props.onAddNote('pendingTransactions', this.props.selection.length)}>
                            <i className="far fa-sticky-note m-r-1" /><span className="ep-btn__txt">{resourceProvider.read('addNote')}</span></button>
                    </span>
                </header>
                <div className="ep-section__body ep-section__body--hf p-a-0">
                    <Grid name={this.props.name} columns={this.getColumns()} requestGridData={this.getErrors} className="ep-table--row-sm ep-table--wizard" />
                </div>
                <SectionFooter floatRight={true} config={footerConfigService.getCommonConfig('errorsGrid',
                    {
                        acceptActions: [wizardNextStep(this.props.config.wizardName)], onCancel: this.props.cancelCutOff, disableAccept: (this.props.items.length > 0),
                        acceptText: 'next', acceptClassName: 'ep-btn', acceptType: 'button'
                    })} />
            </div>
        );
    }

    componentDidUpdate(prevProps) {
        if (this.props.commentToggler !== prevProps.commentToggler) {
            this.props.onSubmit(this.props.selection, this.props.comment, this.props.name);
            this.props.clearSelection('pendingTransactions');
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    const props = {
        selection: state.grid.pendingTransactions ? state.grid.pendingTransactions.selection : [],
        comment: state.cutoff.operationalCut.pendingTransactions ? state.cutoff.operationalCut.pendingTransactions.comment : null,
        items: state.grid.pendingTransactions ? state.grid.pendingTransactions.items : [],
        operationalCut: state.cutoff.operationalCut,
        step: state.cutoff.operationalCut.step
    };

    if (state.cutoff.operationalCut.pendingTransactions) {
        props.commentToggler = state.cutoff.operationalCut.pendingTransactions.commentToggler;
    }

    return props;
};

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    return {
        cancelCutOff: () => {
            dispatch(initCutOff());
            ownProps.goToStep('operationalCut', 1);
        },
        onAddNote: (name, count) => {
            const txt = count > 1 ? 'Plural' : 'Singular';
            dispatch(openModal('addComments'));
            dispatch(intAddComment(name, resourceProvider.read(`addCommentPreText${txt}`), resourceProvider.read(`addCommentPostText${txt}`), count));
        },
        onSubmit: (items, comment, name) => {
            dispatch(removeGridData(name, items));
            dispatch(setPendingTransactionErrors(items.map(x => Object.assign({}, x, { comment: comment }))));
        },
        getErrors: (apiUrl, ticket, name) => {
            dispatch(requestPendingTransactions(apiUrl, ticket, name));
        },
        incrementCutOff: () => {
            dispatch(incrementCutOff());
        },
        clearSelection: name => {
            dispatch(clearSelection(name));
        }
    };
};

/* istanbul ignore next */
const gridConfig = () => {
    return {
        name: 'pendingTransactions',
        idField: 'errorId',
        selectable: true,
        odata: false,
        apiUrl: apiService.operationalCutOff.getPendingTransactionErrors()
    };
};

/* istanbul ignore next */
export default dataGrid(connect(mapStateToProps, mapDispatchToProps, utilities.merge)(ErrorsGrid), gridConfig);
