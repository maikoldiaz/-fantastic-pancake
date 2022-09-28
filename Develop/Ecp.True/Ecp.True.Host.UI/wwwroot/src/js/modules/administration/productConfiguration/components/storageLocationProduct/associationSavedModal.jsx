import React from 'react';
import { connect } from 'react-redux';
import ModalFooter from './../../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from './../../../../../common/services/footerConfigService';
import { navigationService } from './../../../../../common/services/navigationService';
import { resourceProvider } from './../../../../../common/services/resourceProvider';
import { closeModal } from './../../../../../common/actions';
import { constants } from '../../../../../common/services/constants.js';

export class AssociationSavedModal extends React.Component {
    constructor() {
        super();
    }

    render() {
        const { associations } = this.props;
        const associationsToSaved = associations.length;
        const associationsSaved = associations.filter(association =>
            association.status === constants.InfoCreationStatus.Created).length;
        const associationsUnsaved = associations.filter(association =>
            association.status === constants.InfoCreationStatus.Duplicated ||
            association.status === constants.InfoCreationStatus.Error);

        const title = resourceProvider.readFormat('saveCostCenterWithDuplicatesMessage', [associationsSaved, associationsToSaved]);

        return (<>
            <div className="ep-body__content">
                <div className="row">
                    <div className="col-md-12">
                        <div className="text-center">
                            <label className="ep-label">{title}:</label>
                        </div>
                    </div>
                    <div className="col-md-12">
                        <div className="text-center">
                            <ul className="ep-section__header">
                                {associationsUnsaved && associationsUnsaved.map((association, i) =>
                                    (
                                        <li key={`associations_${i}`}>
                                            <div className="row">
                                                <div className="col-md-6">
                                                    <label className="ep-label m-b-0 m-r-2 float-r">{resourceProvider.read('logisticsCenter')}:</label>
                                                </div>
                                                <div className="col-md-6">
                                                    <span className="ep-data fw-bold text-left">{association.logisticCenterName}</span>
                                                </div>
                                            </div>
                                            <div className="row">
                                                <div className="col-md-6">
                                                    <label className="ep-label m-b-0 m-r-2 float-r">{resourceProvider.read('storageLocation')}:</label>
                                                </div>
                                                <div className="col-md-6">
                                                    <span className="ep-data fw-bold text-left">{association.storageLocationName}</span>
                                                </div>
                                            </div>
                                            <div className="row">
                                                <div className="col-md-6">
                                                    <label className="ep-label m-b-0 m-r-2 float-r">{resourceProvider.read('productSapName')}:</label>
                                                </div>
                                                <div className="col-md-6">
                                                    <span className="ep-data fw-bold text-left">{association.productName}
                                                        {
                                                            <span className="fas--error"> {
                                                                association.status === constants.InfoCreationStatus.Duplicated
                                                                    ? resourceProvider.read('duplicate')
                                                                    : undefined }</span>
                                                        }
                                                    </span>
                                                </div>
                                            </div>
                                            <div className="row">
                                                <div className="col-md-12">
                                                    <span className="ep-data fw-bold fas--error">{
                                                        association.status === constants.InfoCreationStatus.Error
                                                            ? resourceProvider.read('outdatedInformation')
                                                            : undefined }
                                                    </span>
                                                </div>
                                            </div>
                                            <br />
                                        </li>)
                                )
                                }
                            </ul>
                        </div>
                    </div>
                    <div className="col-md-12">
                        <div className="text-center">
                            <label className="ep-label">{resourceProvider.read('seeMessageAssociations')}</label>
                        </div>
                    </div>
                </div>
            </div >
            <ModalFooter config={footerConfigService.getAcceptConfig('associationSavedModal', {
                acceptType: 'button',
                acceptText: 'accept',
                onAccept: () => {
                    this.props.closeModal();
                    navigationService.navigateTo('manage');
                }
            })} />
        </>);
    }
}

const mapStateTopProps = state => {
    return {
        associations: state.products.associationsCreated
    };
};

const mapDispatchToProps = dispatch => {
    return {
        closeModal: () => {
            dispatch(closeModal());
        }
    };
};

export default connect(mapStateTopProps, mapDispatchToProps)(AssociationSavedModal);
