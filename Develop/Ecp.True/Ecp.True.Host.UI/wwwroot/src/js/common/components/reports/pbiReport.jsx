import React from 'react';
import { connect } from 'react-redux';
import { requestReportConfig } from '../../actions.js';
import { pbiService } from '../../services/pbiService.js';
import { utilities } from '../../services/utilities';

// A high order component to generate power BI embedded report.
export const toPbiReport = function (key, filters) {
    class PowerBiReport extends React.Component {
        constructor() {
            super();

            this.rootElement = null;
            this.visual = null;
        }

        componentDidMount() {
            this.props.requestReportConfig();
        }

        render() {
            return (
                <section className="ep-content">
                    <div className="ep-content__body">
                        <section className="ep-section ep-section--panel ep-section--panel-noscroll" id={`rpt_${key}`} ref={el => {
                            this.rootElement = el;
                        }} />
                    </div>
                </section>
            );
        }

        componentDidUpdate(prevProps) {
            if (prevProps.refreshToggler !== this.props.refreshToggler) {
                const embedConfiguration = pbiService.buildEmbedConfig(this.props.config, utilities.getLanguage());
                embedConfiguration.filters = filters;
                window.powerbi.reset(this.rootElement);
                this.visual = window.powerbi.embed(this.rootElement, embedConfiguration);
                window.reportVisual = this.visual;
                this.visual.on('dataSelected', this.props.dataSelected);
            }
        }

        componentWillUnmount() {
            window.powerbi.reset(this.rootElement);
            this.component = null;
        }
    }

    /* istanbul ignore next */
    const mapStateToProps = state => {
        return {
            config: state.reports[key] ? state.reports[key].config : null,
            refreshToggler: state.reports[key] ? state.reports[key].refreshToggler : false
        };
    };

    /* istanbul ignore next */
    const mapDispatchToProps = (dispatch, ownProps) => {
        return {
            requestReportConfig: () => {
                dispatch(requestReportConfig(key));
            },
            dataSelected: e => {
                if (ownProps.onDataSelected) {
                    ownProps.onDataSelected(e);
                }
            }
        };
    };

    /* istanbul ignore next */
    return connect(mapStateToProps, mapDispatchToProps, utilities.merge)(PowerBiReport);
};
