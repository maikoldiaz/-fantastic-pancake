import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import HomologationsDataGrid from './homologationsDataGrid.jsx';
import { openModal, openFlyout, enableDisablePageAction, showSuccess } from '../../../../common/actions';
import { requestHomologationGroup, requestHomologationDataMappings, requestCreateUpdateHomologationGroup } from './../actions';
import ObjectsFlyout from './objectsFlyout.jsx';
import { routerActions } from './../../../../common/router/routerActions.js';
import { constants } from './../../../../common/services/constants';
import { navigationService } from './../../../../common/services/navigationService';
import { utilities } from './../../../../common/services/utilities.js';

export class HomologationDetails extends React.Component {
    constructor() {
        super();
        this.onSubmit = this.onSubmit.bind(this);
        routerActions.configure('submit', this.onSubmit);
    }

    onSubmit() {
        const homologationGroup = this.props.homologationGroup;
        const homologationObjectTypes = homologationGroup.homologationObjectTypes;
        const homologationGroupDataMappings = this.props.homologationGroupDataMappings;
        const data = {
            sourceSystemId: homologationGroup.sourceSystem.systemTypeId,
            destinationSystemId: homologationGroup.destinationSystem.systemTypeId
        };

        if (this.props.mode === constants.Modes.Create) {
            data.homologationGroups = [{
                groupTypeId: homologationGroup.group.categoryId,
                homologationObjects: homologationObjectTypes,
                homologationDataMapping: homologationGroupDataMappings
            }];
        } else {
            const homologationGroupId = navigationService.getParamByName('homologationGroupId');
            homologationGroupDataMappings.map(v => {
                v.homologationGroupId = homologationGroupId;
                return v;
            });

            homologationObjectTypes.map(v => {
                v.homologationGroupId = homologationGroupId;
                return v;
            });

            data.homologationGroups = [{
                homologationGroupId: homologationGroupId,
                groupTypeId: homologationGroup.group.categoryId,
                homologationObjects: homologationObjectTypes,
                homologationDataMapping: homologationGroupDataMappings,
                rowVersion: this.props.homologationGroup.rowVersion
            }];
        }

        this.props.requestCreateUpdateHomologationGroup(data);
    }

    componentDidUpdate(prevProps) {
        this.props.enableDisableSubmit(!this.props.isValid);

        if (prevProps.objectTypesToggler !== this.props.objectTypesToggler) {
            this.props.initUpdateHomologationGroup();
        }

        if (prevProps.homologationGroupToggler !== this.props.homologationGroupToggler) {
            this.props.getDataMappings();
        }

        if (prevProps.refreshToggler !== this.props.refreshToggler) {
            this.props.showNotification();
            navigationService.navigateTo('manage');
        }
    }
    render() {
        const homologationGroup = this.props.homologationGroup;
        const homologationObjectTypes = homologationGroup.homologationObjectTypes;
        return (
            <section className="ep-content">
                <div className="ep-content__body">
                    <section className="ep-section ep-section--panel">
                        <div className="p-t-4 p-x-8">
                            <div className="row p-x-3">
                                <div className="col-md-3">
                                    <div className="ep-control-group m-b-3">
                                        <label className="ep-label" htmlFor="txt_sourceSystem_name">{resourceProvider.read('sourceSystem')}</label>
                                        <input className="ep-textbox" id="txt_sourceSystem_name" readOnly value={homologationGroup.sourceSystem.name} />
                                    </div>
                                </div>
                                <div className="col-md-3">
                                    <div className="ep-control-group m-b-3">
                                        <label className="ep-label" htmlFor="txt_destinationSystem_name">{resourceProvider.read('destinationSystem')}</label>
                                        <input className="ep-textbox" id="txt_destinationSystem_name" readOnly value={homologationGroup.destinationSystem.name} />
                                    </div>
                                </div>
                                <div className="col-md-3">
                                    <div className="ep-control-group m-b-3">
                                        <label className="ep-label" htmlFor="txt_groups_name">{resourceProvider.read('groups')}</label>
                                        <input className="ep-textbox" id="txt_groups_name" readOnly value={homologationGroup.group.name} />
                                    </div>
                                </div>
                                <div className="col-md-3">
                                    <div className="ep-control-group m-b-3">
                                        <span className="ep-label">{resourceProvider.read('objects')}</span>
                                        <button className="ep-btn ep-btn--link" onClick={this.props.openObjectsFlyout}>
                                            {homologationObjectTypes && homologationObjectTypes.length > 0 ? <><i className="far fa-eye m-r-1" />
                                                {`${homologationObjectTypes.length} ${resourceProvider.read('objects')}`}</> :
                                                <><i className="fas fa-plus-square m-r-1" />{resourceProvider.read('addObjects')}</>}
                                        </button>
                                    </div>
                                </div>
                            </div>
                            <div className="p-x-3 clearfix">
                                <span className="float-r"><button className="ep-btn" onClick={this.props.showHomologationDataModal}>
                                    <i className="fas fa-plus-square m-r-1" /><span className="ep-btn__txt">{resourceProvider.read('addRecord')}</span></button></span>
                            </div>
                        </div>
                        <HomologationsDataGrid />
                    </section>
                </div>
                <ObjectsFlyout name="objectsFlyout" />
            </section>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    const homologationGroup = state.homologations.homologationGroup;
    const isValid = homologationGroup.homologationObjectTypes && homologationGroup.homologationObjectTypes.length > 0 &&
        state.grid.homologationGroupData && state.grid.homologationGroupData.items.length > 0;
    return {
        homologationGroup,
        homologationGroupDataMappings: state.grid.homologationGroupData ? state.grid.homologationGroupData.items : [],
        isValid,
        mode: state.homologations.mode,
        refreshToggler: state.homologations.refreshToggler,
        homologationGroupToggler: state.homologations.homologationGroupToggler,
        objectTypesToggler: state.homologations.objectTypesToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        showHomologationDataModal: () => {
            dispatch(openModal('addHomologationData', constants.Modes.Create));
        },
        openObjectsFlyout: () => {
            dispatch(openFlyout('objectsFlyout'));
        },
        enableDisableSubmit: disabled => {
            dispatch(enableDisablePageAction('submit', disabled));
        },
        initUpdateHomologationGroup: () => {
            const homologationGroupId = navigationService.getParamByName('homologationGroupId');
            if (utilities.checkIfNumber(homologationGroupId)) {
                dispatch(requestHomologationGroup(homologationGroupId));
            }
        },
        getDataMappings: () => {
            const homologationGroupId = navigationService.getParamByName('homologationGroupId');
            if (utilities.checkIfNumber(homologationGroupId)) {
                dispatch(requestHomologationDataMappings(homologationGroupId));
            }
        },
        requestCreateUpdateHomologationGroup: homologationGroup => {
            dispatch(requestCreateUpdateHomologationGroup(homologationGroup));
        },
        showNotification: () => {
            dispatch(showSuccess(resourceProvider.read('homologationGroupSavedSuccessfully')));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(HomologationDetails);
