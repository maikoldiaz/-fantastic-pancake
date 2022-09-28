export default class WizardConfig {
    constructor(config) {
        this.config = config;
    }

    getName() {
        return this.config.wizardName;
    }

    isPage() {
        return this.config.isPage;
    }

    getWizardSteps(wizardKey) {
        return this.config.wizardName === wizardKey ? this.config.wizardSteps : [];
    }

    getWizardTotalSteps(wizardKey) {
        return this.config.wizardName === wizardKey ? this.config.wizardSteps.length : 0;
    }

    getCurrentComponent(wizardKey, activeStep) {
        return this.config.wizardName === wizardKey ? this.config.wizardSteps[activeStep - 1].component : null;
    }

    getHeaderLabelComponent(wizardKey, activeStep) {
        return this.config.wizardName === wizardKey ? this.config.wizardSteps[activeStep - 1].headerLabelComponent : null;
    }

    getActiveStep(wizardKey) {
        return this.config.wizardName === wizardKey ? this.config.activeStep : 1;
    }

    clickable(wizardKey) {
        if (this.config.wizardName !== wizardKey) {
            return false;
        }

        return this.config.clickable === false ? false : true;
    }
}
