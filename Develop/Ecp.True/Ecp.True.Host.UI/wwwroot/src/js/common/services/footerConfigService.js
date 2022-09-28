const footerConfigService = (function () {
    return {
        getCommonConfig: (key, options = {}) => {
            return {
                accept: { key, type: options.acceptType || 'submit', className: options.acceptClassName || 'ep-btn ep-btn--sm', onClick: options.onAccept,
                    text: options.acceptText || 'submit', actions: options.acceptActions || [], disable: options.disableAccept || false, closeModal: options.closeModal },
                cancel: { key, type: 'button', className: options.cancelClassName || 'ep-btn ep-btn--link', onClick: options.onCancel, actions: options.cancelActions || [],
                    disable: options.disableCancel || false, text: options.cancelText || 'cancel' }
            };
        },
        getAcceptConfig: (key, options = {}) => {
            return {
                accept: { key, type: options.acceptType || 'submit', className: options.acceptClassName || 'ep-btn ep-btn--sm', onClick: options.onAccept,
                    text: options.acceptText || 'submit', actions: options.acceptActions || [], disable: options.disableAccept || false, closeModal: options.closeModal }
            };
        },
        getCancelConfig: (key, options = {}) => {
            return {
                cancel: { key, type: 'button', className: options.cancelClassName || 'ep-btn ep-btn--link', onClick: options.onCancel, actions: options.cancelActions || [],
                    disable: options.disableCancel || false, text: options.cancelText || 'cancel' }
            };
        },
        getFlyoutConfig: (key, text, onClick = null, type = 'submit', className = 'ep-btn ep-btn--block') => {
            return { key, type, text, onClick, className };
        }
    };
}());

export { footerConfigService };
