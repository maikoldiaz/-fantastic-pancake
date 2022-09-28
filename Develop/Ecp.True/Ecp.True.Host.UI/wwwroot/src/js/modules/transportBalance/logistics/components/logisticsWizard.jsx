import React from 'react';
import Wizard from '../../../../common/components/wizard/wizard.jsx';
import LogisticsCriteria from './logisticsCriteria.jsx';
import LogisticsPeriod from './logisticsPeriod.jsx';
import LogisticsValidations from './logisticsValidations.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider.js';

const logisticsWizardConfig = () => {
    return {
        wizardName: 'createLogistics',
        activeStep: 1,
        isPage: false,
        clickable: false,
        wizardSteps: [
            {
                title: resourceProvider.read('criteria'),
                component: LogisticsCriteria
            },
            {
                title: resourceProvider.read('period'),
                component: LogisticsPeriod
            },
            {
                title: resourceProvider.read('validation'),
                component: LogisticsValidations
            }
        ],
        disabledWizardStepsClick: true
    };
};

export default class LogisticsWizard extends React.Component {
    render() {
        return (
            <Wizard wizardName="createLogistics" config={logisticsWizardConfig()} {...this.props} />
        );
    }
}
