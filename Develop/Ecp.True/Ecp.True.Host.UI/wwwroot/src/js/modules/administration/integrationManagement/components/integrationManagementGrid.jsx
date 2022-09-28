/* eslint-disable quotes */
import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../common/components/grid/grid.jsx';
import { dataGrid } from '../../../../common/components/grid/gridComponent';
import { gridUtils } from '../../../../common/components/grid/gridUtils';
import { ActionCell, DateCell, TranslateCell } from '../../../../common/components/grid/gridCells.jsx';
import { utilities } from '../../../../common/services/utilities';
import { constants } from '../../../../common/services/constants';
import { optionService } from '../../../../common/services/optionService';
import { apiService } from '../../../../common/services/apiService.js';
import { openMessageModal, requestFileReadAccessInfoByContainer } from '../../../../common/actions';
import blobService from '../../../../common/services/blobService';
import { resourceProvider } from '../../../../common/services/resourceProvider.js';


export class IntegrationManagementGrid extends React.Component {
    constructor() {
        super();
        this.onDownload = this.onDownload.bind(this);
    }

    getColumns() {
        const actionCell = rowProps => (<ActionCell {...this.props} {...rowProps} onDownload={this.onDownload} />);
        const dateWithTime = rowProps => <DateCell {...this.props} {...rowProps} dateWithTime={true} />;
        const translate = gridProps => <TranslateCell {...this.props} {...gridProps} transformToLowercase={true} />;

        const columns = [];
        columns.push(gridUtils.buildDateColumn('createdDate', this.props, dateWithTime, 'createdDate'));
        columns.push(gridUtils.buildTextColumn('name', this.props, null, 'nameOffile'));
        columns.push(gridUtils.buildSelectColumn('sourceTypeId', this.props, translate, 'sourceTypeId', {
            values: optionService.getSourceTypeBySapAndFico()
        }));
        columns.push(gridUtils.buildSelectColumn('integrationType', this.props, null, 'integrationType', {
            values: optionService.getIntegrationType()
        }));
        columns.push(gridUtils.buildActionColumn(actionCell, '', 100));
        return columns;
    }

    onDownload(row) {
        const containerName = this.getNameContainer(row.sourceTypeId);
        if (!utilities.isNullOrUndefined(this.props.readAccessInfo[containerName])) {
            const { sasToken, accountName } = this.props.readAccessInfo[containerName];
            blobService.initialize(accountName);
            const name = new RegExp("\\.json$", 'i').test(row.name) ? row.name : `${row.name}.json`;
            blobService.downloadSecureFile(sasToken, containerName, row.blobPath, name)
                .catch(err => {
                    console.error(err);
                    this.props.showTechnicalError(resourceProvider.read('errorDownloadFile'), resourceProvider.read('errorDownloadFileTitle'), false);
                });
        }
    }

    getNameContainer(sourceType) {
        switch (sourceType) {
        case 'PURCHASE':
        case 'SELL':
        case 'MOVEMENTS':
        case 'INVENTORY':
        case 'LOGISTIC':
            return "true";
        case 'OPERATIVEDELTA':
        case 'OFFICIALDELTA':
            return 'delta';
        case 'OWNERSHIP':
            return 'ownership';
        default:
            return "";
        }
    }

    componentDidMount() {
        if (this.props.download) {
            this.props.requestFileReadAccess();
        }
    }

    render() {
        return (
            <Grid name={this.props.name} columns={this.getColumns()} shouldRefresh={(() => true)} />
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        download: true,
        downloadTitle: 'downloadFile',
        readAccessInfo: state.shared.readAccessInfoByContainer
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        enableDownload: () => {
            return true;
        },
        requestFileReadAccess: () => {
            dispatch(requestFileReadAccessInfoByContainer('true'));
            dispatch(requestFileReadAccessInfoByContainer('delta'));
            dispatch(requestFileReadAccessInfoByContainer('ownership'));
        },
        showTechnicalError: (message, title, canCancel) => {
            dispatch(openMessageModal(message, { title, canCancel }));
        }
    };
};

/* istanbul ignore next */
const defaultFilter = () => {
    const sourceType = optionService.getSourceTypeBySapAndFico().map(x => `'${x.value}'`).join(',');
    return `sourceTypeId in (${sourceType})`;
};

/* istanbul ignore next */
const gridConfig = props => {
    return {
        name: 'integrationManagementGrid',
        apiUrl: apiService.integration.getIntegrationManagement(),
        sortable: {
            defaultSort: 'createdDate desc'
        },
        filterable: {
            defaultFilter: defaultFilter(props),
            override: false
        },
        refreshable: {
            interval: constants.Timeouts.Tickets
        },
        defaultPageSize: 10,
        section: true
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(dataGrid(IntegrationManagementGrid, gridConfig));
