import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { utilities } from '../../../../common/services/utilities';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';
import GridNoData from '../../../../common/components/grid/gridNoData.jsx';
import { receiveSaveManualMovementsReset, requestDeltaNodeMovements, saveManualMovements } from '../actions';
import { dateService } from '../../../../common/services/dateService';
import { constants } from '../../../../common/services/constants';
import { openMessageModal } from '../../../../common/actions';


export class AddManualMovements extends React.Component {
    constructor() {
        super();
        this.confirm = this.confirm.bind(this);
    }

    confirm() {
        this.props.setSaveManualMovements(this.props.deltaNodeId, this.props.manualMovements.map(x => x.movementTransactionId));
    }

    render() {
        return (
            <>
                <div id="AddManualMovements_Modal" className="ep-modal__content">
                    <p><span className="fw-bold">{resourceProvider.read('addManualMovementsDeltaNodeInfo')}</span></p>
                    <div className="ep-control-group">
                        <section className="ep-table-wrap">
                            <div className="ep-table ep-table--alt-row ep-table--mh120 ep-table--h170 ep-table--nofilter" id="tbl_manual_movements">
                                <table>
                                    <colgroup>
                                        <col style={{ width: '10%' }} />
                                        <col style={{ width: '12.5%' }} />
                                        <col style={{ width: '12.5%' }} />
                                        <col style={{ width: '12.5%' }} />
                                        <col style={{ width: '12.5%' }} />
                                        <col style={{ width: '10%' }} />
                                        <col style={{ width: '10%' }} />
                                        <col style={{ width: '10%' }} />
                                    </colgroup>
                                    <thead>
                                        <tr>
                                            <th className=" fw-bold">{resourceProvider.read('movementType')}</th>
                                            <th className=" fw-bold">{resourceProvider.read('sourceNode')}</th>
                                            <th className=" fw-bold">{resourceProvider.read('destinationNode')}</th>
                                            <th className=" fw-bold">{resourceProvider.read('sourceProduct')}</th>
                                            <th className=" fw-bold">{resourceProvider.read('destinationProduct')}</th>
                                            <th className=" fw-bold">{resourceProvider.read('owner')}</th>
                                            <th className=" fw-bold">{resourceProvider.read('OwnerAmount')}</th>
                                            <th className=" fw-bold">{resourceProvider.read('units')}</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {
                                            this.props.manualMovements.map((item, index) => {
                                                const sourceNode = item.movementSource ? item.movementSource.sourceNode : null;
                                                const destinationNode = item.movementDestination ? item.movementDestination.destinationNode : null;
                                                const sourceProduct = item.movementSource ? item.movementSource.sourceProduct : null;
                                                const destinationProduct = item.movementDestination ? item.movementDestination.destinationProduct : null;

                                                return (
                                                    item.owners && item.owners.map((item2, index2) => {
                                                        return (
                                                            <tr key={`row-${index}.${index2}`}>
                                                                <td>{item.movementType ? item.movementType.name : ''}</td>
                                                                <td>{sourceNode && sourceNode.name }</td>
                                                                <td>{destinationNode && destinationNode.name}</td>
                                                                <td>{sourceProduct && sourceProduct.name}</td>
                                                                <td>{destinationProduct && destinationProduct.name}</td>
                                                                <td>{item2.ownerElement.name}</td>
                                                                <td>{item2.ownershipValue}</td>
                                                                <td>{item2.ownershipValueUnit}</td>
                                                            </tr>);
                                                    })
                                                );
                                            })
                                        }
                                    </tbody>
                                </table>
                                {this.props.manualMovements.length === 0 && <GridNoData {...this.props} />}
                            </div>
                        </section>
                    </div>
                </div>
                <ModalFooter config={footerConfigService.getCommonConfig('addManualMovements',
                    { onAccept: this.confirm, acceptText: 'add2', disableAccept: this.props.manualMovements.length === 0 })} />
            </>
        );
    }

    componentDidMount() {
        const initialDate = dateService.parseToPBIDateString(this.props.initialDateShort, constants.DateFormat.ShortDate);
        const finalDate = dateService.parseToPBIDateString(this.props.finalDateShort, constants.DateFormat.ShortDate);
        this.props.getRequestDeltaNodeMovements(initialDate, finalDate, this.props.nodeId);
        this.props.init();
    }

    componentDidUpdate(prevProps) {
        if (prevProps.isSaveForm === null && this.props.isSaveForm) {
            this.props.closeModal();
        }
        if (prevProps.isSaveForm === null && this.props.isSaveForm === false) {
            this.props.closeModal();
            this.props.showTechnicalError(resourceProvider.read('reportsError'), resourceProvider.read('error'), false);
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        ...state.report.officialDeltaNode.filters,
        manualMovements: state.report.officialDeltaNode.manualMovements || [],
        isSaveForm: state.report.officialDeltaNode.isSaveForm,
        isOpen: state.modal.isOpen
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        init: () => {
            dispatch(receiveSaveManualMovementsReset());
        },
        getRequestDeltaNodeMovements: (startTime, endTime, nodeId) => {
            dispatch(requestDeltaNodeMovements(startTime, endTime, nodeId));
        },
        setSaveManualMovements: (deltaNodeId, manualMovementsId) => {
            dispatch(saveManualMovements(deltaNodeId, manualMovementsId));
        },
        showTechnicalError: (message, title, canCancel) => {
            dispatch(openMessageModal(message, { title, canCancel }));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(AddManualMovements);
