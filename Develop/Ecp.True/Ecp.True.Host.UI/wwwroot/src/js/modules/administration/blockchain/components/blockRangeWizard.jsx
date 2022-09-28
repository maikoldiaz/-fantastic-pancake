import React from 'react';
import Wizard from '../../../../common/components/wizard/wizard.jsx';
import BlockRangeSearch from './blockRangeSearch.jsx';
import BlockRangeGrid from './blockRangeGrid.jsx';

const blockRangeWizardConfig = () => {
    return {
        wizardName: 'blockRangeWizard',
        activeStep: 1,
        isPage: true,
        clickable: false,
        wizardSteps: [
            {
                title: 'start',
                component: BlockRangeSearch
            },
            {
                title: 'blockchainEventDetails',
                component: BlockRangeGrid
            }
        ],
        disabledWizardStepsClick: true
    };
};

export default class BlockRangeWizard extends React.Component {
    render() {
        return (
            <section className="ep-content p-t-3">
                <Wizard wizardName="blockRangeWizard" config={blockRangeWizardConfig()} {...this.props} />
            </section>
        );
    }
}
