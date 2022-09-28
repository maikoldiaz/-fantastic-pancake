import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../../common/components/grid/grid.jsx';
import { gridUtils } from '../../../../../common/components/grid/gridUtils';
import { dataGrid } from '../../../../../common/components/grid/gridComponent';
import { receiveGridData } from '../../../../../common/components/grid/actions';

class TagErrorsGrid extends React.Component {
    constructor() {
        super();
        this.getColumns = this.getColumns.bind(this);
    }

    getColumns() {
        const columns = [];

        columns.push(gridUtils.buildTextColumn('node.name', this.props, null, 'node'));
        columns.push(gridUtils.buildTextColumn('categoryElement.category.name', this.props, null, 'category'));
        columns.push(gridUtils.buildTextColumn('categoryElement.name', this.props, null, 'element'));

        return columns;
    }

    render() {
        return (
            <Grid name={this.props.name} columns={this.getColumns()} />
        );
    }

    componentDidMount() {
        this.props.populate(this.props.nodes, this.props.name);
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        nodes: state.node.nodeTags.errorNodes
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        populate: (items, name) => {
            dispatch(receiveGridData(items, name));
        }
    };
};

/* istanbul ignore next */
const gridConfig = () => {
    return {
        name: 'nodeTagErrors',
        odata: false,
        section: true
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(dataGrid(TagErrorsGrid, gridConfig));
