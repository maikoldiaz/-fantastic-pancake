import React from 'react';
import thunk from 'redux-thunk';
import ReactDOM from 'react-dom';

import { connect } from 'react-redux';
import { combineReducers } from 'redux';

import AppHeader from './../layouts/appHeader.jsx';
import AppRouter from './../router/appRouter.js';

import Modal from '../components/modal/modal.jsx';
import Loader from '../components/loader/loader.jsx';

import configureProvider from './../store/configureProvider';
import fetchMiddleware from './../middlewares/fetchMiddleware';

import { bootstrapApp, appReady } from './../actions';
import { bootstrapService } from './../services/bootstrapService';
import { navigationService } from './../services/navigationService';
import Shared from '../components/shared.jsx';
import Error from '../components/errors/error.jsx';
import { constants } from '../services/constants.js';

export class App extends React.Component {
    render() {
        return (
            <>
                <AppHeader />
                {this.props.ready &&
                <>
                    <AppRouter />
                    <Shared />
                </>}
                {this.props.error &&
                <section className="ep-body">
                    <section className="ep-body__content">
                        <Error errorCode={constants.Errors.ServerError} />
                    </section>
                </section>}
                <div id="overlays">
                    <Modal />
                    <Loader />
                </div>
                <div id="epPortals" />
            </>
        );
    }

    componentDidMount() {
        if (navigationService.isNotAuthorized()) {
            this.props.appReady();
        } else {
            this.props.bootstrapApp();
        }
    }

    componentDidUpdate(prevProps) {
        if (prevProps.appToggler !== this.props.appToggler) {
            bootstrapService.bootstrap(this.props.context);
            this.props.appReady();
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        appToggler: state.root.appToggler,
        ready: state.root.appReady,
        context: state.root.context,
        error: state.root.error
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        bootstrapApp: () => {
            dispatch(bootstrapApp());
        },
        appReady: () => {
            dispatch(appReady());
        }
    };
};

/* istanbul ignore next */
export const AppComponent = connect(mapStateToProps, mapDispatchToProps)(App);

/* istanbul ignore next */
export const appInitFactory = () => {
    const appElement = document.getElementById('true_app');

    if (appElement) {
        const { provider } = configureProvider(<AppComponent />, combineReducers(bootstrapService.getAllReducers()), {}, thunk, fetchMiddleware);
        ReactDOM.render(provider, appElement);
    }
};
