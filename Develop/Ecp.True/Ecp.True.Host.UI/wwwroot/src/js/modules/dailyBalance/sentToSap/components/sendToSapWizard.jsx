import React from 'react';
import { connect } from 'react-redux';
import { utilities } from '../../../../common/services/utilities';
import Wizard from '../../../../common/components/wizard/wizard.jsx';
import InitSapFormTicket from './initSapFormTicket.jsx';
import ValidateSapTicket from './validateSapTicket.jsx';
import MovementsGrid from '../components/movementsGrid.jsx';
import CancelBatch from '../components/cancelBatch.jsx';
import FailMovementGrid from './FailMovementGrid.jsx';

const sendToSapWizardConfig = () => {
    return {
        wizardName: 'sendToSapWizard',
        activeStep: 1,
        isPage: true,
        clickable: false,
        wizardSteps: [
            {
                title: 'start',
                component: InitSapFormTicket
            },
            {
                title: 'nodesValidation',
                component: ValidateSapTicket
            },
            {
                title: 'failMovements',
                component: FailMovementGrid
            },
            {
                title: 'preview',
                component: MovementsGrid,
                headerLabelComponent: CancelBatch
            }
        ],
        disabledWizardStepsClick: true
    };
};

class SendToSapWizard extends React.Component {
    getConfiguration() {
        const config = sendToSapWizardConfig();
        if (!utilities.isNullOrUndefined(this.props.confirmWizard) && !utilities.isNullOrUndefined(this.props.confirmWizard.ticketId)) {
            config.activeStep = 4;
        }

        return config;
    }

    render() {
        const config = this.getConfiguration();

        return (
            <section className="ep-content p-t-3">
                <Wizard wizardName="sendToSapWizard" config={config} {...this.props} />
            </section>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        confirmWizard: state.sendToSap.confirmWizard
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, null, utilities.merge)(SendToSapWizard);
