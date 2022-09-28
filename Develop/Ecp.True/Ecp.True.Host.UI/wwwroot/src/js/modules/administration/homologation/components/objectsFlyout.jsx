import React from 'react';
import { connect } from 'react-redux';
import Grid from './../../../../common/components/grid/grid.jsx';
import { gridUtils } from './../../../../common/components/grid/gridUtils';
import { TogglerCell } from './../../../../common/components/grid/gridCells.jsx';
import { dataGrid } from './../../../../common/components/grid/gridComponent';
import Flyout from './../../../../common/components/flyout/flyout.jsx';
import { receiveGridData, selectGridData, addUpdateGridData } from '../../../../common/components/grid/actions';
import { requestHomologationObjectTypes, createHomologationObjectTypes, updateHomologationObjectTypes } from './../actions';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import { utilities } from './../../../../common/services/utilities';
import FlyoutFooter from '../../../../common/components/footer/flyoutFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import { closeFlyout } from '../../../../common/actions';

class ObjectsFlyout extends React.Component {
    constructor() {
        super();
        this.filterObjectTypes = this.filterObjectTypes.bind(this);
        this.checkEnter = this.checkEnter.bind(this);
        this.createHomologationObjectTypes = this.createHomologationObjectTypes.bind(this);
    }

    filterObjectTypes(filterKey) {
        const searchName = document.getElementById('txt_objectsFlyout_name');

        const nativeInputValueSetter = Object.getOwnPropertyDescriptor(window.HTMLInputElement.prototype, 'value').set;
        nativeInputValueSetter.call(searchName, filterKey.toLowerCase());

        searchName.dispatchEvent(new Event('change', { bubbles: true }));
    }

    checkEnter(e) {
        if (e.key === 'Enter') {
            const searchName = document.getElementById('txt_objectsFlyout_name');
            searchName.dispatchEvent(new KeyboardEvent('keydown', { bubbles: true, cancelable: true, keyCode: 13 }));
        }
    }

    onCancel() {
        this.props.closeFlyout(this.props.name);
    }

    createHomologationObjectTypes() {
        if (!utilities.isNullOrUndefined(this.props.selectedObjectsTypes)) {
            this.props.createHomologationObjectTypes(this.props.selectedObjectsTypes);
        }
    }

    getColumns() {
        const columns = [];
        const togglerCell = rowProps => <span className="float-r"><TogglerCell {...this.props} {...rowProps} /></span>;

        columns.push(gridUtils.buildTextColumn('name', this.props, '', 'name', { width: 150, hideFilterTextBox: true }));
        columns.push(gridUtils.buildSelectColumn('isRequiredMapping', this.props, togglerCell, 'required', { filterable: false }));
        return columns;
    }

    componentDidUpdate(prevProps) {
        if (prevProps.objectTypesToggler !== this.props.objectTypesToggler) {
            this.props.loadObjectTypes(this.props.objectTypes);
        }

        if (prevProps.homologationGroupToggler !== this.props.homologationGroupToggler) {
            this.props.updateHomologationObjectTypesGrid(this.props.mappedObjectTypes);
        }
    }

    componentDidMount() {
        this.props.requestObjects();
    }

    render() {
        return (
            <Flyout name={this.props.name}>
                <header className="ep-flyout__header ep-flyout__header--unjust">
                    <button id="btn_uploadFileFilter_cancel" type="button" className="ep-btn ep-btn--tr m-r-6" onClick={() => this.onCancel()}>
                        {resourceProvider.read('cancel')}
                    </button>
                    <h1 className="ep-flyout__title">{resourceProvider.read('addObjects')}</h1>
                </header>
                <section className="ep-flyout__body">
                    <div className="ep-control ep-control--addon m-b-2">
                        <div className="ep-control__inner">
                            <label className="ep-label sr-only" htmlFor="txt_objectTypes_search">{resourceProvider.read('searchObjects')}</label>
                            <input type="text" placeholder={resourceProvider.read('searchObjects')} className="ep-textbox"
                                id="txt_objectTypes_search" onChange={e => this.filterObjectTypes(e.target.value)} onKeyUp={e => this.checkEnter(e)} />
                            <span className="ep-control__inner-addon"><i className="fas fa-search" /></span>
                        </div>
                    </div>
                    <Grid wrapperClassName="ep-table-wrap--hn47" className="ep-table--list" name="objectTypes" columns={this.getColumns()} />
                </section>
                <FlyoutFooter config={footerConfigService.getFlyoutConfig(this.props.name, 'applyFilters', this.createHomologationObjectTypes)} />
            </Flyout>
        );
    }
}

const mapStateToProps = state => {
    return {
        objectTypes: state.homologations.objectTypes,
        mappedObjectTypes: state.homologations.homologationGroup && state.homologations.homologationGroup.homologationObjectTypes,
        selectedObjectsTypes: state.grid.objectTypes.selection,
        objectTypesToggler: state.homologations.objectTypesToggler,
        homologationGroupToggler: state.homologations.homologationGroupToggler
    };
};

const mapDispatchToProps = dispatch => {
    return {
        requestObjects: () => {
            dispatch(requestHomologationObjectTypes());
        },
        loadObjectTypes: objectTypes => {
            dispatch(receiveGridData(objectTypes, 'objectTypes'));
        },
        closeFlyout: flyoutName => {
            dispatch(closeFlyout(flyoutName));
        },
        updateHomologationObjectType: (obj, isRequiredMapping) => {
            const objectType = Object.assign({}, obj, { isRequiredMapping });
            dispatch(addUpdateGridData('objectTypes', 'homologationObjectTypeId', objectType));
            dispatch(updateHomologationObjectTypes(objectType));
        },
        createHomologationObjectTypes: objectTypes => {
            dispatch(createHomologationObjectTypes(objectTypes));
            dispatch(closeFlyout('objectsFlyout'));
        },
        updateHomologationObjectTypesGrid: objectTypes => {
            objectTypes.forEach(objectType => {
                dispatch(addUpdateGridData('objectTypes', 'homologationObjectTypeId', objectType));
                dispatch(selectGridData(`${objectType.homologationObjectTypeId}`, 'objectTypes'));
            });
        }
    };
};


const objectsGridConfig = () => {
    return {
        name: 'objectTypes',
        odata: false,
        idField: 'homologationObjectTypeId',
        selectable: {
            pageSelection: false
        },
        showPageSizeOptions: false,
        pageButtonCount: 2,
        defaultPageSize: 12
    };
};

export default dataGrid((connect(mapStateToProps, mapDispatchToProps)(ObjectsFlyout)), objectsGridConfig);
