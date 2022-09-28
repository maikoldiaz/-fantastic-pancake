import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../common/components/grid/grid.jsx';
import { gridUtils } from '../../../../common/components/grid/gridUtils';
import { dataGrid } from '../../../../common/components/grid/gridComponent';
import { apiService } from '../../../../common/services/apiService';
import { ActionCell, DateCell } from '../../../../common/components/grid/gridCells.jsx';
import { receiveGridData } from '../../../../common/components/grid/actions';
import { openModal, showError } from '../../../../common/actions';
import { optionService } from '../../../../common/services/optionService';
import { requestTransactions, viewTransaction, onPageChange, resetState } from './../actions';
import { constants } from '../../../../common/services/constants';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { utilities } from '../../../../common/services/utilities';

export class TransactionsGrid extends React.Component {
    constructor() {
        super();
        this.getTransactions = this.getTransactions.bind(this);
        this.onPageChange = this.onPageChange.bind(this);
    }

    getTransactions(apiUrl) {
        if (this.props.isNext) {
            this.props.getTransactions(apiUrl, 100, this.props.nextPageHead, this.props.name);
        } else {
            const previousHead = this.props.previousHead.filter(x => x.page === this.props.currentPage)[0].head === -1 ?
                null : this.props.previousHead.filter(x => x.page === this.props.currentPage)[0].head;
            this.props.getTransactions(apiUrl, 100, this.props.isNext ? this.props.nextPageHead : previousHead, this.props.name);
        }
    }

    async onPageChange(data) {
        await this.props.onPageChange(data.isNext);
        return this.props.currentPage;
    }

    getColumns() {
        const actions = gridProps => <ActionCell {...this.props} {...gridProps} />;
        const date = rowProps => <DateCell {...this.props} {...rowProps} dateWithTime={true} />;

        const columns = [];

        columns.push(gridUtils.buildTextColumn('blockNumber', this.props, null, 'blockNumber', { width: 200 }));
        columns.push(gridUtils.buildTextColumn('address', this.props, null, 'contractAddress'));
        columns.push(gridUtils.buildTextColumn('transactionHash', this.props, null, 'transactionHash'));
        columns.push(gridUtils.buildDateColumn('transactionTime', this.props, date, 'createdDate', { width: 200 }));
        columns.push(gridUtils.buildSelectColumn('type', this.props, row => constants.BlockChainPageType[row.row.type],
            'type', { values: optionService.getBlockchainTypes(), width: 250 }));

        columns.push(gridUtils.buildActionColumn(actions, null, 100));

        return columns;
    }

    render() {
        return (
            <Grid name={this.props.name} columns={this.getColumns()} requestGridData={this.getTransactions} onPageNavigation={this.onPageChange} />
        );
    }
    componentDidUpdate(prevProps) {
        if (this.props.transactionsToggler !== prevProps.transactionsToggler) {
            this.props.receiveGridData(this.props.transactions.events, this.props.name);
        }
        if (this.props.transactionFailure !== prevProps.transactionFailure) {
            this.props.showError(resourceProvider.read('rpcTimeout'));
            this.props.receiveGridData([], this.props.name);
        }
    }
    componentWillUnmount() {
        this.props.resetState();
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        view: true,
        viewTitle: 'viewSummary',
        nextPageHead: state.blockchain.nextPageHead,
        transactions: state.blockchain.transactions,
        transactionsToggler: state.blockchain.transactionsToggler,
        previousHead: state.blockchain.previousHead,
        isNext: state.blockchain.isNext,
        currentPage: state.blockchain.currentPage,
        transactionFailure: state.blockchain.transactionFailure
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        onView: transaction => {
            dispatch(viewTransaction(transaction));
            dispatch(openModal('transactionDetails'), constants.Modes.View);
        },
        getTransactions: (apiUrl, pageSize, lastHead, name) => {
            dispatch(requestTransactions(apiUrl, pageSize, lastHead, name));
        },
        receiveGridData: (transactions, name) => {
            dispatch(receiveGridData(transactions, name));
        },
        onPageChange: isNext => {
            dispatch(onPageChange(isNext));
        },
        showError: message => {
            dispatch(showError(message, false, resourceProvider.read('error')));
        },
        resetState: () => {
            dispatch(resetState());
        }
    };
};

const transactionsGridConfig = props => {
    return {
        name: 'blockchaintransactions',
        idField: 'transactionhash',
        sortable: false,
        apiUrl: apiService.blockchain.getEvents(),
        odata: false,
        showPagination: true,
        defaultPageSize: 100,
        incrementalPagination: true,
        showPageSizeOptions: false,
        currentPage: props.currentPage,
        section: true
    };
};

/* istanbul ignore next */
const GridComponent = dataGrid(TransactionsGrid, transactionsGridConfig);

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(GridComponent);
