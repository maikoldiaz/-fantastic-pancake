import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../common/components/grid/grid.jsx';
import { dataGrid } from '../../../../common/components/grid/gridComponent';
import { DateCell } from '../../../../common/components/grid/gridCells.jsx';
import { gridUtils } from '../../../../common/components/grid/gridUtils';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { apiService } from '../../../../common/services/apiService';
import { utilities } from '../../../../common/services/utilities';
import { constants } from '../../../../common/services/constants';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';

class OwnershipNodeErrorDetailsGrid extends React.Component {
    getColumns() {
        const columns = [];
        const date = rowProps => <DateCell {...this.props} {...rowProps} />;
        columns.push(gridUtils.buildTextColumn('operationId', this.props, null, 'ownershipNodeErrorId', { sortable: false, filterable: false }));
        columns.push(gridUtils.buildTextColumn('type', this.props, r => resourceProvider.read(utilities.toLowerCase(r.original.type)),
            'classificationType', { sortable: false, filterable: false }));
        columns.push(gridUtils.buildTextColumn('operation', this.props, r => resourceProvider.read(utilities.toLowerCase(r.original.operation)),
            'operationType', { sortable: false, filterable: false }));
        columns.push(gridUtils.buildDateColumn('operationDate', this.props, date, 'operationDate', { sortable: false, filterable: false }));
        columns.push(gridUtils.buildDateColumn('executionDate', this.props, date, 'executionDateOC', { sortable: false, filterable: false }));
        columns.push(gridUtils.buildTextColumn('segment', this.props, null, 'segment', { sortable: false, filterable: false }));
        columns.push(gridUtils.buildNumberColumn('netVolume', this.props, 'netVolume', { sortable: false, filterable: false }));

        if (utilities.matchesAny(this.props.nodeState, [constants.OwnershipNodeStatusType.CONCILIATIONFAILED, constants.OwnershipNodeStatusType.NOTCONCILIATED])) {
            columns.push(gridUtils.buildTextColumn('nodeOrigin', this.props, null, 'sourceNode', { sortable: false, filterable: false }));
            columns.push(gridUtils.buildTextColumn('nodeDestination', this.props, null, 'destinationNode', { sortable: false, filterable: false }));
        } else {
            columns.push(gridUtils.buildTextColumn('productOrigin', this.props, null, 'productOrigin', { sortable: false, filterable: false }));
            columns.push(gridUtils.buildTextColumn('productDestination', this.props, null, 'productDestination', { sortable: false, filterable: false }));
        }

        columns.push(gridUtils.buildTextColumn('errorMessage', this.props, null, 'errorMessageOC', { sortable: false, filterable: false }));

        return columns;
    }
    render() {
        return (
            <>
                <section className="ep-modal__content">
                    <div className="ep-label-wrap">
                        <label className="ep-label">{resourceProvider.read('node')}</label> : <span className="ep-data m-l-2 fw-600" >{this.props.nodeName}</span>
                    </div>
                    <Grid className="ep-table--h200 ep-table--nofilter"
                        name={this.props.name} columns={this.getColumns()} />
                </section>
                <ModalFooter config={footerConfigService.getAcceptConfig('ownershipNodeErrorDetails', { closeModal: true, acceptText: 'accept' })} />
            </>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        ownershipNodeId: state.nodeOwnership.ownershipNode.node.ownershipNodeId,
        nodeName: state.nodeOwnership.ownershipNode.node.nodeName,
        nodeState: state.nodeOwnership.ownershipNode.node.state,
        ignoreMax: true
    };
};

/* istanbul ignore next */
const defaultFilter = props => {
    return `ownershipNodeId eq ${props.ownershipNodeId}`;
};

const ownershipNodeErrorDetailsGridConfig = props => {
    return {
        name: 'ownershipNodeErrors',
        showPagination: false,
        apiUrl: apiService.ownershipNode.getErrors(),
        filterable: {
            defaultFilter: defaultFilter(props),
            override: false
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps)(dataGrid(OwnershipNodeErrorDetailsGrid, ownershipNodeErrorDetailsGridConfig));
