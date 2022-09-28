export default class ModalConfig {
    constructor(config) {
        this.config = config;
    }

    hasConfig(wizardKey) {
        return Object.prototype.hasOwnProperty.call(this.config, wizardKey);
    }

    getCurrentComponent(modalKey) {
        return this.config[modalKey];
    }

    getAllSteps(wizardKey) {
        return this.config[wizardKey].steps;
    }

    isWarning(wizardKey) {
        return Object.prototype.hasOwnProperty.call(this.config[wizardKey], 'isWarning') ? this.config[wizardKey].isWarning : false;
    }

    showSteps(wizardKey) {
        return Object.prototype.hasOwnProperty.call(this.config[wizardKey], 'showSteps') ? this.config[wizardKey].showSteps : true;
    }

    showClose(wizardKey) {
        return Object.prototype.hasOwnProperty.call(this.config[wizardKey], 'showClose') ? this.config[wizardKey].showClose : true;
    }

    getHandleCloseOnIndexes(wizardKey) {
        return Object.prototype.hasOwnProperty.call(this.config[wizardKey], 'handleCloseOnIndexes') ? this.config[wizardKey].handleCloseOnIndexes : [];
    }

    getProps(wizardKey) {
        return Object.prototype.hasOwnProperty.call(this.config[wizardKey], 'props') ? this.config[wizardKey].props : false;
    }
}
