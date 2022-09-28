import React from 'react';
import { connect } from 'react-redux';
import { utilities } from '../../services/utilities';
import { requestFlowConfig } from '../../actions';
import { powerAutomateService } from '../../services/powerAutomateService';

export const toPowerAutomate = function (key) {
    class PowerAutomate extends React.Component {
        getLocale() {
            return utilities.getLanguage();
        }

        componentDidMount() {
            this.props.requestFlowConfig();
        }

        componentDidUpdate(prevProps) {
            if (prevProps.refreshToggler !== this.props.refreshToggler) {
                this.props.config.type = key;
                powerAutomateService.embedPowerAutomate(this.props.config, this.getLocale());
            }
        }

        render() {
            return (
                <section className="ep-content">
                    <div className="ep-content__body">
                        <section className="ep-section ep-section--panel ep-section--panel-noscroll">
                            <div id="div_flow" className="ep-section__iframe" />
                        </section>
                    </div>
                </section>
            );
        }
    }

    /* istanbul ignore next */
    const mapStateToProps = state => {
        return {
            config: state.powerAutomate[key] ? state.powerAutomate[key].config : null,
            refreshToggler: state.powerAutomate[key] ? state.powerAutomate[key].refreshToggler : false
        };
    };

    /* istanbul ignore next */
    const mapDispatchToProps = dispatch => {
        return {
            requestFlowConfig: () => {
                dispatch(requestFlowConfig(key));
            }
        };
    };

    return connect(mapStateToProps, mapDispatchToProps)(PowerAutomate);
};
