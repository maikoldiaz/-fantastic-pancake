import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { dateService } from '../../../../common/services/dateService';
import { initInventoriesValidations, requestInventoriesValidations, requestFirstTimeNodes } from '../actions';
import { constants } from '../../../../common/services/constants';
import { showWarning, hideNotification, wizardNextStep } from '../../../../common/actions';
import classNames from 'classnames/bind';
import SectionFooter from '../../../../common/components/footer/sectionFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';

export class ValidateInitialInventory extends React.Component {
    showWarning() {
        this.props.showWarning(resourceProvider.read('newNodesValMesDesc'), resourceProvider.read('newNodesValMesTitle'));
    }
    render() {
        const validations = this.props.validations;
        const nodesWithInventoriesWithoutOwners = validations[constants.InventoriesValidations.InitialInventories];
        const newNodes = validations[constants.InventoriesValidations.NewNodes];

        const isNodesWithInventoriesWithoutOwners = nodesWithInventoriesWithoutOwners && nodesWithInventoriesWithoutOwners.length > 0;
        const isNewNodes = newNodes && newNodes.length > 0;

        if (isNewNodes) {
            this.showWarning();
        } else {
            this.props.hideWarning();
        }

        return (
            <div className="ep-section">
                <div className="ep-section__body ep-section__body--f">
                    <h1 className="fs-18 fw-bold m-t-0 m-b-8 text-center">
                        {resourceProvider.readFormat('initialInventoriesValidationMessage',
                            [ dateService.capitalize(this.props.ticket.startDate),
                                dateService.capitalize(this.props.ticket.endDate), this.props.ticket.categoryElementName])
                        }
                    </h1>
                    <section className="ep-section__content-w500 m-t-6">
                        <ul className="ep-validation__lst">
                            {nodesWithInventoriesWithoutOwners &&
                            <li className="ep-validation__itm">
                                <h1 className="ep-validation__itm-title"><span className={classNames('ep-validation__itm-icn',
                                    { ['success']: !isNodesWithInventoriesWithoutOwners, ['error']: isNodesWithInventoriesWithoutOwners })}>
                                    {isNodesWithInventoriesWithoutOwners ? <i className="fas fa-times" /> : <i className="fas fa-check" />}</span>
                                {resourceProvider.read('validationInitialInventoriesLbl')}</h1>
                                <p className="ep-validation__itm-desc">
                                    {resourceProvider.read(isNodesWithInventoriesWithoutOwners ? 'validationInitialInventoriesError' : 'validationInitialInventoriesSuccess')}</p>
                                {isNodesWithInventoriesWithoutOwners &&
                                <div className="ep-validation__itm-body">
                                    <div className="ep-label-wrap">
                                        <label className="ep-label">{resourceProvider.read('totalNodes')}</label> :
                                        <span className="ep-data m-l-2 fw-600 fc-green">{nodesWithInventoriesWithoutOwners.length}</span>
                                    </div>
                                    <section className="ep-table-wrap">
                                        <div className="ep-table ep-table--smpl ep-table--alt-row ep-table--h170" id="tbl_validate_nodesWithoutOwnership">
                                            <table>
                                                <colgroup>
                                                    <col style={{ width: '50%' }}/>
                                                    <col style={{ width: '50%' }}/>
                                                </colgroup>
                                                <thead>
                                                    <tr>
                                                        <th>{resourceProvider.read('node')}</th>
                                                        <th><span className="float-r">{resourceProvider.read('initialInventoryDate')}</span></th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    {
                                                        nodesWithInventoriesWithoutOwners.map((item, index) => {
                                                            return (
                                                                <tr key={`row-${index}`}>
                                                                    <td>{item.nodeName}</td>
                                                                    <td><span className="float-r">{dateService.capitalize(item.inventoryDate)}</span></td>
                                                                </tr>
                                                            );
                                                        })
                                                    }
                                                </tbody>
                                            </table>
                                        </div>
                                    </section>
                                </div>
                                }
                            </li>}
                            {newNodes &&
                            <li className="ep-validation__itm">
                                <h1 className="ep-validation__itm-title"><span className={classNames('ep-validation__itm-icn',
                                    { ['success']: !isNewNodes, ['warning']: isNewNodes })}>
                                    {isNewNodes ? <i className="fas fa-exclamation-triangle fas--warning fs-20" /> : <i className="fas fa-check" />}</span>
                                {resourceProvider.read('validationNewNodesLbl')}</h1>
                                <p className="ep-validation__itm-desc">
                                    {resourceProvider.read(isNewNodes ? 'validationNewNodesError' : 'validationNewNodesSuccess')}</p>
                                {isNewNodes &&
                                <div className="ep-validation__itm-body">
                                    <div className="ep-label-wrap">
                                        <label className="ep-label">{resourceProvider.read('totalNodes')}</label> : <span className="ep-data m-l-2 fw-600 fc-green">{newNodes.length}</span>
                                    </div>
                                    <section className="ep-table-wrap">
                                        <div className="ep-table ep-table--smpl ep-table--h170 ep-table--alt-row" id="tbl_validate_nodesWithoutInitialInventory">
                                            <table>
                                                <colgroup>
                                                    <col style={{ width: '50%' }}/>
                                                    <col style={{ width: '50%' }}/>
                                                </colgroup>
                                                <thead>
                                                    <tr>
                                                        <th>{resourceProvider.read('node')}</th>
                                                        <th><span className="float-r">{resourceProvider.read('initialInventoryDate')}</span></th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    {
                                                        newNodes.map((item, index) => {
                                                            return (
                                                                <tr key={`row-${index}`}>
                                                                    <td>{item.nodeName}</td>
                                                                    <td><span className="float-r">{dateService.capitalize(item.inventoryDate)}</span></td>
                                                                </tr>
                                                            );
                                                        })
                                                    }
                                                </tbody>
                                            </table>
                                        </div>
                                    </section>
                                </div>
                                }
                            </li>}
                        </ul>
                    </section>
                </div>
                <SectionFooter floatRight={true} config={footerConfigService.getCommonConfig('validateInitialInventory',
                    {
                        acceptActions: [wizardNextStep(this.props.config.wizardName)], onCancel: this.props.cancelCutOffValidation, disableAccept: (isNodesWithInventoriesWithoutOwners),
                        acceptText: 'next', acceptClassName: 'ep-btn'
                    })} />
            </div>
        );
    }

    componentDidMount() {
        const ticketInfo = {
            categoryElementId: this.props.ticket.categoryElementId,
            startDate: this.props.ticket.startDate,
            endDate: this.props.ticket.endDate
        };

        this.props.getInventoriesValidations(ticketInfo);
        this.props.getFirstTimeNodes(ticketInfo);
    }

    componentWillUnmount() {
        this.props.hideWarning();
        this.props.resetInventoryValidations();
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        ticket: state.cutoff.operationalCut.ticket,
        validations: state.cutoff.operationalCut.inventoriesValidations
    };
};

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    return {
        getInventoriesValidations: ticket => {
            dispatch(requestInventoriesValidations(ticket));
        },
        resetInventoryValidations: () => {
            dispatch(initInventoriesValidations());
        },
        cancelCutOffValidation: () => {
            dispatch(initInventoriesValidations());
            ownProps.goToStep('operationalCut', 1);
        },
        showWarning: (message, title) => {
            dispatch(showWarning(message, false, title));
        },
        hideWarning: () =>{
            dispatch(hideNotification());
        },
        getFirstTimeNodes: ticket => {
            dispatch(requestFirstTimeNodes(ticket));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(ValidateInitialInventory);
