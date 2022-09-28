import React from 'react';
import { Provider } from 'react-redux';
import configureStore from './configureStore';
import { dispatcher } from './dispatcher.js';

const configureProvider = function (component, reducer, preloadedState, ...middlewares) {
    const store = configureStore(reducer, preloadedState, middlewares);
    dispatcher.initialize(store.dispatch);

    const provider = (<Provider store={store}>
        <React.Fragment>
            {component}
        </React.Fragment>
    </Provider>);

    return { store, provider };
};

export default configureProvider;
