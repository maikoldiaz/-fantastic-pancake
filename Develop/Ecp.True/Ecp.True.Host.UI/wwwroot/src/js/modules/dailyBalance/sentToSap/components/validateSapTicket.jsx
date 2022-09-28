import React from 'react';
import { connect } from 'react-redux';
import classNames from 'classnames/bind';
import { resetSapTicketValidations } from '../actions';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { dateService } from '../../../../common/services/dateService';
import SectionFooter from '../../../../common/components/footer/sectionFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import { openMessageModal } from '../../../../common/actions';
import { constants } from '../../../../common/services/constants';
import { Notification } from '../../../../common/components/notification/notification.jsx';
import { utilities } from '../../../../common/services/utilities';
import { navigationService } from '../../../../common/services/navigationService';

class ValidateSapTicket extends React.Component {
    constructor() {
        super();
        this.confirm = this.confirm.bind(this);
    }

    confirm() {
        this.props.onNext(this.props.config.wizardName);
    }

    buildValidation(name, propertyValidation, isRestrictive, nodeHasStatus) {
        const invalidNodes = this.props.validations
            .filter(node => !node[propertyValidation])
            .map(node => ({
                name: node.nodeName,
                date: this.props.ticket.scenarioTypeId === constants.ScenarioTypeId.operational ?
                    dateService.capitalize(node.operationDate) : `${dateService.capitalize(node.startDate)} | ${dateService.capitalize(node.endDate)}`,
                status: nodeHasStatus ? (node.statusName || '') : null
            }));

        return {
            isSuccessful: this.props.validations.every(node => node[propertyValidation]),
            isRestrictive: isRestrictive
                ? this.props.validations.some(node => !node[propertyValidation])
                : this.props.validations.every(node => !node[propertyValidation]),
            title: `${name}Lbl`,
            successDescription: name,
            errorDescription: `${name}Error`,
            errorMessage: `${name}Restrictive`,
            nodes: invalidNodes
        };
    }

    getValidations() {
        return [
            this.buildValidation('approvedNodes', 'isApproved', false, true),
            this.buildValidation('sendToSapIsActive', 'isEnabledForSendToSap', false, true),
            this.buildValidation('predecessorIsActiveInSap', 'predecessorIsApproved', true, false)
        ];
    }

    renderTable(nodes) {
        return (
            <>
                <p className="m-t-4 m-b-0 fw-bold">
                    {resourceProvider.read('totalNodes')}: {nodes.length}
                </p>
                < section className="ep-table-wrap" >
                    <div className="ep-table ep-table--smpl ep-table--alt-row ep-scroll-table ep-sticky-header-table">
                        <table>
                            <colgroup>
                                <col style={{ width: '25%' }} />
                                <col style={{ width: '25%' }} />
                                {!utilities.isNullOrUndefined(nodes[0].status) && <col style={{ width: '25%' }} />}
                            </colgroup>
                            <thead>
                                <tr>
                                    <th className=" fw-bold">{resourceProvider.read('nodeName')}</th>
                                    <th className=" fw-bold">{resourceProvider.read('date')}</th>
                                    {!utilities.isNullOrUndefined(nodes[0].status) && <th className=" fw-bold">{resourceProvider.read('state')}</th>}
                                </tr>
                            </thead>
                            <tbody>
                                {nodes.map((item, index) => {
                                    return (
                                        <tr key={`row-${index}`}>
                                            <td>{item.name}</td>
                                            <td>{item.date}</td>
                                            {!utilities.isNullOrUndefined(item.status) && <td>{item.status}</td>}
                                        </tr>
                                    );
                                })}
                            </tbody>
                        </table>
                    </div>
                </section >
            </>
        );
    }

    renderAlert(validations) {
        const restrictiveValidations = validations.filter(validation => validation.isRestrictive);
        const isRestrictiveMessage = restrictiveValidations.length > 0;
        const isWarningMessage = !isRestrictiveMessage && validations.some(validation => !validation.isSuccessful);

        const status = isRestrictiveMessage ? constants.NotificationType.Error : constants.NotificationType.Warning;
        const title = isRestrictiveMessage ? `${resourceProvider.read('error')}:` : `${resourceProvider.read('noteThat')}:`;
        const message = isRestrictiveMessage
            ? restrictiveValidations.map((validation, index) => (<div key={index} className={classNames({
                ['d-inline-block']: restrictiveValidations.length === index + 1
            })}>{resourceProvider.read(validation.errorMessage)}</div>))
            : resourceProvider.read('sapValidationWarning');

        return (
            <Notification
                className="p-init m-a-5"
                state={status}
                title={title}
                message={message}
                show={isRestrictiveMessage || isWarningMessage} />
        );
    }

    render() {
        const validations = this.getValidations();
        const isValidationsPassed = validations && validations.every(item => !item.isRestrictive);

        return (
            <section className="full-height">
                <div className="ep-section__body ep-section__body--auto">
                    {this.renderAlert(validations)}
                    <section className="ep-section__content-w500 m-t-3 ep-validation">
                        <ul className="ep-validation__lst">
                            {validations &&
                                validations.map((item, index) => (
                                    <li key={`col-${index}`} className="ep-validation__itm">
                                        <h1 className="ep-validation__itm-title">
                                            <span className={classNames('ep-validation__itm-icn', {
                                                ['success']: item.isSuccessful && !item.isRestrictive,
                                                ['warning']: !item.isSuccessful && !item.isRestrictive,
                                                ['error']: !item.isSuccessful && item.isRestrictive
                                            })} >
                                                {item.isSuccessful && !item.isRestrictive && <i className="fas fa-check" />}
                                                {!item.isSuccessful && !item.isRestrictive && <i className="fas fa-exclamation-triangle fas--warning fs-20" />}
                                                {!item.isSuccessful && item.isRestrictive && <i className="fas fa-times" />}
                                            </span>
                                            {resourceProvider.read(item.title)}
                                        </h1>
                                        <p className="ep-validation__itm-desc">
                                            {item.isSuccessful ? resourceProvider.read(item.successDescription) : resourceProvider.read(item.errorDescription)}
                                        </p>
                                        {item.nodes.length > 0 && this.renderTable(item.nodes)}
                                    </li>
                                ))}
                        </ul>
                    </section>
                    <SectionFooter floatRight={true} config={footerConfigService.getCommonConfig('sapProcess', {
                        onAccept: this.confirm,
                        onCancel: this.props.cancelValidation,
                        disableAccept: !isValidationsPassed,
                        acceptText: 'next',
                        acceptClassName: 'ep-btn'
                    })} />
                </div>
            </section>
        );
    }

    componentDidUpdate(prevProps) {
        if (this.props.receiveToggler !== prevProps.receiveToggler) {
            navigationService.navigateToModule('senttosap/manage');
        }

        if ((prevProps.failureToggler !== this.props.failureToggler) && (!utilities.isNullOrUndefined(this.props.failureToggler))) {
            this.props.showTechnicalError(resourceProvider.read('systemErrorMessage'), resourceProvider.read('error'), false);
            navigationService.navigateToModule('senttosap/manage');
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        ticket: state.sendToSap.ticket,
        validations: state.sendToSap.validations,
        receiveToggler: state.sendToSap.receiveToggler,
        failureToggler: state.sendToSap.failureToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    return {
        cancelValidation: () => {
            dispatch(resetSapTicketValidations());
            ownProps.goToStep('sendToSapWizard', 1);
        },
        showTechnicalError: (message, title, canCancel) => {
            dispatch(openMessageModal(message, { title: title, canCancel: canCancel }));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(ValidateSapTicket);
