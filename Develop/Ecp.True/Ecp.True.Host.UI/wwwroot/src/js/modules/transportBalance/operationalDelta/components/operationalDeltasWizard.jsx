import React from 'react';
import Wizard from '../../../../common/components/wizard/wizard.jsx';
import InitDeltaTicket from './initDeltaTicket.jsx';
import PendingMovementsGrid from './pendingMovementsGrid.jsx';
import PendingInventoriesGrid from './pendingInventoriesGrid.jsx';

const cutOffWizardConfig = () => {
    return {
        wizardName: 'operationalDelta',
        activeStep: 1,
        isPage: true,
        clickable: false,
        wizardSteps: [
            {
                title: 'start',
                component: InitDeltaTicket
            },
            {
                title: 'pendingInventories',
                component: PendingInventoriesGrid
            },
            {
                title: 'pendingMovements',
                component: PendingMovementsGrid
            }
        ],
        disabledWizardStepsClick: true
    };
};

export default class OperationalDeltasWizard extends React.Component {
    render() {
        return (
            <section className="ep-content p-t-3">
                <Wizard wizardName="operationalDelta" config={cutOffWizardConfig()} />
            </section>
        );
    }
}
