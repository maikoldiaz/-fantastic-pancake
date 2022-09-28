import React from 'react';
import { Route, Switch, Redirect } from 'react-router-dom';
import AppMenu from '../../common/router/appMenu.jsx';
import { bootstrapService } from './../../common/services/bootstrapService.js';
import PageRouter from './../../common/router/pageRouter.jsx';
import { navigationService } from './../../common/services/navigationService.js';
import Notification from './../../common/components/notification/notification.jsx';
import Error from './../components/errors/error.jsx';
import { constants } from '../services/constants.js';
import { utilities } from '../services/utilities.js';

export class AppBody extends React.Component {
    render() {
        const RouterConfig = bootstrapService.getRouterConfig();
        const props = this.props;
        const modulePath = navigationService.getModulePath();
        const defaultRoute = bootstrapService.getDefaultRoute();
        const path = navigationService.getBasePath();
        return (
            <section className="ep-body">
                <AppMenu {...props} />
                <section className="ep-body__content">
                    <Notification isOnModal={false} />
                    <Switch>
                        <Route exact={true} path={`${navigationService.getBasePath()}/error/forbidden`} render={routeProps => {
                            navigationService.initialize(routeProps.history, routeProps.location, routeProps.match);
                            return (
                                <Error {...routeProps} errorCode={constants.Errors.Forbidden} />
                            );
                        }} />
                        <Route exact={true} path={`${navigationService.getBasePath()}/error/unauthorized`} render={routeProps => {
                            navigationService.initialize(routeProps.history, routeProps.location, routeProps.match);
                            return (
                                <Error {...routeProps} errorCode={constants.Errors.NoAccess} />
                            );
                        }} />
                        <Route exact={true} path={`${navigationService.getBasePath()}/error/noaccess`} render={routeProps => {
                            navigationService.initialize(routeProps.history, routeProps.location, routeProps.match);
                            return (
                                <Error {...routeProps} errorCode={constants.Errors.NoAccess} />
                            );
                        }} />
                        <Route exact={true} path={`${navigationService.getBasePath()}/error/notfound`} render={routeProps => {
                            navigationService.initialize(routeProps.history, routeProps.location, routeProps.match);
                            return (
                                <Error {...routeProps} errorCode={constants.Errors.NotFound} />
                            );
                        }} />
                        <Route exact={true} path={`${navigationService.getBasePath()}/error/unknown`} render={routeProps => {
                            navigationService.initialize(routeProps.history, routeProps.location, routeProps.match);
                            return (
                                <Error {...routeProps} errorCode={constants.Errors.ServerError} />
                            );
                        }} />
                        {RouterConfig &&
                            <Route path={`/${modulePath}`} render={routeProps => {
                                return <PageRouter {...props} {...routeProps} routerConfig={new RouterConfig()} />;
                            }} />}
                        <Route exact={true} path="/" render={() => (<Redirect to={`${path}/${defaultRoute}`} />)} />
                    </Switch>
                </section>
            </section>
        );
    }

    componentDidMount() {
        const routerConfig = bootstrapService.getRouterConfig();
        const modulePath = navigationService.getModulePath();
        if (!routerConfig && !utilities.isNullOrWhitespace(modulePath) && !utilities.equalsIgnoreCase(modulePath, 'error')) {
            navigationService.handleError(constants.Errors.NotFound);
        }
    }
}
