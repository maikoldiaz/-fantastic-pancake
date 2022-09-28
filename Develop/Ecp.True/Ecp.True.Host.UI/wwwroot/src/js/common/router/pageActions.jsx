import React from 'react';
import { connect } from 'react-redux';
import { constants } from './../services/constants.js';
import { resourceProvider } from '../services/resourceProvider.js';
import { routerActions } from './routerActions.js';
import { openModal, openFlyout, hidePageAction } from './../actions.js';
import { navigationService } from './../services/navigationService.js';
import classNames from 'classnames/bind';

class PageActions extends React.Component {
    constructor() {
        super();
        this.renderAction = this.renderAction.bind(this);
        this.fireAction = this.fireAction.bind(this);
    }

    fireAction(action) {
        if (action.actionType === 'modal') {
            this.props.showModal(action);
        } else if (action.actionType === 'flyout') {
            this.props.openFlyout(action);
        } else if (action.actionType === 'custom') {
            routerActions.fireAction(action.title);
        } else if (action.actionType === 'navigate') {
            routerActions.fireAction(action.title);
            navigationService.navigateToModule(action.route);
        }
    }

    renderDropdown(action) {
        const style = this.props.controlState.enabled ? {} : { pointerEvents: 'none', opacity: '0.4' };
        return (
            <div id={`dd_${action.title}`} className="ep-dropdown m-l-2"
                style={action.name === 'nodeTags' ? style : {}}>
                <div className="ep-dropdown__lbl">
                    <span className="ep-dropdown__txt">
                        <i className={classNames(action.iconClass, 'm-r-1')} />
                        {resourceProvider.read(action.title)}
                    </span>
                    <span className="ep-dropdown__action">
                        <i className="fas fa-angle-down" />
                    </span>
                </div>
                <ul className="ep-dropdown__lst" id={`ul_${action.title}`}>
                    {action.options.filter(x => !this.props.controlState.disabledActions.includes(x.key)).map(v => {
                        return (<li key={v.key} className="ep-dropdown__lst-itm">
                            <a key={v.key + '_lnk'} id={`lnk_${v.key}`} className="ep-dropdown__lst-lnk" onClick={() => this.fireAction(v)}>
                                {!action.noIcon && <i className={classNames(v.iconClass, 'm-r-1')} />}
                                {resourceProvider.read(v.title)}
                            </a>
                        </li>);
                    })}
                </ul>
            </div>);
    }

    renderAction(action) {
        if (this.props.controlState.hiddenActions.includes(action.title)) {
            return null;
        }

        const disabled = this.props.controlState.disabledActions.includes(action.title);

        return (
            <>
                {(action.type === constants.RouterActions.Type.Button) &&
                    <button id={'btn_' + action.title} className={classNames('ep-btn m-l-2', action.classButton)} disabled={disabled}
                        onClick={() => this.fireAction(action)}>
                        {!action.noIcon && <i className={classNames(action.iconClass, 'm-r-1')} />}
                        {!action.noIcon ? <span className={classNames(action.class, 'ep-btn__txt')}>{resourceProvider.read(action.title)}</span> : resourceProvider.read(action.title)}
                    </button>
                }
                {(action.type === constants.RouterActions.Type.Dropdown) &&
                    this.renderDropdown(action)
                }
                {(action.type === constants.RouterActions.Type.Link) &&
                    <button id={'btn_' + action.title} className="ep-btn ep-btn--link m-l-2 m-r-2" onClick={() => this.fireAction(action)}>
                        {!action.noIcon && <i className={classNames(action.iconClass, 'm-r-1')} />}
                        {!action.noIcon ? <span className="ep-btn__txt">{resourceProvider.read(action.title)}</span> : resourceProvider.read(action.title)}
                    </button>
                }
                {(action.type === constants.RouterActions.Type.Component) &&
                    <action.component />
                }
            </>
        );
    }

    render() {
        const props = this.props;
        const actions = props.actions;
        return (
            <>
                {actions && actions.map(v => {
                    return (
                        <React.Fragment key={v.title}>
                            {this.renderAction(v)}
                        </React.Fragment>);
                })}
            </>
        );
    }

    componentDidMount() {
        this.props.actions.filter(a => a.hide === true).forEach(action => this.props.hidePageAction(action.key));
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        controlState: state.pageActions
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        showModal: action => {
            dispatch(openModal(action.key, action.mode));
        },
        openFlyout: action => {
            dispatch(openFlyout(action.key));
        },
        hidePageAction: hideAction => {
            dispatch(hidePageAction(hideAction));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(PageActions);
