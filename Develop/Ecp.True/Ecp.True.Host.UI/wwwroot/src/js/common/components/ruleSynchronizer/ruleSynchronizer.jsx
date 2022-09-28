import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../services/resourceProvider';
import { utilities } from '../../services/utilities';
import classNames from 'classnames/bind';
import { syncRules, initSyncRules, requestSyncProgress, openMessageModal } from '../../actions';
import { constants } from '../../services/constants';

export class RuleSynchronizer extends React.Component {
    constructor() {
        super();
        this.syncRules = this.syncRules.bind(this);
    }

    syncRules() {
        if (this.timer) {
            clearInterval(this.timer);
        }

        this.props.syncRules();
    }

    render() {
        return (
            <div id={`ruleSynchronizer_${this.props.state}`} className={`ep-notification ep-notification--${this.props.state} ep-notification--sync`}>
                <span className="ep-notification__icn">
                    <i className="fas fa-info-circle" />
                </span>
                <div className="ep-notification__content">
                    <div className={classNames('ep-notification__info', { ['ep-notification__info--sm']: this.props.isButton })}>
                        <h1 className="ep-notification__title">{resourceProvider.read('cacheRefreshTitle')}</h1>
                        <div className="ep-notification__msg">
                            {resourceProvider.read(`${this.props.state}CacheRefreshMessage`)}
                        </div>
                    </div>
                    <div className="ep-notification__action">
                        <button className="ep-btn ep-btn--sm" onClick={this.syncRules} disabled={!this.props.enabled}>
                            <i className="fas m-r-1 fa-redo" /><span id={`btn_${this.props.name}_ruleSynchronizer`} className="ep-btn__txt">{resourceProvider.read('updateStrategies')}</span>
                        </button>
                    </div>
                </div>
            </div>
        );
    }

    componentDidMount() {
        this.props.initSyncRules(this.props.name);
    }

    componentDidUpdate(prevProps) {
        if (this.props.timerToggler !== prevProps.timerToggler) {
            this.timer = setInterval(() => {
                this.props.requestSyncProgress();
            }, constants.Timeouts.Synchronizer);
        }

        if (this.props.errorToggler !== prevProps.errorToggler) {
            this.props.showError();
        }
    }

    componentWillUnmount() {
        if (this.timer) {
            clearInterval(this.timer);
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = (state, ownProps) => {
    return {
        state: state.ruleSynchronizer[ownProps.name] ? state.ruleSynchronizer[ownProps.name].state : constants.SyncStatus.NotReady,
        enabled: state.ruleSynchronizer[ownProps.name] ? state.ruleSynchronizer[ownProps.name].enabled : false,
        timerToggler: state.ruleSynchronizer[ownProps.name] ? state.ruleSynchronizer[ownProps.name].timerToggler : undefined,
        errorToggler: state.ruleSynchronizer[ownProps.name] ? state.ruleSynchronizer[ownProps.name].errorToggler : undefined
    };
};

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    return {
        initSyncRules: () => {
            dispatch(initSyncRules(ownProps.name));
        },
        requestSyncProgress: () => {
            dispatch(requestSyncProgress(ownProps.name));
        },
        syncRules: () => {
            dispatch(syncRules(ownProps.name));
        },
        showError: () => {
            dispatch(openMessageModal(resourceProvider.read('syncInProgressErrorMessage'), {
                title: 'error',
                canCancel: false
            }));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(RuleSynchronizer);

