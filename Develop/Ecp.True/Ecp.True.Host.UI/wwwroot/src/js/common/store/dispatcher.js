const dispatcher = (function () {
    let reduxDispatch = null;

    return {
        initialize: d => {
            reduxDispatch = d;
        },
        dispatch: action => {
            if (reduxDispatch) {
                reduxDispatch(action);
            }
        }
    };
}());

export { dispatcher };
