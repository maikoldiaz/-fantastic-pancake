import React from 'react';
import Wizard from '../../../../common/components/wizard/wizard.jsx';
import LogisticsCriteria from '../../../transportBalance/logistics/components/logisticsCriteria.jsx';
import OfficialLogisticsPeriod from './officialLogisticsPeriod.jsx';
import LogisticsValidations from '../../../transportBalance/logistics/components/logisticsValidations.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider.js';

const officialLogisticsWizardConfig = () => {
    return {
        wizardName: 'createOfficialLogisticsWizard',
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
                component: OfficialLogisticsPeriod
            },
            {
                title: resourceProvider.read('validation'),
                component: LogisticsValidations
            }
        ],
        disabledWizardStepsClick: true
    };
};

export default class OfficialLogisticsWizard extends React.Component {
    render() {
        return (
            <Wizard wizardName="createOfficialLogisticsWizard" config={officialLogisticsWizardConfig()} {...this.props} />
        );
    }
}
