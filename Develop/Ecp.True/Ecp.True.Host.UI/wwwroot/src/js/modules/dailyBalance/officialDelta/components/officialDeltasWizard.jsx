import React from 'react';
import Wizard from '../../../../common/components/wizard/wizard.jsx';
import InitOfficialDeltaTicket from './initialize.jsx';
import ValidateOfficialDeltaTicket from './validate.jsx';

const officialDeltaWizardConfig = () => {
    return {
        wizardName: 'officialDelta',
        activeStep: 1,
        isPage: true,
        clickable: false,
        wizardSteps: [
            {
                title: 'start',
                component: InitOfficialDeltaTicket
            },
            {
                title: 'validation',
                component: ValidateOfficialDeltaTicket
            }
        ],
        disabledWizardStepsClick: true
    };
};

export default class OfficialDeltasWizard extends React.Component {
    render() {
        return (
            <section className="ep-content p-t-3">
                <Wizard wizardName="officialDelta" config={officialDeltaWizardConfig()} />
            </section>
        );
    }
}
