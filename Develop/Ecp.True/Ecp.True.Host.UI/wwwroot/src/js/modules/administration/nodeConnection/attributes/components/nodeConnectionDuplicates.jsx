import React from 'react';
import { connect } from 'react-redux';
import ModalFooter from '../../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../../common/services/footerConfigService';
import { navigationService } from '../../../../../common/services/navigationService';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { closeModal } from '../../../../../common/actions';

export class NodeConnectionDuplicates extends React.Component {
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
                                        <>
                                            <label className="ep-label m-b-0 m-r-2">
                                                {resourceProvider.read('sourceSegment')}:
                                            </label>
                                            <span className="ep-data fw-bold">{d.sourceSegmentName}</span> &nbsp;
                                        </>
                                        <>
                                            <label className="ep-label m-b-0 m-r-2">
                                                {resourceProvider.read('sourceNode')}:
                                            </label>
                                            <span className="ep-data fw-bold">{d.sourceNodeName}</span> &nbsp;
                                        </>
                                        <>
                                            <label className="ep-label m-b-0 m-r-2">
                                                {resourceProvider.read('destinationSegment')}:
                                            </label>
                                            <span className="ep-data fw-bold">{d.destinationSegmentName}</span> &nbsp;
                                        </>
                                        <>
                                            <label className="ep-label m-b-0 m-r-2">
                                                {resourceProvider.read('destinationNode')}:
                                            </label>
                                            <span className="ep-data fw-bold">{d.destinationNodeName}</span> &nbsp;
                                        </>
                                        <>
                                            <label className="ep-label m-b-0 m-r-2">
                                                {resourceProvider.read('state')}:
                                            </label>
                                            <span className="ep-data fw-bold">
                                                {resourceProvider.read('duplicate')} {resourceProvider.read('active')}
                                            </span>
                                        </>
                                    </li>)
                            )}
                        </ul>
                    </div>
                    <div style={{ display: 'flex', justifyContent: 'center' }}>
                        <label className="ep-label">{resourceProvider.read('newNodeConnectionDuplicates')}</label>
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

export default connect(mapStateTopProps, mapDispatchToProps)(NodeConnectionDuplicates);
