import React from 'react';
import Wizard from '../../../../common/components/wizard/wizard.jsx';
import InitTicket from './initTicket.jsx';
import ValidateInitialInventory from './validateInitialInventory.jsx';
import ErrorsGrid from './errorsGrid.jsx';
import OfficialPointsGrid from './officialPointsGrid.jsx';
import UnbalancesGrid from './unbalancesGrid.jsx';
import WizardHeader from './wizardHeader.jsx';

const cutOffWizardConfig = () => {
    return {
        wizardName: 'operationalCut',
        activeStep: 1,
        isPage: true,
        clickable: false,
        wizardSteps: [
            {
                title: 'start',
                component: InitTicket,
                headerLabelComponent: WizardHeader
            },
            {
                title: 'validateInitialInventory',
                component: ValidateInitialInventory,
                headerLabelComponent: WizardHeader
            },
            {
                title: 'verifyMessaging',
                component: ErrorsGrid,
                headerLabelComponent: WizardHeader
            },
            {
                title: 'verifyOfficialPoints',
                component: OfficialPointsGrid,
                headerLabelComponent: WizardHeader
            },
            {
                title: 'checkConsistency',
                component: UnbalancesGrid,
                headerLabelComponent: WizardHeader
            }
        ],
        disabledWizardStepsClick: true
    };
};

export default class CutOffWizard extends React.Component {
    render() {
        return (
            <section className="ep-content p-t-3">
                <Wizard wizardName="operationalCut" config={cutOffWizardConfig()} />
            </section>
        );
    }
}
