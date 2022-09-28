import React from 'react';
import { BrowserRouter, Route, Switch } from 'react-router-dom';
import { AppBody } from './../layouts/appBody.jsx';

import { navigationService } from './../services/navigationService.js';

export default class AppRouter extends React.Component {
    render() {
        return (
            <BrowserRouter>
                <Switch>
                    <Route render={routeProps => {
                        navigationService.initialize(routeProps.history, routeProps.location, routeProps.match);
                        return <AppBody {...routeProps} />;
                    }} />
                </Switch>
            </BrowserRouter>
        );
    }
}
