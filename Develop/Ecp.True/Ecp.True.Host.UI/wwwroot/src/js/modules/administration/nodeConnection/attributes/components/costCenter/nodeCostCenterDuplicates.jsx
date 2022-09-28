import React from 'react';
import { connect } from 'react-redux';
import ModalFooter from '../../../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../../../common/services/footerConfigService';
import { navigationService } from '../../../../../../common/services/navigationService';
import { resourceProvider } from '../../../../../../common/services/resourceProvider';
import { closeModal } from '../../../../../../common/actions';

export class NodeCostCenterDuplicates extends React.Component {
    constructor() {
        super();
    }

    render() {
        const { totalToSaved, totalSaved, duplicates } = this.props;
        const title = resourceProvider.readFormat('saveCostCenterWithDuplicatesMessage', [totalSaved, totalToSaved]);

        return (<><div className="ep-body__content">
            <div className="row">
                <div className="col-md-12">
                    <div className="text-center">
                        <label className="ep-label">{title}</label>
                    </div>
                </div>
                <div className="col-md-12">
                    <div style={{ display: 'flex', justifyContent: 'center' }}>
                        <ul className="ep-section__header">
                            {duplicates && duplicates.map((d, i) =>
                                (
                                    <li className="ep-section__header-lbl" key={`duplicate_${i}`}>
                                        <label className="ep-label m-b-0 m-r-2">
                                            {resourceProvider.read('sourceCategoryElement')}:
                                        </label>
                                        <span className="ep-data fw-bold">{d.movementTypeName}</span> &nbsp;
                                        <label className="ep-label m-b-0 m-r-2">
                                            {resourceProvider.read('costCenter')}:
                                        </label>
                                        <span className="ep-data fw-bold">{d.costCenterName}</span> &nbsp;
                                        <label className="ep-label m-b-0 m-r-2">
                                            {resourceProvider.read('error')}:
                                        </label>
                                        <span className="ep-data fw-bold">
                                            {resourceProvider.read('duplicate')} {d.status ? resourceProvider.read('active') : resourceProvider.read('inActive')}
                                        </span>
                                    </li>)
                            )}
                        </ul>
                    </div>
                </div>
                <div className="col-md-12">
                    <div className="text-center">
                        <label className="ep-label">{ resourceProvider.read('costCenterModalMessage') }</label>
                    </div>
                </div>
            </div>
        </div >
        <ModalFooter config={footerConfigService.getAcceptConfig('nodeCostCenterDuplicates', {
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
    const { totalToSaved, totalUnsaved } = state.nodeConnection.nodeCostCenters;
    const currentTotalSaved = totalToSaved - totalUnsaved;
    return {
        duplicates: state.nodeConnection.nodeCostCenters.flatDuplicates,
        totalToSaved: state.nodeConnection.nodeCostCenters.totalToSaved,
        totalSaved: currentTotalSaved
    };
};

const mapDispatchToProps = dispatch => {
    return {
        closeModal: () => {
            dispatch(closeModal());
        }
    };
};

export default connect(mapStateTopProps, mapDispatchToProps)(NodeCostCenterDuplicates);
