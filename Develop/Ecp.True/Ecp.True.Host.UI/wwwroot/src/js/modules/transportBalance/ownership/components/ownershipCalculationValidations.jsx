import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { requestOwnershipCalculationValidations } from './../actions';
import { dateService } from '../../../../common/services/dateService';
import { utilities } from '../../../../common/services/utilities';
import classNames from 'classnames/bind';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import { wizardNextStep } from '../../../../common/actions';

const readErrorMessage = validation => {
    let validationMessage;
    const errorMessage = JSON.parse(validation.errorMessage);
    if (!utilities.isNullOrUndefined(errorMessage)) {
        validationMessage = validation.result ? resourceProvider.read(validation.name) :
            resourceProvider.readFormat(`${validation.name}Fail`, [errorMessage.Name, resourceProvider.read(errorMessage.Type)]);
    } else {
        validationMessage = validation.result ? resourceProvider.read(validation.name) : resourceProvider.read(`${validation.name}Fail`);
    }

    return validationMessage;
};

export class OwnershipCalculationValidations extends React.Component {
    componentDidMount() {
        this.props.getOwnershipCalculationValidations(this.props.ticket);
    }

    render() {
        const validations = this.props.validations;
        const isValidationsPassed = validations && validations.every(item => item.result);
        return (
            <>
                <section className="ep-modal__content">
                    <h1 className="fs-18 fw-bold m-t-0 m-b-8">
                        {resourceProvider.readFormat('owenershipValidationMessage',
                            [this.props.ticket.categoryElementName,
                                dateService.capitalize(this.props.ticket.startDate),
                                dateService.capitalize(this.props.ticket.endDate)])
                        }
                    </h1>
                    <section className="ep-validation">
                        <h1 className="ep-validation__header">{resourceProvider.read('executeElements')}</h1>
                        <ul className="ep-validation__lst">
                            {validations &&
                                validations.map((item, index) => (
                                    <li key={`col-${index}`} className="ep-validation__itm">
                                        <h1 className="ep-validation__itm-title"><span className={classNames('ep-validation__itm-icn', { ['success']: item.result, ['error']: !item.result })}>
                                            {item.result ? <i className="fas fa-check" /> : <i className="fas fa-times" />}</span>
                                        {resourceProvider.read(`${item.name}Lbl`)}</h1>
                                        <p className="ep-validation__itm-desc">{readErrorMessage(item)}</p>
                                    </li>
                                ))}
                        </ul>
                    </section>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('ownershipCalculationValidations',
                    { acceptActions: [wizardNextStep(this.props.config.wizardName)], acceptText: 'next', disableAccept: !isValidationsPassed })} />
            </>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        ticket: state.ownership.ticket,
        validations: state.ownership.validations
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        getOwnershipCalculationValidations: calculation => {
            dispatch(requestOwnershipCalculationValidations(calculation));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(OwnershipCalculationValidations);
