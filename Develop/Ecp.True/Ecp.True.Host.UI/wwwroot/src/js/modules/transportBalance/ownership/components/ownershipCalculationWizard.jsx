import React from 'react';
import Wizard from '../../../../common/components/wizard/wizard.jsx';
import OwnershipCalculationCriteria from './ownershipCalculationCriteria.jsx';
import OwnershipCalculationValidations from './ownershipCalculationValidations.jsx';
import OwnershipCalculationConfirmation from './ownershipCalculationConfirmation.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider.js';

const ownershipCalculationWizardConfig = () => {
    return {
        wizardName: 'ownershipCalculation',
        activeStep: 1,
        clickable: false,
        wizardSteps: [
            {
                title: resourceProvider.read('criteria'),
                component: OwnershipCalculationCriteria
            },
            {
                title: resourceProvider.read('validations'),
                component: OwnershipCalculationValidations
            },
            {
                title: resourceProvider.read('confirmation'),
                component: OwnershipCalculationConfirmation
            }
        ],
        disabledWizardStepsClick: true
    };
};

export default class OwnershipCalculationWizard extends React.Component {
    render() {
        return (
            <Wizard wizardName="ownershipCalculation" config={ownershipCalculationWizardConfig()} {...this.props}/>
        );
    }
}
