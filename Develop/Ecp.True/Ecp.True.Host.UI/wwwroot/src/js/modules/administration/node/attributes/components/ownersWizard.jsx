import React from 'react';
import Wizard from './../../../../../common/components/wizard/wizard.jsx';
import OwnersSelect from './ownersSelect.jsx';
import Ownership from './ownership.jsx';

const nodeAttributesWizardConfig = () => {
    return {
        wizardName: 'editNodeOwnership',
        activeStep: 1,
        wizardSteps: [
            {
                title: 'property',
                component: OwnersSelect
            },
            {
                title: 'propertyPercentages',
                component: Ownership
            }
        ]
    };
};

export default class OwnersWizard extends React.Component {
    render() {
        return (
            <section className="ep-modal__content p-a-0">
                <Wizard wizardName="editNodeOwnership" config={nodeAttributesWizardConfig()} {...this.props} />
            </section>
        );
    }
}
