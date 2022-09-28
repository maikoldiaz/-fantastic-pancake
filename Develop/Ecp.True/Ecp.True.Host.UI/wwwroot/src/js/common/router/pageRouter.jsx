import React from 'react';
import { Route, Switch } from 'react-router-dom';
import ModulePage from './modulePage.jsx';
import { navigationService } from '../services/navigationService.js';
import { authService } from '../services/authService.js';
import { httpService } from '../services/httpService.js';
import { constants } from '../services/constants.js';
import { utilities } from '../services/utilities.js';
import Error from '../components/errors/error.jsx';

export default class PageRouter extends React.Component {
    render() {
        const pages = this.props.routerConfig.getPageNames();
        const authorized = authService.isAuthorized(this.props.routerConfig.getPageRoles());
        return (
            <>
                {authorized &&
                    <Switch>
                        {pages.length > 0 && pages.map(x => {
                            const details = this.props.routerConfig.getPageDetails(x.key);
                            return (
                                details ?
                                    <Route key={x.key} path={`${this.props.match.url}/${x.key}/${this.props.routerConfig.buildNavPath(x.key)}`}
                                        render={routeProps => {
                                            navigationService.initialize(this.props.history, this.props.location, routeProps.match);
                                            return (
                                                <ModulePage route={details} {...routeProps} {...this.props} />
                                            );
                                        }} /> : null
                            );
                        })}
                        {pages.length > 0 && pages.map(x => {
                            const route = this.props.routerConfig.getPageRoute(x.name);
                            return (
                                <Route key={x.key} exact={true}
                                    path={`${this.props.match.url}/${x.key}`}
                                    render={routeProps => {
                                        navigationService.initialize(this.props.history, this.props.location, routeProps.match);
                                        return (
                                            <ModulePage route={route} {...routeProps} {...this.props} />);
                                    }
                                    } />
                            );
                        })}
                    </Switch>
                }
                {!authorized && <Error errorCode={constants.Errors.NoAccess} />}
            </>
        );
    }

    componentDidMount() {
        const pages = this.props.routerConfig.getPageNames();
        const pageName = httpService.getSubModuleName();
        const detailsPageRoute = httpService.getDetailsModuleName();

        if (!pages.find(p => utilities.equalsIgnoreCase(p.key, pageName)) ||
            (!utilities.isNullOrWhitespace(detailsPageRoute) && utilities.isNullOrWhitespace(this.props.routerConfig.getPageDetails(pageName)))) {
            navigationService.handleError(constants.Errors.NotFound);
        }
    }
}
