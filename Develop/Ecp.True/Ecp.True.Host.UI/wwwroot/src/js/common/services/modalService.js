import { bootstrapService } from './bootstrapService.js';

const modalService = (function () {
    let config = null;
    let currentComponent = null;
    let props = null;
    let isOpen = false;

    return {
        initialize: modalState => {
            const ModalConfig = bootstrapService.getModalConfig();
            if (ModalConfig) {
                config = new ModalConfig();
                currentComponent = config.getCurrentComponent(modalState.modalKey);
                props = config.getProps(modalState.modalKey);
                isOpen = true;
            } else if (modalState.component) {
                currentComponent = modalState.component;
                isOpen = true;
            } else {
                config = null;
            }
        },
        getCurrentComponent: () => {
            return currentComponent.component || currentComponent;
        },
        getTitle: title => {
            return title ? title : currentComponent.title;
        },
        getProps: () => {
            return props;
        },
        reset: () => {
            isOpen = false;
        },
        isOpen: () => {
            return isOpen;
        }
    };
}());

export { modalService };
