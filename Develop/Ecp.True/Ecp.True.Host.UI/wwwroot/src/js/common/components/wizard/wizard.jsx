import React from 'react';
import { connect } from 'react-redux';
import WizardConfig from './wizardConfig.js';
import { initWizard, wizardNextStep, wizardPrevStep, wizardSetStep } from '../../actions.js';
import { resourceProvider } from '../../services/resourceProvider';
import classNames from 'classnames/bind';

class Wizard extends React.Component {
    constructor(props) {
        super(props);
        this.config = new WizardConfig(this.props.config);
        this.onStepClick = this.onStepClick.bind(this);
    }

    componentDidMount() {
        const wizardName = this.config.getName();
        this.props.setActiveStep(wizardName, this.config.getActiveStep(wizardName), this.config.getWizardTotalSteps(wizardName));
    }

    onStepClick(name, index) {
        if (this.config.clickable(name)) {
            this.props.goToStep(name, index);
        }
    }

    render() {
        if (!this.props.wizardState) {
            return null;
        }

        const wizardName = this.config.getName();
        const steps = this.config.getWizardSteps(wizardName);
        const CurrentWizardComponent = this.config.getCurrentComponent(wizardName, this.props.wizardState.activeStep);
        const HeaderLabelComponent = this.config.getHeaderLabelComponent(wizardName, this.props.wizardState.activeStep);
        const activeStep = this.props.wizardState.activeStep;
        const isPage = this.config.isPage();

        return (
            <section className={classNames('ep-wizard', { ['ep-wizard--page']: isPage })} id={`sec_${wizardName}_wzd`}>
                <header className="ep-wizard__header">
                    <ul className="ep-wizard__lst">
                        {steps.map((step, stepIndex) => {
                            return (
                                <li id={`li_${step.title}`} key={`itm-${step.title}`} className="ep-wizard__lst-itm">
                                    <a id={`lnk_${step.title}`} key={`lnk-${step.title}`}
                                        className={classNames('ep-wizard__lst-lnk', {
                                            ['ep-wizard__lst-lnk--active']: (activeStep === (stepIndex + 1)),
                                            ['ep-wizard__lst-lnk--visited']: (activeStep > (stepIndex + 1))
                                        })}
                                        onClick={() => this.onStepClick(wizardName, stepIndex + 1)}>{resourceProvider.read(step.title)}</a>
                                </li>
                            );
                        })}
                    </ul>
                    <div className="ep-wizard__info">
                        {HeaderLabelComponent && <HeaderLabelComponent />}
                    </div>
                </header>
                <div className="ep-wizard__body">
                    <CurrentWizardComponent {...this.props} />
                </div>
            </section>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = (state, ownProps) => {
    return {
        wizardState: state.wizard[ownProps.wizardName]
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        setActiveStep: (name, step, totalSteps) => {
            dispatch(initWizard(name, step, totalSteps));
        },
        onNext: name => {
            dispatch(wizardNextStep(name));
        },
        onPrev: name => {
            dispatch(wizardPrevStep(name));
        },
        goToStep: (name, step) => {
            dispatch(wizardSetStep(name, step));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(Wizard);
