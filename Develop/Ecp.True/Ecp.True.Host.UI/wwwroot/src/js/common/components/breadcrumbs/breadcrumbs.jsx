import React from 'react';
import { connect } from 'react-redux';
import { navigationService } from '../../services/navigationService';
import { scenarioService } from '../../services/scenarioService';
import { resourceProvider } from '../../services/resourceProvider';
import { utilities } from '../../services/utilities';

export class Breadcrumbs extends React.Component {
    onFeatureClick(name, routeKey, preventNavigation) {
        if (preventNavigation) {
            navigationService.navigateToModule(`${name}/${routeKey}`);
        } else {
            navigationService.navigateToModule(`${name}/manage`);
        }
    }

    render() {
        const module = navigationService.getModulePath();
        const navigation = this.props.route.navigation === false ? false : true;
        const feature = navigation ? scenarioService.getFeature(module) : { scenario: this.props.route.scenario };
        const bcrumbsKey = this.props.route.bcrumbsKey;
        const featureKey = this.props.currentModule || feature.description || feature.name;
        return (
            <div className="ep-bcrumbs">
                <span className="ep-bcrumbs__icn"><i className="fas fa-home" /></span>
                <ul className="ep-bcrumbs__lst">
                    <li className="ep-bcrumbs__lst-itm"><a className="ep-bcrumbs__lst-lnk">{resourceProvider.read(feature.scenario)}</a></li>
                    {navigation && <li className="ep-bcrumbs__lst-itm">
                        <a className="ep-bcrumbs__lst-lnk" onClick={() => this.onFeatureClick(feature.name, this.props.route.routeKey, this.props.route.preventNavigation)}>
                            {resourceProvider.read(featureKey)}
                        </a>
                    </li>}
                    { !utilities.isNullOrWhitespace(bcrumbsKey) &&
                        <li className="ep-bcrumbs__lst-itm"><a className="ep-bcrumbs__lst-lnk">{resourceProvider.read(bcrumbsKey)}</a></li>
                    }
                </ul>
            </div >
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        currentModule: state.root.currentModule
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, null, utilities.merge)(Breadcrumbs);


