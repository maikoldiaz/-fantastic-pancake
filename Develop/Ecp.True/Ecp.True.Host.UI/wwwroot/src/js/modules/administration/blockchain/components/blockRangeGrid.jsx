import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../common/components/grid/grid.jsx';
import { gridUtils } from '../../../../common/components/grid/gridUtils';
import { dataGrid } from '../../../../common/components/grid/gridComponent';
import { apiService } from '../../../../common/services/apiService';
import { ActionCell, DateCell } from '../../../../common/components/grid/gridCells.jsx';
import { receiveGridData } from '../../../../common/components/grid/actions';
import { optionService } from '../../../../common/services/optionService';
import { requestRangeEvents, viewTransaction } from '../actions';
import { constants } from '../../../../common/services/constants';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { utilities } from '../../../../common/services/utilities';
import { openModal, showError } from '../../../../common/actions';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import SectionFooter from '../../../../common/components/footer/sectionFooter.jsx';
import { navigationService } from '../../../../common/services/navigationService';

export class BlockRangeGrid extends React.Component {
    constructor() {
        super();
        this.getEvents = this.getEvents.bind(this);
    }

    getEvents(apiUrl) {
        this.props.getEvents(apiUrl, this.props.blockRange);
    }

    getColumns() {
        const actions = gridProps => <ActionCell {...this.props} {...gridProps} />;
        const date = rowProps => <DateCell {...this.props} {...rowProps} dateWithTime={true} />;

        const columns = [];

        columns.push(gridUtils.buildTextColumn('blockNumber', this.props, null, 'blockNumber', { width: 200 }));
        columns.push(gridUtils.buildTextColumn('transactionHash', this.props, null, 'transactionHash'));
        columns.push(gridUtils.buildDateColumn('transactionTime', this.props, date, 'createdDate', { width: 200 }));
        columns.push(gridUtils.buildSelectColumn('type', this.props, row => constants.BlockChainPageType[row.row.type],
            'type', { values: optionService.getBlockchainTypes(), width: 250 }));
        columns.push(gridUtils.buildActionColumn(actions, null, 100));
        return columns;
    }

    render() {
        const eventTypes = [{ label: 'Todos', value: 0 }, ...optionService.getBlockchainTypes()];
        return (
            <div className="ep-section">
                <header className="ep-section__header">
                    <h1 className="ep-section__title m-t-3">
                        {resourceProvider.read('blockRangeHeaderMessage')
                            .replace('{0}', this.props.blockRange.headBlock)
                            .replace('{1}', this.props.blockRange.tailBlock)
                            .replace('{2}', eventTypes[this.props.blockRange.event].label)
                        }
                    </h1>
                </header>
                <div className="ep-section__body ep-section__body--hf p-a-0">
                    <Grid name={this.props.name} columns={this.getColumns()} requestGridData={this.getEvents} className="ep-table--row-sm ep-table--wizard" />
                </div>
                <SectionFooter floatRight={true} config={footerConfigService.getCommonConfig('blockchainRangeEvents', {
                    onAccept: this.props.navigate, acceptClassName: 'ep-btn', acceptText: 'accept', onCancel: this.props.cancelBlockRangeSearch
                })} />
            </div>
        );
    }
    componentDidUpdate(prevProps) {
        if (this.props.blockRangeEventsToggler !== prevProps.blockRangeEventsToggler) {
            this.props.receiveGridData(this.props.blockRangeEvents, this.props.name);
        }
        if (this.props.blockRangeEventsFailureToggler !== prevProps.blockRangeEventsFailureToggler) {
            this.props.showError(resourceProvider.read('rpcTimeout'));
            this.props.receiveGridData([], this.props.name);
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        view: true,
        viewTitle: 'viewSummary',
        blockRange: state.blockchain.blockRange,
        blockRangeEvents: state.blockchain.blockRangeEvents,
        blockRangeEventsToggler: state.blockchain.blockRangeEventsToggler,
        blockRangeEventsFailureToggler: state.blockchain.blockRangeEventsFailureToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    return {
        cancelBlockRangeSearch: () => {
            ownProps.goToStep('blockRangeWizard', 1);
        },
        onView: transaction => {
            dispatch(viewTransaction(transaction));
            dispatch(openModal('transactionDetails'), constants.Modes.View);
        },
        getEvents: (apiUrl, request) => {
            dispatch(requestRangeEvents(apiUrl, request));
        },
        receiveGridData: (events, name) => {
            dispatch(receiveGridData(events, name));
        },
        showError: message => {
            dispatch(showError(message, false, resourceProvider.read('error')));
        },
        navigate: () => {
            navigationService.navigateToModule('blockchain/manage');
        }
    };
};

const gridConfig = () => {
    return {
        name: 'blockchainRangeEvents',
        idField: 'transactionhash',
        apiUrl: apiService.blockchain.getEventsInRange(),
        odata: false,
        showPagination: true
    };
};

/* istanbul ignore next */
const GridComponent = dataGrid(BlockRangeGrid, gridConfig);

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(GridComponent);
