import React from 'react';
import { connect } from 'react-redux';
import Grid from '../grid/grid.jsx';
import { gridUtils } from '../grid/gridUtils';
import { dataGrid } from '../grid/gridComponent';
import { receiveGridData } from '../grid/actions';
import { toggleUpdatePopUp } from '../../actions';
import { resourceProvider } from '../../services/resourceProvider';
import { constants } from '../../services/constants';
import ModalFooter from '../footer/modalFooter.jsx';
import { footerConfigService } from '../../services/footerConfigService';

class BulkUpdateConfirm extends React.Component {
    constructor() {
        super();
        this.getColumns = this.getColumns.bind(this);
        this.getMessageText = this.getMessageText.bind(this);
        this.updateRules = this.updateRules.bind(this);
    }

    updateRules() {
        this.props.toggleUpdatePopUp(this.props.type);
        this.props.closeModal();
    }

    getColumns() {
        const columns = [];
        if (this.props.type === constants.RuleType.Node) {
            columns.push(gridUtils.buildTextColumn('name', this.props, null, 'node', { sortable: false, filterable: false }));
        } else if (this.props.type === constants.RuleType.NodeConnectionProduct) {
            columns.push(gridUtils.buildTextColumn('sourceNode', this.props, null, 'sourceNode', { sortable: false, filterable: false }));
            columns.push(gridUtils.buildTextColumn('destinationNode', this.props, null, 'destinationNode', { sortable: false, filterable: false }));
            columns.push(gridUtils.buildTextColumn('product', this.props, null, 'product', { sortable: false, filterable: false }));
        } else {
            columns.push(gridUtils.buildTextColumn('nodeName', this.props, null, 'node', { sortable: false, filterable: false }));
            columns.push(gridUtils.buildTextColumn('product', this.props, null, 'product', { sortable: false, filterable: false }));
        }
        columns.push(gridUtils.buildTextColumn('ruleName', this.props, null, 'strategy', { sortable: false, filterable: false }));
        return columns;
    }

    getMessageText() {
        let type = resourceProvider.read('connectionMessagePrefix');
        if (this.props.type === constants.RuleType.NodeProduct || this.props.type === constants.RuleType.Node) {
            type = resourceProvider.read('nodeMessagePrefix');
        }
        return resourceProvider.readFormat('bulkUpdateConfirmMessage', [type]);
    }

    render() {
        return (
            <>
                <section className="ep-modal__content">
                    <div className="ep-control-group m-b-4">
                        <span className="ep-data fw-bold" id="lbl_transferPointLogistics_name">
                            {this.getMessageText()}
                        </span>
                    </div>
                    <div className="ep-label-wrap m-b-3">
                        <label className="ep-label">{resourceProvider.read('errorRecordCount')}</label>
                        <span className="ep-data fw-bold fas--success m-l-1">{this.props.data.length}</span>
                    </div>
                    <Grid className="ep-table--pivotal ep-table--pivotal-alt" name={this.props.name} columns={this.getColumns()} />
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('bulkUpdateConfirm', { onAccept: this.updateRules, acceptText: 'accept' })} />
            </>
        );
    }

    componentDidMount() {
        this.props.loadData(this.props.data);
    }
}

/* istanbul ignore next */
const mapStateToProps = (state, ownProps) => {
    return {
        data: state.ownershipRules[ownProps.type].items
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        loadData: data => {
            dispatch(receiveGridData(data, 'bulkUpdateConfirmGrid'));
        },
        toggleUpdatePopUp: name => {
            dispatch(toggleUpdatePopUp(name));
        }
    };
};
const bulkUpdateConfirmGridConfig = () => {
    return {
        name: 'bulkUpdateConfirmGrid',
        odata: false,
        showPagination: false,
        defaultPageSize: 999999
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(dataGrid(BulkUpdateConfirm, bulkUpdateConfirmGridConfig));
