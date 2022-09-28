import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../common/components/grid/grid.jsx';
import { gridUtils } from '../../../../common/components/grid/gridUtils';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { DateCell, ActionCell } from '../../../../common/components/grid/gridCells.jsx';
import { dataGrid } from '../../../../common/components/grid/gridComponent';
import { initCutOff, incrementCutOff, requestTransferPointMovements, setTransferPointMovements, getSapTrackingErrors, setSapTrackingErrors } from '../actions';
import { openModal, intAddComment, wizardNextStep } from '../../../../common/actions';
import { apiService } from '../../../../common/services/apiService';
import { utilities } from '../../../../common/services/utilities';
import SectionFooter from '../../../../common/components/footer/sectionFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import { clearSelection } from '../../../../common/components/grid/actions';
import { constants } from '../../../../common/services/constants.js';

export class OfficialPointsGrid extends React.Component {
    constructor() {
        super();
        this.getMovements = this.getMovements.bind(this);
    }

    getColumns() {
        const columns = [];
        const date = rowProps => <DateCell {...this.props} {...rowProps} />;
        const actions = gridProps => <ActionCell {...this.props} {...gridProps} onView={this.props.onView} />;

        columns.push(gridUtils.buildTextColumn('movementId', this.props, null, 'officialMovementId'));
        columns.push(gridUtils.buildTextColumn('movementTypeName', this.props, null, 'movementTypeTransferOperational'));
        columns.push(gridUtils.buildTextColumn('sourceNodeName', this.props, null, 'sourceNodeTransferOperational'));
        columns.push(gridUtils.buildTextColumn('destinationNodeName', this.props, null, 'destinationNodeTransferOperational'));
        columns.push(gridUtils.buildTextColumn('sourceProductName', this.props, null, 'sourceProductTransferOperational'));
        columns.push(gridUtils.buildTextColumn('destinationProductName', this.props, null, 'destinationProductTransferPoints'));
        columns.push(gridUtils.buildDecimalColumn('netStandardVolume', this.props, 'netQuantity', { type: 'number' }));
        columns.push(gridUtils.buildTextColumn('measurementUnit', this.props, null, 'units'));
        columns.push(gridUtils.buildDateColumn('operationalDate', this.props, date, 'dateOperational'));
        columns.push(gridUtils.buildActionColumn(actions, null, 100));
        return columns;
    }

    getMovements(apiUrl) {
        if (this.props.step === 1) {
            this.props.getMovements(apiUrl, this.props.ticket, this.props.name);
            this.props.incrementCutOff();
        }
    }

    render() {
        return (
            <div className="ep-section">
                <header className="ep-section__header">
                    <h1 className="ep-section__title m-t-3"> {resourceProvider.read('operationalCutOfficialPoints')} </h1>
                </header>
                <div className="ep-section__body ep-section__body--hf p-a-0">
                    <Grid name={this.props.name} columns={this.getColumns()} requestGridData={this.getMovements} className="ep-table--row-sm ep-table--wizard" />
                </div>
                <SectionFooter floatRight={true} config={footerConfigService.getCommonConfig('officialPointsGrid',
                    {
                        acceptActions: [wizardNextStep(this.props.config.wizardName)],
                        onCancel: this.props.cancelCutOff,
                        acceptText: 'next', acceptClassName: 'ep-btn', acceptType: 'button'
                    })} />
            </div>
        );
    }

    componentDidUpdate(prevProps) {
        if (prevProps.items !== this.props.items) {
            this.props.onSubmit(this.props.selection, this.props.name);
        }

        if (this.props.officialPointErrorToggler !== prevProps.officialPointErrorToggler) {
            this.props.openErrorModal();
        }
    }
}

const mapStateToProps = state => {
    return {
        view: true,
        viewTitle: 'viewError',
        selection: state.grid.officialPoints ? state.grid.officialPoints.items : [],
        comment: state.addComment.operationalCut ? state.addComment.operationalCut.comment : null,
        items: state.grid.officialPoints ? state.grid.officialPoints.items : [],
        operationalCut: state.cutoff.operationalCut,
        step: state.cutoff.operationalCut.step,
        ticket: state.cutoff.operationalCut.ticket,
        officialPointErrorToggler: state.cutoff.ticketInfo.officialPointErrorToggler,
        commentToggler: state.addComment.operationalCut ? state.addComment.operationalCut.commentToggler : false
    };
};

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    return {
        cancelCutOff: () => {
            dispatch(initCutOff());
            ownProps.goToStep('operationalCut', 1);
        },
        onAddNote: component => {
            dispatch(openModal('commonAddComment', '', 'officialPointsAddComment'));
            dispatch(intAddComment('operationalCut', '', component, 'comment', 'required'));
        },
        incrementCutOff: () => {
            dispatch(incrementCutOff());
        },
        getMovements: (path, ticket, name) => {
            dispatch(requestTransferPointMovements(path, ticket, name));
        },
        onSubmit: items => {
            dispatch(setTransferPointMovements(
                items.map(x => Object.assign({}, x, { comment: constants.OperationalCutOff.TransferPoint.DefaultComment }))
            ));
        },
        clearSelection: name => {
            dispatch(clearSelection(name));
        },
        enableView: row => {
            return (row.errorCount > 0) || (!utilities.isNullOrUndefined(row.errorMessage) && !utilities.isNullOrWhitespace(row.errorMessage));
        },
        onView: row => {
            if (!utilities.isNullOrUndefined(row.errorMessage) && !utilities.isNullOrWhitespace(row.errorMessage)) {
                dispatch(setSapTrackingErrors([], row));
            } else {
                dispatch(getSapTrackingErrors(row));
            }
        },
        openErrorModal: () => {
            dispatch(openModal('showOfficialPointsError', '', 'officialErrorTitle', 'ep-modal ep-modal--sm'));
        }
    };
};

/* istanbul ignore next */
const gridConfig = () => {
    return {
        name: 'officialPoints',
        idField: 'movementId',
        odata: false,
        apiUrl: apiService.operationalCutOff.getTransferPointMovements()
    };
};

/* istanbul ignore next */
export default dataGrid(connect(mapStateToProps, mapDispatchToProps, utilities.merge)(OfficialPointsGrid), gridConfig);
