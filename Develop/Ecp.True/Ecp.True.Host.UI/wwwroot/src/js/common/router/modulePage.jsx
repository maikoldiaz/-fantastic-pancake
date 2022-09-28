import React from 'react';
import Breadcrumbs from '../components/breadcrumbs/breadcrumbs.jsx';
import { dispatcher } from '../store/dispatcher';
import PageActions from './pageActions.jsx';
import { hideNotification } from '../actions.js';
import { navigationService } from '../services/navigationService.js';
import { scenarioService } from '../services/scenarioService.js';
import { utilities } from '../services/utilities.js';
import { Error } from '../components/errors/error.jsx';
import { constants } from '../services/constants.js';

export default class ModulePage extends React.Component {
    hasAccess(navigation) {
        const module = navigationService.getModulePath();
        if (navigation === false) {
            return true;
        }

        const feature = scenarioService.getFeature(module);
        return !utilities.isNullOrUndefined(feature);
    }

    render() {
        // Hide any active notification.
        dispatcher.dispatch(hideNotification());

        const props = this.props;
        const Component = this.props.route.component;
        const hasAccess = this.hasAccess(this.props.route.navigation);
        return (
            <>
                {hasAccess &&
                <>
                    <div className="ep-pane">
                        <Breadcrumbs {...props} />
                        <div className="ep-actionbar" key={this.props.route.routeKey ? this.props.route.routeKey : this.props.route.navKey}>
                            {this.props.route.actions && <PageActions key={this.props.route.routeKey ? this.props.route.routeKey : this.props.route.navKey} actions={this.props.route.actions} />}
                        </div>
                    </div>
                    <Component {...this.props} {...this.props.route.props} />
                </>}
                {!hasAccess && <Error errorCode={constants.Errors.NoAccess} />}
            </>
        );
    }
}
