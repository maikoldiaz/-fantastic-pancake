import React from 'react';
import { connect } from 'react-redux';
import { NavLink } from 'react-router-dom';
import { bootstrapService } from '../services/bootstrapService';
import { toggleMenuItem, toggleMenu } from '../../common/actions';
import { resourceProvider } from './../../common/services/resourceProvider';
import { scenarioService } from '../services/scenarioService';
import { navigationService } from '../services/navigationService';
import classNames from 'classnames/bind';
import { utilities } from '../services/utilities';

class AppMenu extends React.Component {
    isActive(feature) {
        return feature && navigationService.getModulePath() === feature.toLowerCase();
    }

    render() {
        const scenarios = Object.values(this.props.scenarios);
        return (
            <>
                {scenarios.length > 0 && <aside className={classNames('ep-body__aside', { ['ep-body__aside--expand']: this.props.isOpen })}>
                    <nav className="ep-navbar">
                        <span className="ep-navbar__toggler" onClick={() => this.props.toggleMenu(this.props.isOpen)}>
                            <i className="fas fa-angle-double-right" />
                        </span>
                        <ul className={classNames('ep-nav', { ['ep-nav--expand']: this.props.isOpen })}>
                            {scenarios.map(item => {
                                return (
                                    <li key={item.scenario} className="ep-nav__itm">
                                        <a className="ep-nav__lnk" onClick={() => item.isCollapsible && this.props.toggleMenuItem(item.scenario)}>
                                            <span className="ep-nav__itm-icn"><i className={`ep-icn ep-icn--${item.scenario}`} /></span>
                                            <span className="ep-nav__lnk-txt">{resourceProvider.read(item.scenario)}</span>
                                            {item.isCollapsible && <span className={classNames('ep-nav__toggler', { ['ep-nav__toggler--open']: !item.isCollapsed })}>
                                                <i className="fas fa-chevron-right" />
                                            </span>}
                                        </a>
                                        <ul className={classNames('ep-nav ep-nav--sub', { ['ep-nav--collapsed']: item.isCollapsed })}>
                                            {item.features &&
                                                item.features.map(feature => {
                                                    return (<li key={feature.name} className="ep-nav__itm">
                                                        <NavLink to={`/${bootstrapService.getRoute(feature.name)}`}
                                                            isActive={(match, location) => bootstrapService.isActive(match, location, feature.name)}
                                                            className="ep-nav__lnk" href="" activeClassName="ep-nav__lnk--active">
                                                            <span className="ep-nav__lnk-txt">{feature.description}</span>
                                                        </NavLink>
                                                    </li>);
                                                })
                                            }
                                        </ul>
                                    </li>);
                            })}
                        </ul>
                    </nav>
                </aside>}
            </>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        isOpen: state.root.isOpen,
        scenarios: state.root.scenarios
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        toggleMenuItem: scenario => {
            dispatch(toggleMenuItem(scenario));
        },
        toggleMenu: isOpen => {
            const feature = scenarioService.getFeature(navigationService.getModulePath());
            if (isOpen || utilities.isNullOrUndefined(feature)) {
                dispatch(toggleMenu());
            } else {
                dispatch(toggleMenuItem(feature.scenario));
            }
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(AppMenu);
