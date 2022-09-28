import React from 'react';
import Wizard from '../../../../common/components/wizard/wizard.jsx';
import BlockSearch from './blockSearch.jsx';
import TransactionDetails from './transactionDetails.jsx';

const blockWizardConfig = () => {
    return {
        wizardName: 'blockWizard',
        activeStep: 1,
        isPage: true,
        clickable: false,
        wizardSteps: [
            {
                title: 'start',
                component: BlockSearch
            },
            {
                title: 'transactionDetails',
                component: TransactionDetails
            }
        ],
        disabledWizardStepsClick: true
    };
};

export default class BlockWizard extends React.Component {
    render() {
        return (
            <Wizard wizardName="blockWizard" config={blockWizardConfig()} {...this.props} />
        );
    }
}
